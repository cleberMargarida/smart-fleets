namespace ServiceModels.Abstractions;

[GenerateSerializer]
public abstract class Numeric : Signal<double>, ISignalValueAdapter<double>
{
    public double Adapt(object source)
    {
        return Convert.ToDouble(source);
    }
}

