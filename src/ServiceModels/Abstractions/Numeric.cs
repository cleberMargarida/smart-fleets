namespace ServiceModels.Abstractions;

public abstract class Numeric : Signal<double>, ISignalValueAdapter<double>
{
    public double Adapt(object source) => Convert.ToDouble(source);
}

