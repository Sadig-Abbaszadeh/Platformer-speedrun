using System;
using UnityEngine;

public class StartPoint : CheckPoint
{
    protected override void Awake()
    {
        base.Awake();

        LatestCheckpoint = index;
        LatestAreaEntered = index;
    }
}