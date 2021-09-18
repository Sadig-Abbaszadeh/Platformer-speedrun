using UnityEngine;

namespace DartsGames
{
    public interface IEditorState
    {
        void OnGUI();
        void Disable();
    }
}