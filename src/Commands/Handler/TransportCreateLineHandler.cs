using ColossalFramework;
using CSM.Helpers;
using CSM.Networking;
using Harmony;
using System.Reflection;
using UnityEngine;

namespace CSM.Commands.Handler
{
    public class TransportCreateLineHandler : CommandHandler<TransportCreateLineCommand>
    {
        public override byte ID => CommandIds.TransportCreateLineCommand;

        public override void HandleOnServer(TransportCreateLineCommand command, Player player) => Handle(command);

        public override void HandleOnClient(TransportCreateLineCommand command) => Handle(command);

        private void Handle(TransportCreateLineCommand command)
        {
            UnityEngine.Debug.Log("new line recived");

            ushort[] _lineNumber = (ushort[])typeof(TransportManager).GetField("m_lineNumber", AccessTools.all).GetValue(TransportManager.instance);          

            TransportInfo info = PrefabCollection<TransportInfo>.GetPrefab(command.infoIndex);
            ushort lineID = command.lineID;
            TransportManager.instance.m_lines.m_buffer[lineID].m_flags = TransportLine.Flags.Created;
            TransportManager.instance.m_lines.m_buffer[lineID].Info = info;
            TransportManager.instance.m_lines.m_buffer[lineID].m_vehicles = 0;
            TransportManager.instance.m_lines.m_buffer[lineID].m_building = 0;
            TransportManager.instance.m_lines.m_buffer[lineID].m_budget = 100;
            TransportManager.instance.m_lines.m_buffer[lineID].m_totalLength = 0f;
            TransportManager.instance.m_lines.m_buffer[lineID].m_averageInterval = 0;
            TransportManager.instance.m_lines.m_buffer[lineID].m_ticketPrice = (ushort)info.m_ticketPrice;
            TransportManager.instance.m_lines.m_buffer[lineID].m_bounds = new Bounds();
            TransportManager.instance.m_lines.m_buffer[lineID].m_color = new Color32();
            TransportManager.instance.m_lines.m_buffer[lineID].m_passengers = new TransportPassengerData();
            if (command.newNumber)
            {
                TransportManager.instance.m_lines.m_buffer[lineID].m_lineNumber = _lineNumber[(int)info.m_transportType] = (ushort)(_lineNumber[(int)info.m_transportType] + 1);
            }
            else
            {
                TransportManager.instance.m_lines.m_buffer[lineID].m_lineNumber = 0;
            }
            TransportManager.instance.m_lineCount = ((int)TransportManager.instance.m_lines.ItemCount()) - 1;
            switch (info.m_vehicleType)
            {
                case VehicleInfo.VehicleType.Monorail:
                    TransportManager.instance.m_monorailLine.Disable();
                    break;

                case VehicleInfo.VehicleType.Ferry:
                    TransportManager.instance.m_ferryLine.Disable();
                    break;

                case VehicleInfo.VehicleType.Blimp:
                    TransportManager.instance.m_blimpLine.Disable();
                    break;
            }
            
        }
    }
}
