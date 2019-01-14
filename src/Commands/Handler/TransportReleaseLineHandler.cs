using ColossalFramework;
using CSM.Injections;
using CSM.Networking;

namespace CSM.Commands.Handler
{
    public class TransportReleaseLineHandler : CommandHandler<TransportReleaseLineCommand>
    {
        public override byte ID => CommandIds.TransportReleaseLineCommand;

        public override void HandleOnServer(TransportReleaseLineCommand command, Player player) => Handle(command);

        public override void HandleOnClient(TransportReleaseLineCommand command) => Handle(command);

        private void Handle(TransportReleaseLineCommand command)
        {
            TransportHandler.IgnoreLines.Add(command.LineID);
            TransportManager.instance.ReleaseLine(command.LineID);
            TransportHandler.IgnoreLines.Remove(command.LineID);
        }

    }
}
