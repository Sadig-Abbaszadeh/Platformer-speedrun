using DartsGames;
using System;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    protected LayerMask interactMask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (interactMask.ContainsLayer(collision.gameObject.layer))
            OnInteract(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interactMask.ContainsLayer(collision.gameObject.layer))
            OnStopInteract(collision);
    }

    protected virtual void OnStopInteract(Collider2D collision) { }

    protected virtual void OnInteract(Collider2D collision) { }
}