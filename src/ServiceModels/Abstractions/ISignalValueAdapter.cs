namespace ServiceModels;

public interface ISignalValueAdapter
{
    object Adapt(object source);
}

public interface ISignalValueAdapter<out T> : ISignalValueAdapter
{
#pragma warning disable CS8603
    object ISignalValueAdapter.Adapt(object source) => Adapt(source);
#pragma warning restore CS8603

    new T Adapt(object source);
}

