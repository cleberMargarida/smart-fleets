using ServiceModels;
using ServiceModels.Binding;
using System.Linq.Dynamic.Core.Parser;
using System.Linq.Expressions;

namespace SmartFleets.Domain.Entities;

/// <summary>
/// Represents the metadata for a fault in the SmartFleets system.
/// </summary>
public class FaultMetadata
{
    private static readonly ParameterExpression _parameter = Expression.Parameter(typeof(VehicleState), "state");

    /// <summary>
    /// Gets or sets the unique identifier for the fault metadata.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the description of the fault.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the fault is enabled.
    /// </summary>
    public required bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the predicate as a string that defines the fault.
    /// </summary>
    public required string PredicateAsString { get; set; }

    /// <summary>
    /// Gets or sets the list of signal types associated with the fault.
    /// </summary>
    public required IList<SignalType> SignalTypes { get; set; } = new List<SignalType>();

    /// <summary>
    /// Simulates the fault with the given vehicle state.
    /// </summary>
    /// <param name="state">The vehicle state to simulate the fault against.</param>
    /// <returns>true if the fault is met; otherwise, false.</returns>
    public bool Simulate(VehicleState state)
    {
        return GetFunction().Invoke(state);
    }

    private Func<VehicleState, bool> GetFunction()
    {
        return GetPredicate().Compile();
    }

    private Expression<Func<VehicleState, bool>> GetPredicate()
    {
        return Expression.Lambda<Func<VehicleState, bool>>(GetBody(), _parameter);
    }

    private Expression GetBody()
    {
        return GetParser().Parse(typeof(bool));
    }

    private ExpressionParser GetParser()
    {
        return new ExpressionParser(new [] {_parameter}, PredicateAsString, new object?[] {_parameter}, null);
    }
}
