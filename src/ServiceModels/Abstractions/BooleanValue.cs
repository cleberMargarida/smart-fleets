namespace ServiceModels.Abstractions;

public abstract class BooleanValue : Signal<bool>, ISignalValueAdapter<bool>
{
    public bool Adapt(object source) => Convert.ToBoolean(source);
}

