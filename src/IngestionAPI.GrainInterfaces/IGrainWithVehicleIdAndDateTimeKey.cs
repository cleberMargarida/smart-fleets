namespace IngestionAPI.GrainInterfaces
{
    /// <summary>
    /// Represents a grain interface that extends <see cref="IGrainWithVehicleIdKey"/> to include a date and time key.
    /// </summary>
    public interface IGrainWithVehicleIdAndDateTimeKey : IGrainWithVehicleIdKey
    {
    }
}
