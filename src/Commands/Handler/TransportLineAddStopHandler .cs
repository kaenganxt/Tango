using ColossalFramework;
using CSM.Injections;
using CSM.Networking;

namespace CSM.Commands.Handler
{
    public class TransportLineAddStopHandler : CommandHandler<TransportLineAddStopCommand>
    {

        public override byte ID => CommandIds.TransportLineAddStopCommand;

        public override void HandleOnServer(TransportLineAddStopCommand command, Player player) => Handle(command);

        public override void HandleOnClient(TransportLineAddStopCommand command) => Handle(command);

        private void Handle(TransportLineAddStopCommand command)
        {
            TransportHandler.IgnoreAddStops.Add(command.LineID);
            TransportManager.instance.m_lines.m_buffer[command.LineID].AddStop(command.LineID, command.index, command.Position, command.fixedPlatform);
            TransportHandler.IgnoreAddStops.Remove(command.LineID);

        }

    }
}
