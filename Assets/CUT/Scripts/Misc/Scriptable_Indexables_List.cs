using System.Collections.Generic;
using UnityEngine;

namespace DartsGames
{
    public class Scriptable_Indexables_List<T> : Scriptable_GUI_Button where T : I_Indexable
    {
        [SerializeField]
        protected List<T> listOfItems;

        public sealed override string ButtonName() => "Set IDs in order";

        public sealed override void OnGUIButtonPressed()
        {
            for (int i = 0; i < listOfItems.Count; i++)
                listOfItems[i].ID = i;
        }
    }
}