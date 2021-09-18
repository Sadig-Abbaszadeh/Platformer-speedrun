using DartsGames;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : InteractableObject
{
    // ??
    public static CheckPoint CurrentCheckpoint { get; private set; } = null;

    private static int _lastCheck = -1,
        _lastArea = -1;

    public static int LatestCheckpoint
    {
        get => _lastCheck;
        private set
        {
            if (value > _lastCheck)
                _lastCheck = value;
        }
    }

    public static int LatestAreaEntered
    {
        get => _lastArea;
        private set
        {
            if (value > _lastArea)
                _lastArea = value;
        }
    }

    public static CheckPoint[] All;

    [SerializeField]
    protected Animator poleAnimator;

    [HideInInspector, NonSerialized]
    public List<ISplitObject> Objects = new List<ISplitObject>();
    [HideInInspector, NonSerialized]
    public int index;

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