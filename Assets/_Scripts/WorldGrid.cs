using DartsGames;
using System;
using System.Collections.Generic;
using UnityEngine;

[ExtendEditor]
public class WorldGrid : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabObject;
    [SerializeField, Min(1)]
    private int width = 1, height = 1;
    [SerializeField]
    private Vector2 spacing;
    [SerializeField, HideInInspector]
    private List<GameObject> objs = new List<GameObject>();

    public List<GameObject> GridObjects => objs;
    public int Width => width;
    public int Height => height;

    [InspectorButton]
    public void CreateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                objs.Add(
                    Instantiate(prefabObject, transform.position + new Vector3(spacing.x * x, spacing.y * y), Quaternion.identity, transform));
            }
        }
    }

    [InspectorButton]
    public void DeleteGrid()
    {
        foreach (var g in objs)
            DestroyImmediate(g);

        objs.Clear();
    }
}