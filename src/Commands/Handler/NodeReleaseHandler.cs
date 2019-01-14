using ColossalFramework;
using CSM.Injections;
using CSM.Networking;

namespace CSM.Commands.Handler
{
    public class NodeReleaseHandler : CommandHandler<NodeReleaseCommand>
    {
        public override byte ID => CommandIds.NodeReleaseCommand;

        public override void HandleOnServer(NodeReleaseCommand command, Player player) => Handle(command);

        public override void HandleOnClient(NodeReleaseCommand command) => Handle(command);

        private void Handle(NodeReleaseCommand command)
        {
            NodeHandler.IgnoreAll++;
            Singleton<NetManager>.instance.ReleaseNode(command.NodeId);
            NodeHandler.IgnoreAll--;
        }
    }
}