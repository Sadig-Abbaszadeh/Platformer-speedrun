using DartsGames;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CheckPoint : InteractableObject
{
    private static int _lastCheck = -1,
        _lastArea = -1;

    public static int LatestCheckpoint
    {
        get => _lastCheck;
        protected set
        {
            if (value > _lastCheck)
                _lastCheck = value;
        }
    }

    public static int LatestAreaEntered
    {
        get => _lastArea;
        protected set
        {
            if (value > _lastArea)
                _lastArea = value;
        }
    }

    private static List<CheckPoint> All = new List<CheckPoint>();

    [HideInInspector, NonSerialized]
    protected List<ISplitObject> Objects = new List<ISplitObject>();

    protected int index;

    public static void SetCheckpointObjects(List<ISplitObject> splitObjects)
    {
        if (All.Count <= 0) return;

        splitObjects = splitObjects.OrderBy(s => s._Transform.position.x).ToList();

        if (splitObjects.Count <= 0) return;

        var index = 0;
        CheckPoint c;
        float left, right;

        (c, left, right) = GetCheckpointThresholds(index);

        for (int i = 0; i < splitObjects.Count; i++)
        {
            var pos = splitObjects[i]._Transform.position.x;

            if (pos >= left && pos < right)
                c.Objects.Add(splitObjects[i]);
            else if (pos >= right)
            {
                index++;
                (c, left, right) = GetCheckpointThresholds(index);

                if (c == null)
                    return;
            }
        }
    }

    private static (CheckPoint, float, float) GetCheckpointThresholds(int index)
    {
        CheckPoint c;
        float left, right;

        if (All.Count > index)
        {
            c = All[index];
            left = c.transform.position.x;

            if (All.Count > index + 1)
            {
                right = All[index + 1].transform.position.x;
            }
            else
            {
                right = float.MaxValue;
            }

            return (c, left, right);
        }
        else
            return (null, 0, 0);
    }

    public static Vector3 GetLastCheckpointPosition() => All[LatestCheckpoint].transform.position;

    public static void SetActivateCheckpointObjects()
    {
        for (int i = LatestCheckpoint; i <= LatestAreaEntered; i++)
            foreach (var o in All[i].Objects)
                o.SetState(true);
    }

    protected virtual void Awake()
    {
        index = All.Count;
        All.Add(this);
    }
}