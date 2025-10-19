using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ZKTecoADMS.Core.Behaviours;

public class LoggerBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggerBehaviour<TRequest, TResponse>> _logger;

    public LoggerBehaviour(ILogger<LoggerBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var uniqueId = Guid.NewGuid().ToString();

        _logger.LogInformation(
            "[START] {RequestName} [{UniqueId}] {@Request}",
            requestName, uniqueId, request);

        var timer = new Stopwatch();
        timer.Start();

        try
        {
            var response = await next();
            timer.Stop();

            _logger.LogInformation(
                "[END] {RequestName} [{UniqueId}] completed in {ElapsedMilliseconds}ms {@Response}",
                requestName, uniqueId, timer.ElapsedMilliseconds, response);

            return response;
        }
        catch (Exception ex)
        {
            timer.Stop();
            _logger.LogError(
                ex,
                "[ERROR] {RequestName} [{UniqueId}] failed in {ElapsedMilliseconds}ms {@Request}",
                requestName, uniqueId, timer.ElapsedMilliseconds, request);
            throw;
        }
    }
}