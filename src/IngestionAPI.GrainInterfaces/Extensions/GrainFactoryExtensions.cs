using IngestionAPI.GrainInterfaces;

namespace Orleans
{
    public static class GrainFactoryExtensions
    {
        public static T GetGrain<T>(this IGrainFactory factory, string vehicleId) where T : IGrainWithVehicleIdKey
        {
            return factory.GetGrain<T>(vehicleId);
        }

        public static T GetGrain<T>(this IGrainFactory factory, string vehicleId, DateTime dateTimeUtc) where T : IGrainWithVehicleIdAndDateTimeKey
        {
            //TODO to consider ranges different than second first digit we can use the path below.
            //int number = 16; // Change this to the number you want to round
            //int rounded = ((number + 2) / 5) * 5; // This will round the number to the nearest multiple of 5
            // Will group all ocurred events grain related within 10s interval.
            return factory.GetGrain<T>($"{vehicleId}/{dateTimeUtc:yyyy-MM-dd HH:mm:s}{dateTimeUtc.Second/10}");
        }
    }
}
