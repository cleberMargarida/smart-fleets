namespace ServiceModels.Abstractions;

[GenerateSerializer]
public abstract class BooleanValue : Signal<bool>, ISignalValueAdapter<bool>
{
    public bool Adapt(object source)
    {
        return Convert.ToBoolean(source);
    }
}

