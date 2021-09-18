using UnityEngine;

namespace DartsGames
{
    public class Scriptable_GUI_Button : ScriptableObject
    {
        public virtual string ButtonName() => "GUI Button";
        public virtual void OnGUIButtonPressed() { }
    }
}