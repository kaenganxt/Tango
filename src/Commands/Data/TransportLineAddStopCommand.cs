using ProtoBuf;
using UnityEngine;

namespace CSM.Commands
{
    [ProtoContract]
    public class TransportLineAddStopCommand : CommandBase
    {
        [ProtoMember(1)]
        public ushort LineID;

        [ProtoMember(2)]
        public int index;

        [ProtoMember(3)]
        public Vector3 Position;

        [ProtoMember(4)]
        public bool fixedPlatform;


    }
}
