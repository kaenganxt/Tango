using ProtoBuf;
using UnityEngine;


namespace CSM.Commands
{
    [ProtoContract]
    public class TransportLineRemoveStopCommand : CommandBase
    {
        [ProtoMember(1)]
        public ushort LineID;

        [ProtoMember(2)]
        public int Index;
    }
}
