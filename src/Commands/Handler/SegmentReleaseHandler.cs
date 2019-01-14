using ColossalFramework;
using CSM.Injections;
using CSM.Networking;

namespace CSM.Commands.Handler
{
    public class SegmentReleaseHandler : CommandHandler<SegmentReleaseCommand>
    {
        public override byte ID => CommandIds.SegmentReleaseCommand;

        public override void HandleOnServer(SegmentReleaseCommand command, Player player) => Handle(command);

        public override void HandleOnClient(SegmentReleaseCommand command) => Handle(command);

        private void Handle(SegmentReleaseCommand command)
        {
            NodeHandler.IgnoreAll++;
            Singleton<NetManager>.instance.ReleaseSegment(command.SegmentId, command.KeepNodes);
            NodeHandler.IgnoreAll--;
        }
    }
}