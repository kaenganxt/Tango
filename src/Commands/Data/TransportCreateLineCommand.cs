using ProtoBuf;


namespace CSM.Commands
{
    [ProtoContract]
    public class TransportCreateLineCommand : CommandBase
    {
        [ProtoMember(1)]
        public ushort lineID;

        [ProtoMember(2)]
        public bool newNumber;

        [ProtoMember(3)]
        public ushort infoIndex;
    }
}
