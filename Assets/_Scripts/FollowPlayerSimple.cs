using System;
using UnityEngine;

public class FollowPlayerSimple : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private Vector3 pos;

    private void Update()
    {
        pos = transform.position;
        pos.x = target.position.x;

        transform.position = pos;
    }
}