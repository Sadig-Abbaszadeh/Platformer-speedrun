using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField]
    private List<Level> levels;

    /// <summary>
    /// 1-based currently played level
    /// </summary>
    public int CurrentLevel { get; private set; }

    [HideInInspector, NonSerialized]
    public List<ISplitObject> splitObjectsList = new List<ISplitObject>();

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        // temporary
        CurrentLevel = 1;

        SetLevels();

        GetCheckpointObjects();
    }

    private void Start()
    {
        SetCheckpointObjects();
    }

    private void SetCheckpointObjects()
    {
        if (CheckPoint.All.Length <= 0) return;

        splitObjectsList = splitObjectsList.OrderBy(s => s._Transform.position.x).ToList();

        if (splitObjectsList.Count <= 0) return;

        var index = 0;
        CheckPoint c;
        float left, right;

        (c, left, right) = GetCheckpointThresholds(index);

        for(int i = 0; i < splitObjectsList.Count; i++)
        {
            var pos = splitObjectsList[i]._Transform.position.x;

            if (pos >= left && pos < right)
                c.Objects.Add(splitObjectsList[i]);
            else if(pos >= right)
            {
                index++;
                (c, left, right) = GetCheckpointThresholds(index);

                if (c == null)
                    return;
            }
        }
    }

    private (CheckPoint, float, float) GetCheckpointThresholds(int index)
    {
        CheckPoint c;
        float left, right;

        if (CheckPoint.All.Length > index)
        {
            c = CheckPoint.All[index];
            left = c.transform.position.x;

            if (CheckPoint.All.Length > index + 1)
            {
                right = CheckPoint.All[index + 1].transform.position.x;
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

    private void GetCheckpointObjects()
    {
        CheckPoint.All = FindObjectsOfType<CheckPoint>().OrderBy(c => c.transform.position.x).ToArray();

        for(int i = 0; i < CheckPoint.All.Length; i++)
        {
            CheckPoint.All[i].index = i;
        }
    }

    private void SetLevels()
    {
        for(int i = 0; i < levels.Count; i++)
        {
            var l = levels[i];

            l.levelNumber = i + 1;
            l.gameObject.SetActive(l.levelNumber == CurrentLevel);
        }
    }
}