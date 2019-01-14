using ProtoBuf;

namespace CSM.Commands
{
    [ProtoContract]
    public class TransportReleaseLineCommand : CommandBase
    {
        [ProtoMember(1)]
        public ushort LineID { get; set; }
    }
}
