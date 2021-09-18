using DartsGames.DialogueGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DartsGames.Editors.DialogueGraph
{
    public class DialogueGraphView : GraphView
    {
        private static readonly string DefaultStyle = "GridBackground {\n   --grid-background-color: #282828;\n --line-color: rgba(193, 196, 192, 0.1);\n   --thick-line-color: rgba(193, 196, 192, 0.1);\n --spacing: 25;\n}";

        // ctor
        public DialogueGraphView()
        {
            SetupView();

            CreateFirstNode();
        }

        // ctor
        public DialogueGraphView(Dialogue dialogue)
        {
            SetupView();

            var nodesDict = new Dictionary<string, DialogueNode>(dialogue.nodeData.Count);
            var portsDict = new Dictionary<string, DialoguePort>(dialogue.portData.Count);

            // do nodes
            foreach (var n in dialogue.nodeData)
            {
                var node = DialogueNode.ReconstructFromSave(n);
                base.AddElement(node);

                nodesDict.Add(node.GUID, node);
            }

            // do ports
            foreach (var p in dialogue.portData)
            {
                var dp = DialoguePort.ReconstructFromSave(p, nodesDict[p.containingNodeGuid]);

                portsDict.Add(dp.guid.ToString(), dp);
            }

            // do connections
            foreach (var p in dialogue.portData)
            {
                var outputPort = portsDict[p.portGuid];

                foreach (var g in p.connectedPortGuids)
                {
                    this.Add(
                        outputPort.port.ConnectTo(portsDict[g].port));
                }
            }
        }

        private void SetupView()
        {
            AddStyle();

            // allow zoom
            base.SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            // Add specific manipulators (tools) for drag and dropping, selection and stuff
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            // make grid background
            var bckg = new GridBackground();
            base.Insert(0, bckg);
            bckg.StretchToParentSize();
        }

        private void AddStyle()
        {
            // add grid 
            var style = Resources.Load<StyleSheet>("DialogueGraphStyle");

            if (style != null)
            {
                styleSheets.Add(style);
            }
            else
            {
                Debug.Log("here");
                var path = Application.dataPath + @"/Editor";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                path = path + @"/Resources";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                File.WriteAllLines(path + "/DialogueGraphStyle.uss", DefaultStyle.Split('\n'));

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                style = Resources.Load<StyleSheet>("DialogueGraphStyle");

                if (style != null)
                    styleSheets.Add(style);
            }
        }

        /// <summary>
        /// create the starting port and add it to the graph
        /// </summary>
        private void CreateFirstNode()
        {
            // make the first node
            var node = new DialogueNode()
            {
                title = "First Node",
                GUID = Guid.NewGuid().ToString(),
                isEntry = true,
                text = "Hi",
            };

            // add first node
            var firstPort = CreatePort(node, Direction.Output);
            firstPort.portName = "Start point";

            node.AddOutputPort(firstPort);

            // set in some place and add to graph
            node.SetPosition(new Rect(100, 200, 100, 150));

            base.AddElement(node);
        }

        public void CreateNodeAndShow(string nodeTitle)
        {
            AddElement(CreateNode(nodeTitle));
        }

        /// <summary>
        /// Generic node. In scripter, many different nodes must be created so yeah
        /// </summary>
        public DialogueNode CreateNode(string nodeTitle)
        {
            // just as above make a node
            var node = new DialogueNode()
            {
                title = nodeTitle,
                text = nodeTitle,
                GUID = Guid.NewGuid().ToString(),
            };

            // multi (?)
            var inPort = CreatePort(node, Direction.Input, Port.Capacity.Multi);
            inPort.portName = "Input";

            node.AddInputPort(inPort);

            node.SetPosition(new Rect(Vector2.zero, DialogueNode.defaultSize));

            var button = new Button(() => AddOutputPort(node));
            button.text = "Add choice";
            node.titleContainer.Add(button);

            return node;
        }

        /// <summary>
        /// Create a port for a node. Note: The port type (float) is used to check the allowed port type (?). Change this for params type for example in the visual scripter tool (?)
        /// </summary>
        private Port CreatePort(DialogueNode forNode, Direction portDirection, Port.Capacity portCapacity = Port.Capacity.Single) =>
            forNode.InstantiatePort(Orientation.Horizontal, portDirection, portCapacity, typeof(float));

        /// <summary>
        /// Adds an output port for a given node and renames the port
        /// </summary>
        private void AddOutputPort(DialogueNode forNode)
        {
            var port = CreatePort(forNode, Direction.Output);

            port.portName = "Choice " +
                forNode.outputContainer.Query("connector").ToList().Count;

            forNode.AddOutputPort(port);
        }

        /// <summary>
        /// Overridden to mandate port connection rules. TODO change this for port types and shit may be
        /// </summary>
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach(p =>
            {
                if (startPort != p && startPort.node != p.node)
                    compatiblePorts.Add(p);
            });

            return compatiblePorts;
        }
    }
}