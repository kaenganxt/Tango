using ColossalFramework;
using CSM.Injections;
using CSM.Networking;

namespace CSM.Commands.Handler
{
    public class TransportLineRemoveStopHandler : CommandHandler<TransportLineRemoveStopCommand>
    {
        public override byte ID => CommandIds.TransportLineRemoveStopCommand;

        public override void HandleOnServer(TransportLineRemoveStopCommand command, Player player) => Handle(command);

        public override void HandleOnClient(TransportLineRemoveStopCommand command) => Handle(command);

        private void Handle(TransportLineRemoveStopCommand command)
        {
            TransportHandler.IgnoreRemoveStops.Add(command.LineID);
            TransportManager.instance.m_lines.m_buffer[command.LineID].RemoveStop(command.LineID, command.Index);
            TransportHandler.IgnoreRemoveStops.Remove(command.LineID);
        }
    }
}
