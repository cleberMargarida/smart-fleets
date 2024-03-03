namespace ServiceModels.Abstractions;

[GenerateSerializer]
public abstract class Coordinate : Signal<(double Latitude, double Longitude)>, ISignalValueAdapter<(double Latitude, double Longitude)>
{
    public (double Latitude, double Longitude) Adapt(object source)
    {
        return source is Dictionary<int, double> values ?
            (Latitude: values[0], Longitude: values[1]) : throw new InvalidOperationException("source");
    }
}

