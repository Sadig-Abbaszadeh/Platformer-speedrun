using System.Collections.Generic;
using UnityEngine;

namespace DartsGames
{
    public class DebugCheats : MonoBehaviour
    {
        [SerializeField]
        private float buttonWidth, buttonHeight, buttonOffset, historyHeight, consoleHeight;
        [SerializeField]
        private int fontSize;

        private bool showConsole = false;

        private string input;

        #region Commands
        private DebugCommand KILL_ALL;
        private DebugCommand<int> DEBUG_TEST;
        #endregion

        private List<DebugCommandBase> commands;

        private void Awake()
        {
            KILL_ALL = new DebugCommand("kill_all", "kills current enemy", "kill_all", () =>
            {
                // command here
            });

            DEBUG_TEST = new DebugCommand<int>("debug_test", "just a test", "idk", a => /*command here*/{ });

            commands = new List<DebugCommandBase>()
        {
            KILL_ALL,
            DEBUG_TEST,

        };

        }

        private void Update()
        {
#if UNITY_EDITOR
            if (showConsole && Input.GetMouseButtonDown(1))
                HandleInput();
#endif
        }

        private void OnGUI()
        {
            float y = 0;

            GUI.skin.textField.fontSize = fontSize;
            GUI.skin.button.fontSize = fontSize;

            if (showConsole)
            {
                //GUI.Box(new Rect(0, 0, Screen.width, consoleHeight), "");
                input = GUI.TextField(new Rect(0, 0, Screen.width, consoleHeight), input);
                y = consoleHeight;

                if (GUI.Button(new Rect(Screen.width - 2 * (buttonWidth + buttonOffset), buttonOffset + y, buttonWidth, buttonHeight), "Ok"))
                    HandleInput();
            }

            if (GUI.Button(new Rect(Screen.width - buttonWidth - buttonOffset, buttonOffset + y, buttonWidth, buttonHeight), "X"))
                showConsole = !showConsole;
        }

        private void HandleInput()
        {
            string[] inputPieces = input.Split(' ');

            foreach (var c in commands)
            {
                // check command name
                if (inputPieces[0].Equals(c.CommandID))
                {
                    // check command type
                    if (c is DebugCommand dc)
                    {
                        dc.Invoke();
                    }
                    else if (c is DebugCommand<int> dcInt)
                    {
                        dcInt.Invoke(int.Parse(inputPieces[1]));
                    }
                }
            }

            input = string.Empty;
        }
    }
}