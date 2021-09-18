using System;
using UnityEngine;
using DartsGames;

[ExtendEditor]
public class FunctionalityTester : MonoBehaviour
{
    [InspectorButton]
    private void Respawn()
    {
        FindObjectOfType<CharController>().transform.position =
            CheckPoint.All[CheckPoint.LatestCheckpoint].transform.position;

        for(int i = CheckPoint.LatestCheckpoint; i <= CheckPoint.LatestAreaEntered; i++)
        {
            foreach (var o in CheckPoint.All[i].Objects)
                o.SetState(true);
        }
    }
}