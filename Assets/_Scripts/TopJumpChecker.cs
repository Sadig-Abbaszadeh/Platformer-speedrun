using System;
using UnityEngine;

public class TopJumpChecker : InteractableObject
{
    [SerializeField]
    // TODO serialize interface instead
    private CharController charController;

    protected override void OnInteract(Collider2D collision)
    {
        Debug.Log("jumpedontop");
        base.OnInteract(collision);

        if (collision.TryGetComponent<ITopJumpable>(out var topJumpable))
        {
            topJumpable.OnCharacterJumpedOnThis(charController);
        }
    }
}