using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Commands.IClock.CDataPost.Strategy;

/// <summary>
/// Handles OPERLOG data (user information) uploads from device to server.
/// Format: USER PIN=%s\tName=%s\tPasswd=%d\tCard=%d\tGrp=%d\tTZ=%s
/// </summary>
public class OperLogStrategy(IServiceProvider serviceProvider) : IPostStrategy
{
    private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    private readonly ILogger<OperLogStrategy> _logger = serviceProvider.GetRequiredService<ILogger<OperLogStrategy>>();
    // Field identifiers based on protocol
    private const string USER_PREFIX = "USER";
    private const string PIN_KEY = "PIN";
    private const string NAME_KEY = "Name";
    private const string PASSWORD_KEY = "Passwd";
    private const string CARD_KEY = "Card";
    private const string GROUP_KEY = "Grp";
    private const string TIMEZONE_KEY = "TZ";
    private const int MIN_FIELDS = 6; // At least PIN, Name, Passwd, Card, Grp, and TZ
    public async Task<string> ProcessDataAsync(Device device, string body)
    {
        var userLines = ExtractUserLines(body);
        _logger.LogInformation("Processing {Count} user records from device {DeviceId}",
            userLines.Count, device.Id);
            
        var users = await ProcessUserLinesAsync(device, userLines);
        _logger.LogInformation("Successfully processed {Count} user records from device {DeviceId}",
            users.Count, device.Id);
            
        if (users.Count == 0)
        {
            _logger.LogWarning("No valid user records to save from device {DeviceId}", device.Id);
            return ClockResponses.Ok;
        }

        await SaveUsersAsync(users, device.DeviceName);
        return ClockResponses.Ok;
    }

    private List<string> ExtractUserLines(string body)
    {
        return body.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries)
                   .Where(line => !string.IsNullOrWhiteSpace(line) && line.TrimStart().StartsWith(USER_PREFIX))
                   .ToList();
    }

    private async Task<List<User>> ProcessUserLinesAsync(Device device, List<string> userLines)
    {
        var users = new List<User>();

        foreach (var line in userLines)
        {
            try
            {
                var user = await TryProcessUserLineAsync(device, line);
                if (user != null)
                {
                    users.Add(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error processing user line from device {DeviceId}: {Line}",
                    device.Id, line);
            }
        }

        return users;
    }

    private async Task SaveUsersAsync(List<User> users, string deviceName)
    {
        foreach (var user in users)
        {
            await _userRepository.AddOrUpdateAsync(user);
        }
    }

    private async Task<User?> TryProcessUserLineAsync(Device device, string line)
    {
        var userFields = ParseUserLine(line);
        
        if (userFields == null || !ValidateUserFields(userFields))
        {
            return null;
        }

        var userData = ExtractUserData(userFields);
        if (userData == null)
        {
            _logger.LogWarning("Failed to extract user data from line: {Line}", line);
            return null;
        }

        return await CreateOrUpdateUserAsync(device, userData);
    }

    /// <summary>
    /// Parses a user line into key-value pairs.
    /// Format: USER PIN=982\tName=Richard\tPasswd=9822\tCard=13375590\tGrp=1\tTZ=
    /// </summary>
    private Dictionary<string, string>? ParseUserLine(string line)
    {
        try
        {
            var fields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            
            // Remove "USER " prefix and split by tabs
            var content = line.Substring(USER_PREFIX.Length).TrimStart();
            var parts = content.Split('\t', StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                var keyValue = part.Split('=', 2);
                if (keyValue.Length == 2)
                {
                    var key = keyValue[0].Trim();
                    var value = keyValue[1].Trim();
                    fields[key] = value;
                }
            }

            return fields;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing user line: {Line}", line);
            return null;
        }
    }

    private bool ValidateUserFields(Dictionary<string, string> fields)
    {
        if (!fields.ContainsKey(PIN_KEY) || string.IsNullOrWhiteSpace(fields[PIN_KEY]))
        {
            _logger.LogWarning("User record missing required PIN field");
            return false;
        }

        if (!fields.ContainsKey(NAME_KEY) || string.IsNullOrWhiteSpace(fields[NAME_KEY]))
        {
            _logger.LogWarning("User record missing required Name field for PIN: {PIN}", 
                fields.GetValueOrDefault(PIN_KEY));
            return false;
        }

        if (fields.Count < MIN_FIELDS)
        {
            _logger.LogWarning("User record has fewer than minimum required fields ({Min}). PIN: {PIN}",
                MIN_FIELDS, fields.GetValueOrDefault(PIN_KEY));
            return false;
        }

        return true;
    }

    private UserData? ExtractUserData(Dictionary<string, string> fields)
    {
        try
        {
            return new UserData
            {
                PIN = ExtractField(fields, PIN_KEY, string.Empty)!,
                Name = ExtractField(fields, NAME_KEY, string.Empty)!,
                Password = ExtractField(fields, PASSWORD_KEY),
                CardNumber = ExtractField(fields, CARD_KEY),
                GroupId = ExtractIntField(fields, GROUP_KEY, 1),
                TimeZone = ExtractField(fields, TIMEZONE_KEY)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extracting user data from fields");
            return null;
        }
    }

    private string? ExtractField(Dictionary<string, string> fields, string key, string? defaultValue = null)
    {
        if (fields.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value))
        {
            return value;
        }
        return defaultValue;
    }

    private int ExtractIntField(Dictionary<string, string> fields, string key, int defaultValue = 0)
    {
        if (fields.TryGetValue(key, out var value) && int.TryParse(value, out var intValue))
        {
            return intValue;
        }

        if (!string.IsNullOrWhiteSpace(value))
        {
            _logger.LogDebug("Failed to parse integer field {Key}: {Value}. Using default: {Default}",
                key, value, defaultValue);
        }

        return defaultValue;
    }

    private async Task<User?> CreateOrUpdateUserAsync(Device device, UserData userData)
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetUserByPinAsync(userData.PIN);

        if (existingUser != null)
        {
            // Update existing user
            UpdateUserProperties(existingUser, userData);
            _logger.LogDebug("Updated existing user with PIN: {PIN}", userData.PIN);
            return existingUser;
        }

        // Create new user
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            DeviceId = device.Id,
            PIN = userData.PIN,
            FullName = userData.Name,
            Password = userData.Password,
            CardNumber = userData.CardNumber,
            GroupId = userData.GroupId,
            IsActive = true,
            Privilege = 0, // Default user privilege
            VerifyMode = 0 // Will be set by device
        };

        return newUser;
    }

    private void UpdateUserProperties(User user, UserData userData)
    {
        user.FullName = userData.Name;
        
        if (!string.IsNullOrWhiteSpace(userData.Password))
        {
            user.Password = userData.Password;
        }
        
        if (!string.IsNullOrWhiteSpace(userData.CardNumber))
        {
            user.CardNumber = userData.CardNumber;
        }
        
        user.GroupId = userData.GroupId;
        user.IsActive = true;
    }

    /// <summary>
    /// Internal data transfer object for parsed user data
    /// </summary>
    private class UserData
    {
        public required string PIN { get; set; }
        public required string Name { get; set; }
        public string? Password { get; set; }
        public string? CardNumber { get; set; }
        public int GroupId { get; set; } = 1;
        public string? TimeZone { get; set; }
    }
}