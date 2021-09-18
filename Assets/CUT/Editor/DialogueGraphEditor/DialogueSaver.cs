using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using DartsGames.DialogueGraph;

namespace DartsGames.Editors.DialogueGraph
{
    public static class DialogueSaver
    {
        public static void SaveGraph(DialogueGraphView graphView, string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrWhiteSpace(fileName) ||
                !IsValidFileName(fileName))
            {
                EditorUtility.DisplayDialog("Error", "Invalid file name", "ok");
                return;
            }

            var nodes = graphView.nodes.ToList().Cast<DialogueNode>();

            //var edges = graphView.edges.ToList();

            var portData = new List<PortSaveData>();
            var nodeData = new List<NodeSaveData>();

            foreach (var n in nodes)
            {
                SetSaveNodeData(n, nodeData, portData);
            }

            SaveGraphToSO(nodeData, portData, fileName);
        }

        private static bool IsValidFileName(string fileName)
        {
            var checkEx = new Regex("[" +
                Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]");

            if (checkEx.IsMatch(fileName)) return false;

            return true;
        }

        private static void SetSaveNodeData(DialogueNode node, List<NodeSaveData> nodeSaveData, List<PortSaveData> portSaveData)
        {
            foreach (var p in node.outputPorts)
            {
                var newPortData = GetPortDataSimple(p, node);

                foreach (var e in p.port.connections)
                {
                    newPortData.connectedPortGuids.Add(
                    (e.input.node as DialogueNode).inputPorts.
                    Find(dp => dp.port == e.input).guid.ToString());
                }

                portSaveData.Add(newPortData);
            }

            foreach (var p in node.inputPorts)
            {
                portSaveData.Add(GetPortDataSimple(p, node));
            }

            nodeSaveData.Add(new NodeSaveData()
            {
                rect = node.GetPosition(),
                Guid = node.GUID,
                text = node.text,
                title = node.title,
                isEntry = node.isEntry,
            });
        }

        private static PortSaveData GetPortDataSimple(DialoguePort p, DialogueNode node) => new PortSaveData()
        {
            portName = p.port.portName,
            portGuid = p.guid.ToString(),
            containingNodeGuid = node.GUID,


            portDirection = (int)p.port.direction,
            portCapacity = (int)p.port.capacity,
            portType = p.port.portType,
        };

        private static void SaveGraphToSO(List<NodeSaveData> nodeData, List<PortSaveData> portData, string fileName)
        {
            var d = Resources.Load<Dialogue>(fileName);

            // override prefab
            if (d != null)
            {
                if (EditorUtility.DisplayDialog("Attention", "Do you want to override the data in the asset " + fileName + "?", "Yes", "No"))
                {
                    d.nodeData = nodeData;
                    d.portData = portData;

                    AssetDatabase.SaveAssets();
                }
            }
            else
            {
                var folderPath = @"/Resources/DialogueData/";
                var resPath = Application.dataPath + folderPath;

                d = ScriptableObject.CreateInstance<Dialogue>();

                d.nodeData = nodeData;
                d.portData = portData;

                if (File.Exists(resPath + fileName + ".asset"))
                    fileName = fileName + UnityEngine.Random.Range(1000, 10000);

                d.name = fileName;

                if (!Directory.Exists(resPath))
                    Directory.CreateDirectory(resPath);

                AssetDatabase.CreateAsset(d, "Assets" + folderPath + fileName + ".asset");
                AssetDatabase.SaveAssets();
            }
        }
    }
}