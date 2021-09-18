using DartsGames.DialogueGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DartsGames.Editors.DialogueGraph
{
    public class DialogueNode : Node
    {
        public static readonly Vector2 defaultSize = new Vector2(150, 200);
        // node ID
        public string GUID;
        // is this the first node
        public bool isEntry = false;

        // data (?)
        public string text;

        public List<DialoguePort> inputPorts = new List<DialoguePort>(),
            outputPorts = new List<DialoguePort>();

        public static DialogueNode ReconstructFromSave(NodeSaveData saveData)
        {
            var node = new DialogueNode()
            {
                GUID = saveData.Guid,
                text = saveData.text,
                title = saveData.title,
                isEntry = saveData.isEntry,
            };

            node.SetPosition(saveData.rect);
            return node;
        }
    }
}