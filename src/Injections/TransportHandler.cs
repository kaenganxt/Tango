using ColossalFramework;
using CSM.Commands;
using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CSM.Injections
{
    public class TransportHandler
    {
        public static List<ushort> IgnoreLines { get; } = new List<ushort>();
        public static List<ushort> IgnoreAddStops { get; } = new List<ushort>();
        public static List<ushort> IgnoreRemoveStops { get; } = new List<ushort>();
        public static List<ushort> IgnoreMoveStops { get; } = new List<ushort>();

        public static bool IgnoreAll { get; set; } = false;
    }

    [HarmonyPatch(typeof(TransportTool))]
    [HarmonyPatch("EnsureTempLine")]
    public class EnsureTempLine
    {
        public static void Prefix()
        {
            TransportHandler.IgnoreAll = true;
        }

        public static void Postfix()
        {
            TransportHandler.IgnoreAll = false;
        }
    }

    [HarmonyPatch(typeof(TransportLine))]
    [HarmonyPatch("CreateNode")]
    public class CreateLineNode
    {
        public static void Prefix(TransportLine.Flags ___m_flags)
        {
            if (!TransportHandler.IgnoreAll && !___m_flags.IsFlagSet(TransportLine.Flags.Temporary))
            {
                NodeHandler.IgnoreAll++;
            }
        }

        public static void Postfix(TransportLine.Flags ___m_flags)
        {
            if (!TransportHandler.IgnoreAll && !___m_flags.IsFlagSet(TransportLine.Flags.Temporary))
            {
                NodeHandler.IgnoreAll--;
            }
        }
    }

    [HarmonyPatch(typeof(TransportLine))]
    [HarmonyPatch("CreateSegment")]
    public class CreateLineSegment
    {
        public static void Prefix(TransportLine.Flags ___m_flags)
        {
            if (!TransportHandler.IgnoreAll && !___m_flags.IsFlagSet(TransportLine.Flags.Temporary))
            {
                NodeHandler.IgnoreAll++;
            }
        }

        public static void Postfix(TransportLine.Flags ___m_flags)
        {
            if (!TransportHandler.IgnoreAll && !___m_flags.IsFlagSet(TransportLine.Flags.Temporary))
            {
                NodeHandler.IgnoreAll--;
            }
        }
    }

    [HarmonyPatch(typeof(TransportLine))]
    [HarmonyPatch("ReleaseNode")]
    public class ReleaseLineNode
    {
        public static void Prefix(TransportLine.Flags ___m_flags)
        {
            if (!TransportHandler.IgnoreAll && !___m_flags.IsFlagSet(TransportLine.Flags.Temporary))
            {
                NodeHandler.IgnoreAll++;
            }
        }

        public static void Postfix(TransportLine.Flags ___m_flags)
        {
            if (!TransportHandler.IgnoreAll && !___m_flags.IsFlagSet(TransportLine.Flags.Temporary))
            {
                NodeHandler.IgnoreAll--;
            }
        }
    }

    [HarmonyPatch(typeof(TransportManager))]
    [HarmonyPatch("CreateLine")]
    public class CreateLine
    {
        public static void Postfix(bool __result, ref ushort lineID, bool newNumber)
        {
            if (TransportHandler.IgnoreAll)
            {
                return;
            }

            CSM.Log($"line was created {lineID}");

            if (__result)
            {
                Command.SendToAll(new TransportCreateLineCommand
                {
                    lineID = lineID,
                    newNumber = newNumber,
                    infoIndex = TransportManager.instance.m_lines.m_buffer[lineID].m_infoIndex,
                });
            }
        }
    }

    [HarmonyPatch(typeof(TransportManager))]
    [HarmonyPatch("ReleaseLineImplementation")]
    public class ReleaseLineImplementation
    {
        public static void Prefix(ushort lineID, ref TransportLine data)
        {
            if (TransportHandler.IgnoreAll)
            {
                return;
            }

            if (data.m_flags != 0 && !TransportHandler.IgnoreLines.Contains(lineID))
            {
                CSM.Log($"line was released {lineID}");
                Command.SendToAll(new TransportReleaseLineCommand
                {
                    LineID = lineID,
                });
            }
        }
    }

    [HarmonyPatch(typeof(TransportLine))]
    [HarmonyPatch("AddStop")]
    public class AddStop
    {
        public static void Prefix(ushort lineID)
        {
            bool isTemp = TransportManager.instance.m_lines.m_buffer[lineID].m_flags.IsFlagSet(TransportLine.Flags.Temporary);
            if (!TransportHandler.IgnoreAll && !isTemp)
            {
                NodeHandler.IgnoreAll++;
            }
        }

        public static void Postfix(bool __result, ushort lineID, int index, Vector3 position, bool fixedPlatform)
        {
            bool isTemp = TransportManager.instance.m_lines.m_buffer[lineID].m_flags.IsFlagSet(TransportLine.Flags.Temporary);
            if (TransportHandler.IgnoreAll || isTemp)
            {
                return;
            }

            NodeHandler.IgnoreAll--;

            if (__result && !TransportHandler.IgnoreAddStops.Contains(lineID))
            {
                CSM.Log($"stop was added {lineID} {index} {TransportManager.instance.m_lines.m_buffer[lineID].m_flags}");
                Command.SendToAll(new TransportLineAddStopCommand
                {
                    fixedPlatform = fixedPlatform,
                    LineID = lineID,
                    index = index,
                    Position = position
                });
            }

        }
    }

    [HarmonyPatch(typeof(TransportLine))]
    public class RemoveStop
    {
        public static void Prefix(ushort lineID)
        {
            bool isTemp = TransportManager.instance.m_lines.m_buffer[lineID].m_flags.IsFlagSet(TransportLine.Flags.Temporary);
            if (!TransportHandler.IgnoreAll && !isTemp)
            {
                NodeHandler.IgnoreAll++;
            }
        }

        public static void Postfix(ushort lineID, int index, bool __result)
        {
            bool isTemp = TransportManager.instance.m_lines.m_buffer[lineID].m_flags.IsFlagSet(TransportLine.Flags.Temporary);
            if (TransportHandler.IgnoreAll || isTemp)
            {
                return;
            }

            NodeHandler.IgnoreAll--;
            if (!TransportHandler.IgnoreRemoveStops.Contains(lineID))
            {
                CSM.Log($"stop was removed {lineID} {index}");
                Command.SendToAll(new TransportLineRemoveStopCommand
                {
                    LineID = lineID,
                    Index = index,
                });

            }
        }
        public static MethodBase TargetMethod()
        {
            return typeof(TransportLine).GetMethod("RemoveStop", AccessTools.all, null, new Type[] { typeof(ushort), typeof(int), typeof(Vector3).MakeByRefType() }, new ParameterModifier[] { });
        }
    }

    [HarmonyPatch(typeof(TransportLine))]
    public class MoveStop
    {
        public static void Prefix(ushort lineID)
        {
            bool isTemp = TransportManager.instance.m_lines.m_buffer[lineID].m_flags.IsFlagSet(TransportLine.Flags.Temporary);
            // For temp lines we only sync nodes & segments, but not the stops
            // For normal lines, we need to ignore the nodes & segments, because they are created by the line stops
            if (!TransportHandler.IgnoreAll && !isTemp)
            {
                NodeHandler.IgnoreAll++;
            }
        }

        public static void Postfix(ushort lineID, int index, Vector3 newPos, bool fixedPlatform, bool __result)
        {
            bool isTemp = TransportManager.instance.m_lines.m_buffer[lineID].m_flags.IsFlagSet(TransportLine.Flags.Temporary);
            if (TransportHandler.IgnoreAll || isTemp)
            {
                return;
            }

            NodeHandler.IgnoreAll--;

            if (__result && !TransportHandler.IgnoreMoveStops.Contains(lineID))
            {
                CSM.Log($"move stop {lineID} {index} {TransportManager.instance.m_lines.m_buffer[lineID].m_flags.IsFlagSet(TransportLine.Flags.Temporary)}");
                Command.SendToAll(new TransportLineMoveStopCommand
                {
                    LineID = lineID,
                    index = index,
                    newPosition = newPos,
                    fixedPlatform = fixedPlatform
                });
            }
        }

        public static MethodBase TargetMethod()
        {
            return typeof(TransportLine).GetMethod("MoveStop", AccessTools.all, null, new Type[] { typeof(ushort), typeof(int), typeof(Vector3), typeof(bool), typeof(Vector3).MakeByRefType() }, new ParameterModifier[] { });
        }
    }
}
