using System;
using DartsGames;
using UnityEngine;

public class GroundCheck : InteractableObject
{
    [SerializeField]
    private Animator playerAnimator;

    public bool IsGrounded { get; private set; } = false;

    public Action<bool> OnGroundChange;

    protected override void OnInteract(Collider2D collision)
    {
        base.OnInteract(collision);
        SetGrounded(collision, true);
    }

    protected override void OnStopInteract(Collider2D collision)
    {
        base.OnStopInteract(collision);
        SetGrounded(collision, false);
    }

    private void SetGrounded(Collider2D collision, bool enter)
    {
        IsGrounded = enter;

        playerAnimator.SetBool(AnimParameters.isAirborne, !enter);

        OnGroundChange?.Invoke(enter);
    }
}