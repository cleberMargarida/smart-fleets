namespace IngestionAPI.GrainInterfaces
{
    /// <summary>
    /// Represents a grain interface that extends <see cref="IGrainWithStringKey"/> to include a vehicle ID key.
    /// </summary>
    public interface IGrainWithVehicleIdKey : IGrainWithStringKey
    {
    }
}
