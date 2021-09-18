using System;
using UnityEngine;

/// <summary>
/// Used to identify and set state of objects belonging to separate checkpoints. Remember to register this in awake in LevelManager.SplitObjects
/// </summary>
public interface ISplitObject
{
    Transform _Transform { get; }

    void SetState(bool active);
}