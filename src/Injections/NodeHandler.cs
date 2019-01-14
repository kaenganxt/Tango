using ColossalFramework;
using CSM.Commands;
using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSM.Injections
{
    public class NodeHandler
    {
        public static int IgnoreAll { get; set; } = 0;
    }

    [HarmonyPatch(typeof(NetManager))]
    [HarmonyPatch("CreateNode")]
    public class CreateNode
    {   
        /// <summary>
        /// This handler is executed after a new NetNode is created using NetManager::CreateNode
        /// </summary>
        /// <param name="__result">The boolean return value of CreateNode states if the node was created successfully</param>
        /// <param name="node">This is the node id set by CreateNode</param>
        public static void Postfix(bool __result, ref ushort node)
        {
            CSM.Log($"node created {node} {NodeHandler.IgnoreAll}");

            if (NodeHandler.IgnoreAll > 0)
            {
                return;
            }
            
            if (__result)
            {
                NetNode netNode = Singleton<NetManager>.instance.m_nodes.m_buffer[node];
                Command.SendToAll(new NodeCreateCommand
                {
                    Position = netNode.m_position,
                    InfoIndex = netNode.m_infoIndex,
                    NodeId = node
                });
            }
        }
    }

    [HarmonyPatch(typeof(NetManager))]
    public class ReleaseNodeImpl
    {
        /// <summary>
        /// This handler is executed before a NetNode is released using NetManager::ReleaseNodeImplementation.
        /// </summary>
        /// <param name="node">The node id</param>
        /// <param name="data">The NetNode object</param>
        public static void Prefix(ushort node, ref NetNode data)
        {
            CSM.Log($"node released {node} {NodeHandler.IgnoreAll}");

            if (NodeHandler.IgnoreAll == 0 && data.m_flags != 0)
            {
                Command.SendToAll(new NodeReleaseCommand
                {
                    NodeId = node
                });
            }

            // Don't send released segments, as the other clients will also do it
            NodeHandler.IgnoreAll++;
        }

        public static void Postfix()
        {
            NodeHandler.IgnoreAll--;
        }

        // Get target method NetManager::ReleaseNodeImplementation(ushort, ref NetNode)
        public static MethodBase TargetMethod()
        {
            return typeof(NetManager).GetMethod("ReleaseNodeImplementation", AccessTools.all, null, new Type[] { typeof(ushort), typeof(NetNode).MakeByRefType() }, new ParameterModifier[] { });
        }
    }

    [HarmonyPatch(typeof(NetManager))]
    [HarmonyPatch("UpdateNodeFlags")]
    public class UpdateNodeFlags
    {
        /// <summary>
        /// This handler is executed after a node was refreshed in any way.
        /// </summary>
        /// <param name="node">The node id</param>
        public static void Postfix(ushort node)
        {
            //CSM.Log($"Update node {node} {NodeHandler.IgnoreAll} {Singleton<NetManager>.instance.m_nodes.m_buffer[node].m_flags}");

            if (NodeHandler.IgnoreAll > 0)
            {
                return;
            }

            NetNode netNode = Singleton<NetManager>.instance.m_nodes.m_buffer[node];

            if (netNode.m_flags == 0)
            {
                return;
            }

            ushort[] segments = new ushort[8];
            segments[0] = netNode.m_segment0;
            segments[1] = netNode.m_segment1;
            segments[2] = netNode.m_segment2;
            segments[3] = netNode.m_segment3;
            segments[4] = netNode.m_segment4;
            segments[5] = netNode.m_segment5;
            segments[6] = netNode.m_segment6;
            segments[7] = netNode.m_segment7;
            Command.SendToAll(new NodeUpdateCommand
            {
                Segments = segments,
                NodeID = node,
                Flags = netNode.m_flags
            });
        }
    }

    [HarmonyPatch(typeof(NetManager))]
    [HarmonyPatch("CreateSegment")]
    public class CreateSegment
    {
        /// <summary>
        /// This handler is executed after a segment was created using NetManager::CreateSegment
        /// </summary>
        /// <param name="__result">The boolean return value of CreateSegment states if the segment was created successfully</param>
        /// <param name="segment">The segment id</param>
        public static void Postfix(bool __result, ref ushort segment)
        {
            CSM.Log($"Segment Created {segment} {NodeHandler.IgnoreAll}");

            if (NodeHandler.IgnoreAll > 0)
            {
                return;
            }

            if (__result)
            {
                NetSegment seg = Singleton<NetManager>.instance.m_segments.m_buffer[segment];
                Command.SendToAll(new SegmentCreateCommand
                {
                    SegmentID = segment,
                    StartNode = seg.m_startNode,
                    EndNode = seg.m_endNode,
                    StartDirection = seg.m_startDirection,
                    EndDirection = seg.m_endDirection,
                    ModifiedIndex = seg.m_modifiedIndex,
                    InfoIndex = seg.m_infoIndex,
                    Invert = seg.m_flags.IsFlagSet(NetSegment.Flags.Invert)
                });
            }
        }
    }

    [HarmonyPatch(typeof(NetManager))]
    public class ReleaseSegmentImpl
    {
        /// <summary>
        /// This handler is executed before a segment is released using NetManager::ReleaseSegmentImplementation
        /// </summary>
        /// <param name="segment">The segment id</param>
        /// <param name="data">The NetSegment object</param>
        /// <param name="keepNodes">If adjacent nodes should also be released</param>
        public static void Prefix(ushort segment, ref NetSegment data, bool keepNodes)
        {
            CSM.Log($"Segment Released {segment} {NodeHandler.IgnoreAll}");

            if (NodeHandler.IgnoreAll > 0)
            {
                return;
            }

            NetManager net = NetManager.instance;

            if (data.m_flags != 0)
            {
                Command.SendToAll(new SegmentReleaseCommand
                {
                    SegmentId = segment,
                    KeepNodes = keepNodes
                });
            }
        }

        // Get target method NetManager::ReleaseSegmentImplementation(ushort, ref NetNode, bool)
        public static MethodBase TargetMethod()
        {
            return typeof(NetManager).GetMethod("ReleaseSegmentImplementation", AccessTools.all, null, new Type[] { typeof(ushort), typeof(NetSegment).MakeByRefType(), typeof(bool) }, new ParameterModifier[] { });
        }
    }
}
