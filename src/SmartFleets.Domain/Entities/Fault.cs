using ServiceModels.Binding;

namespace SmartFleets.Domain.Entities;

/// <summary>
/// Represents a fault event in the SmartFleets system.
/// </summary>
public class Fault
{
    /// <summary>
    /// Gets or sets the unique identifier for the fault.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the description of the fault.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the vehicle associated with the fault.
    /// </summary>
    public required string VehicleId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the signal that triggered the fault.
    /// </summary>
    public required string SignalId { get; set; }

    /// <summary>
    /// Gets or sets the list of signal types associated with the fault.
    /// </summary>
    public required IList<SignalType> SignalTypes { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the fault was created.
    /// </summary>
    public required DateTime CreatedAt { get; set; }
}
