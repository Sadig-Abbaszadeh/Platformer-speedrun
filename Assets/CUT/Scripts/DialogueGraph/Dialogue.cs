using System;
using System.Collections.Generic;
using UnityEngine;

namespace DartsGames.DialogueGraph
{
    public class Dialogue : ScriptableObject
    {
        public List<NodeSaveData> nodeData;
        public List<PortSaveData> portData;
    }
}