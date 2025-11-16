using MediatR;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Behaviours;

/// <summary>
/// Pipeline behavior to check if a device with the given SN is active before processing iClock requests.
/// This behavior intercepts requests that implement IIClockRequest and validates device status.
/// </summary>
public class DeviceActiveCheckBehaviour<TRequest, TResponse>(
    IDeviceService deviceService,
    ILogger<DeviceActiveCheckBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IIClockRequest
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var serialNumber = request.SN;

        logger.LogInformation(
            "[Device Active Check] Validating device with SN: {SerialNumber} for request: {RequestType}",
            serialNumber,
            typeof(TRequest).Name);

        // Check if device exists
        var device = await deviceService.GetDeviceBySerialNumberAsync(serialNumber);
        
        if (device == null)
        {
            logger.LogWarning(
                "[Device Active Check] Device with SN: {SerialNumber} not found. Rejecting request.",
                serialNumber);
            
            return CreateRejectionResponse();
        }

        // Check if device is active
        if (!device.IsActive)
        {
            logger.LogWarning(
                "[Device Active Check] Device with SN: {SerialNumber} (ID: {DeviceId}) is inactive. Rejecting request.",
                serialNumber,
                device.Id);
            
            return CreateRejectionResponse();
        }

        logger.LogInformation(
            "[Device Active Check] Device with SN: {SerialNumber} (ID: {DeviceId}) is active. Proceeding with request.",
            serialNumber,
            device.Id);

        // Device is active, proceed with the request
        return await next();
    }

    /// <summary>
    /// Creates a rejection response based on the response type.
    /// For string responses (typical for iClock protocol), returns ClockResponses.Ok
    /// to prevent device from retrying indefinitely while blocking the request.
    /// </summary>
    private static TResponse CreateRejectionResponse()
    {
        var responseType = typeof(TResponse);

        // For string responses (typical iClock protocol responses)
        if (responseType == typeof(string))
        {
            // Return OK to prevent device from retrying, but the actual request is not processed
            return (TResponse)(object)ClockResponses.Fail;
        }

        // For other response types, return default
        return default!;
    }
}

/// <summary>
/// Marker interface for iClock requests that require device active check.
/// Implement this interface on commands/queries that need device validation.
/// </summary>
public interface IIClockRequest
{
    /// <summary>
    /// The device Serial Number (SN) to validate
    /// </summary>
    string SN { get; }
}
