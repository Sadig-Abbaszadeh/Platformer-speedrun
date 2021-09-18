using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action OnSpace;

    public float HorizontalInput { get; private set; }

    private void Update()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
            OnSpace?.Invoke();
    }
}