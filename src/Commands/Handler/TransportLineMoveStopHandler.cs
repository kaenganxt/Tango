using ColossalFramework;
using CSM.Injections;
using CSM.Networking;


namespace CSM.Commands.Handler
{
    class TransportLineMoveStopHandler : CommandHandler<TransportLineMoveStopCommand>
    {
        public override byte ID => CommandIds.TransportLineMoveStopCommand;

        public override void HandleOnServer(TransportLineMoveStopCommand command, Player player) => Handle(command);

        public override void HandleOnClient(TransportLineMoveStopCommand command) => Handle(command);

        private void Handle(TransportLineMoveStopCommand command)
        {
            TransportHandler.IgnoreMoveStops.Add(command.LineID);
            TransportManager.instance.m_lines.m_buffer[command.LineID].MoveStop(command.LineID, command.index, command.newPosition, command.fixedPlatform);
            TransportHandler.IgnoreMoveStops.Remove(command.LineID);
        }

    }
}
