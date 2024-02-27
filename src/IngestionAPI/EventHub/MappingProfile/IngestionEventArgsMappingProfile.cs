using AutoMapper;
using Azure.Messaging.EventHubs.Processor;
using MessagePack;
using IngestionAPI.Models;

namespace IngestionAPI.EventHub.MappingProfile
{
    public class IngestionEventArgsMappingProfile : Profile
    {
        public IngestionEventArgsMappingProfile()
        {
            CreateMap<ProcessEventArgs, Message>().ConstructUsing(Conversor);
        }

        private Message Conversor(ProcessEventArgs args, ResolutionContext _) =>
            MessagePackSerializer.Deserialize<Message>(args.Data.EventBody.ToMemory());
    }
}
