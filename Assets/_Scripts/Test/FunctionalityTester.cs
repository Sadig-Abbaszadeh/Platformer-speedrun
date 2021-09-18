using System;
using UnityEngine;
using DartsGames;

[ExtendEditor]
public class FunctionalityTester : MonoBehaviour
{
    [InspectorButton]
    private void Respawn()
    {
        // TODO respawn -1 check
        FindObjectOfType<CharController>().transform.position =
            CheckPoint.GetLastCheckpointPosition();

        CheckPoint.SetActivateCheckpointObjects();
    }
}