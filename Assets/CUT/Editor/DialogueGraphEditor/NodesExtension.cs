using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System;
using UnityEngine.UIElements;

namespace DartsGames.Editors.DialogueGraph
{
    public static class NodesExtension
    {
        public static DialoguePort AddInputPort(this DialogueNode node, Port port, string portGuid = null)
        {
            node.inputContainer.Add(port);
            node.RefreshExpandedState();
            node.RefreshPorts();

            var dp = new DialoguePort(port, portGuid);
            node.inputPorts.Add(dp);

            return dp;
        }

        public static DialoguePort AddOutputPort(this DialogueNode node, Port port, string portGuid = null)
        {
            node.outputContainer.Add(port);
            node.RefreshExpandedState();
            node.RefreshPorts();

            var dp = new DialoguePort(port, portGuid);
            node.outputPorts.Add(dp);

            return dp;
        }

        public static void AddButtonSimple(this VisualElement element, string buttonName, Action onPressed) =>
            element.Add(new Button(onPressed) { text = buttonName });
    }
}