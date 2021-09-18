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

        // ?
        SetLevels();
    }

    private void Start()
    {
        SetCheckpointObjects();
    }

    private void SetCheckpointObjects()
    {
        CheckPoint.SetCheckpointObjects(splitObjectsList);
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