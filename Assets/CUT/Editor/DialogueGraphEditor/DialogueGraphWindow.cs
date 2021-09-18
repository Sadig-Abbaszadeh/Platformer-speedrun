using DartsGames.DialogueGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DartsGames.Editors.DialogueGraph
{
    public class DialogueGraphWindow : EditorWindow
    {
        private DialogueGraphView graphView;
        private string fileName = "New Dialogue";

        public static Dialogue loadingDialogue = null;

        [MenuItem("DartsGames/Dialogue Window")]
        public static void ShowWindow()
        {
            // show actual window
            var w = CreateWindow<DialogueGraphWindow>("Dialogue editor");
        }

        private void OnEnable()
        {
            CreateGraphView();
            CreateToolBar();
        }

        /// <summary>
        /// Instantiate the graph view
        /// </summary>
        private void CreateGraphView()
        {
            // make new graph view object, or load from the SO
            if (loadingDialogue == null)
            {
                graphView = new DialogueGraphView()
                {
                    name = "Dialogue Graph"
                };
            }
            else
            {
                // load here
                graphView = new DialogueGraphView(loadingDialogue);

                // save file's name
                fileName = loadingDialogue.name;

                loadingDialogue = null;
            }

            // strecth to match window
            graphView.StretchToParentSize();
            // add visual element
            base.rootVisualElement.Add(graphView);
        }

        /// <summary>
        /// Make toolbar
        /// </summary>
        private void CreateToolBar()
        {
            var toolBar = new Toolbar();

            var fileNameField = new TextField("File Name: ");
            fileNameField.SetValueWithoutNotify(fileName);
            fileNameField.MarkDirtyRepaint();
            fileNameField.RegisterValueChangedCallback(n => fileName = n.newValue);

            toolBar.Add(fileNameField);
            toolBar.AddButtonSimple("Save", TrySaveGraph);
            toolBar.AddButtonSimple("Create Node", () => graphView.CreateNodeAndShow("Dialogue Node"));

            // VisualElement.Add is a golden function that adds other elements into this one.
            // here we add the toolbar to our window
            rootVisualElement.Add(toolBar);
        }

        private void TrySaveGraph()
        {
            DialogueSaver.SaveGraph(graphView, fileName);
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }
    }
}