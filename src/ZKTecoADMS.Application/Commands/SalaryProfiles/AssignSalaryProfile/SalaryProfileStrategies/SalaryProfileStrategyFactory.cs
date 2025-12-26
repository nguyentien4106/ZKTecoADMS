using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.SalaryProfiles.AssignSalaryProfile.SalaryProfileStrategies;

/// <summary>
/// Factory for creating salary profile assignment strategies
/// </summary>
public class SalaryProfileStrategyFactory
{
    private readonly Dictionary<SalaryRateType, ISalaryProfileAssignmentStrategy> _strategies;

    public SalaryProfileStrategyFactory(IEnumerable<ISalaryProfileAssignmentStrategy> strategies)
    {
        _strategies = strategies.ToDictionary(s => s.RateType, s => s);
    }

    /// <summary>
    /// Gets the appropriate strategy for the given rate type
    /// </summary>
    public ISalaryProfileAssignmentStrategy GetStrategy(SalaryRateType rateType)
    {
        if (_strategies.TryGetValue(rateType, out var strategy))
        {
            return strategy;
        }

        throw new InvalidOperationException($"No strategy found for rate type: {rateType}");
    }
}
