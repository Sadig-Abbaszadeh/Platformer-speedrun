using System;
using UnityEngine;

public class Checkpoint_AreaControl : InteractableObject
{
    [SerializeField]
    private LevelCheckpoint checkPoint;

    protected override void OnInteract(Collider2D collision)
    {
        base.OnInteract(collision);

        checkPoint.SetAreaEntered();
    }
}