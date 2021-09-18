using DartsGames.DialogueGraph;
using System;
using UnityEditor.Experimental.GraphView;

namespace DartsGames.Editors.DialogueGraph
{
    public class DialoguePort
    {
        public Port port;
        public Guid guid;

        public DialoguePort(Port port, string portGuid)
        {
            this.port = port;

            this.guid = string.IsNullOrEmpty(portGuid) ? Guid.NewGuid() :
                new Guid(portGuid);
        }

        public static DialoguePort ReconstructFromSave(PortSaveData saveData, DialogueNode forNode)
        {
            var port = forNode.InstantiatePort(Orientation.Horizontal, (Direction)saveData.portDirection,
                (Port.Capacity)saveData.portCapacity, saveData.portType);

            port.portName = saveData.portName;

            if (port.direction == Direction.Input)
                return forNode.AddInputPort(port, saveData.portGuid);
            else
                return forNode.AddOutputPort(port, saveData.portGuid);
        }
    }
}