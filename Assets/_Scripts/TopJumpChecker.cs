using System;
using UnityEngine;
using DartsGames;

public class TopJumpChecker : InteractableObject
{
    [SerializeField]
    private TopJumperWrap topJumperWrap;

    protected override void OnInteract(Collider2D collision)
    {
        Debug.Log("jumpedontop");
        base.OnInteract(collision);

        if (collision.TryGetComponent<ITopJumpable>(out var topJumpable))
        {
            topJumpable.OnCharacterJumpedOnThis(topJumperWrap.value);
        }
    }
}