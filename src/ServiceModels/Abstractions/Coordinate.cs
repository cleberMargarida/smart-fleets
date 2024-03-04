namespace ServiceModels.Abstractions;

/// <summary>
/// Represents a signal value that is a geographic coordinate consisting of latitude and longitude.
/// </summary>
[GenerateSerializer]
public abstract class Coordinate : Signal<(double Latitude, double Longitude)>, ISignalValueAdapter<(double Latitude, double Longitude)>
{
    /// <summary>
    /// Adapts an object to a coordinate value.
    /// </summary>
    /// <param name="source">The object to adapt, expected to be a dictionary with latitude and longitude values.</param>
    /// <returns>The adapted coordinate value as a tuple (Latitude, Longitude).</returns>
    /// <exception cref="InvalidOperationException">Thrown if the source object cannot be adapted to a coordinate.</exception>
    public (double Latitude, double Longitude) Adapt(object source)
    {
        return source is Dictionary<int, double> values ?
            (Latitude: values[0], Longitude: values[1]) : throw new InvalidOperationException("Invalid source for coordinate adaptation.");
    }
}
