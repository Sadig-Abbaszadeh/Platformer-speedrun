using System;
using UnityEngine;

public class LevelCheckpoint : CheckPoint
{
    [SerializeField]
    private Animator poleAnimator;

    protected bool checkpointEntered = false, areaEntered = false;

    public void SetAreaEntered()
    {
        if (areaEntered) return;

        areaEntered = true;

        LatestAreaEntered = index;
    }

    private void SetCheckpointEntered()
    {
        if (checkpointEntered) return;

        checkpointEntered = true;
        poleAnimator.Play(AnimParameters.checkpointActivationAnim);

        LatestCheckpoint = index;
    }

    protected override void OnInteract(Collider2D collision)
    {
        base.OnInteract(collision);

        SetCheckpointEntered();
    }
}