using System;
using System.Collections.Generic;
using UnityEngine;

namespace DartsGames.DialogueGraph
{
    [Serializable]
    public class NodeSaveData
    {
        public Rect rect;
        public string Guid, text, title;
        public bool isEntry;
    }

    [Serializable]
    public class PortSaveData
    {
        public string portName, portGuid, containingNodeGuid;

        /// <summary>
        /// Keep empty for input nodes
        /// </summary>
        public List<string> connectedPortGuids = new List<string>();

        public int portDirection, portCapacity;
        [SerializeField]
        private string _portType;

        public Type portType
        {
            get => Type.GetType(_portType);
            set => _portType = value.AssemblyQualifiedName;
        }
    }
}