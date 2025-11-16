using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Core.Services;

/// <summary>
/// Service for parsing and processing user data from device OPERLOG data.
/// </summary>
public class UserOperationService(ILogger<UserOperationService> logger) : IUserOperationService
{
    // Field identifiers based on protocol
    private const string USER_PREFIX = "USER";
    private const string PIN_KEY = "PIN";
    private const string NAME_KEY = "Name";
    private const string PRI_KEY = "Pri";
    private const string PASSWORD_KEY = "Passwd";
    private const string CARD_KEY = "Card";
    private const string GROUP_KEY = "Grp";
    private const string TIMEZONE_KEY = "TZ";
    private const string VERIFY_KEY = "Verify";
    private const string VICECARD_KEY = "ViceCard";
    private const int MIN_FIELDS = 6; // At least PIN, Name, Passwd, Card, Grp, and Pri

    /// <summary>
    /// Parses and processes user data from device OPERLOG format.
    /// </summary>
    public async Task<List<User>> ProcessUsersFromDeviceAsync(Device device, string body)
    {
        var userLines = ExtractUserLines(body);
        logger.LogInformation("Processing {Count} user records from device {DeviceId}",
            userLines.Count, device.Id);

        var users = await ProcessUserLinesAsync(device, userLines);
        logger.LogInformation("Successfully processed {Count} user records from device {DeviceId}",
            users.Count, device.Id);

        return users;
    }

    private static List<string> ExtractUserLines(string body)
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
                logger.LogError(ex, "Unexpected error processing user line from device {DeviceId}: {Line}",
                    device.Id, line);
            }
        }

        return users;
    }

    private Task<User?> TryProcessUserLineAsync(Device device, string line)
    {
        var userFields = ParseUserLine(line);

        if (userFields == null || !ValidateUserFields(userFields))
        {
            return Task.FromResult<User?>(null);
        }

        return Task.FromResult(ExtractUserData(userFields, device.Id));
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
                if (keyValue.Length != 2) continue;
                var key = keyValue[0].Trim();
                var value = keyValue[1].Trim();
                fields[key] = value;
            }

            return fields;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error parsing user line: {Line}", line);
            return null;
        }
    }

    private bool ValidateUserFields(Dictionary<string, string> fields)
    {
        if (!fields.TryGetValue(PIN_KEY, out var pinValue) || string.IsNullOrWhiteSpace(pinValue))
        {
            logger.LogWarning("User record missing required PIN field");
            return false;
        }

        if (!fields.TryGetValue(NAME_KEY, out var nameValue) || string.IsNullOrWhiteSpace(nameValue))
        {
            logger.LogWarning("User record missing required Name field for PIN: {PIN}",
                fields.GetValueOrDefault(PIN_KEY));
            return false;
        }

        if (fields.Count >= MIN_FIELDS) 
            return true;
        
        logger.LogWarning("User record has fewer than minimum required fields ({Min}). PIN: {PIN}",
            MIN_FIELDS, fields.GetValueOrDefault(PIN_KEY));
        return false;

    }

    private User? ExtractUserData(Dictionary<string, string> fields, Guid deviceId)
    {
        try
        {
            var user = new User
            {
                Pin = ExtractField(fields, PIN_KEY, string.Empty),
                Name = ExtractField(fields, NAME_KEY, string.Empty)!,
                Password = ExtractField(fields, PASSWORD_KEY),
                CardNumber = ExtractField(fields, CARD_KEY),
                GroupId = ExtractIntField(fields, GROUP_KEY, 1),
                Privilege = ExtractIntField(fields, PRI_KEY),
                DeviceId = deviceId,
                IsActive = true
            };
            return user;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error extracting user data from fields");
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
            logger.LogDebug("Failed to parse integer field {Key}: {Value}. Using default: {Default}",
                key, value, defaultValue);
        }

        return defaultValue;
    }
}
