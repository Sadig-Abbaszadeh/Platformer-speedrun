using System;
using UnityEngine;

public class Trap : InteractableObject
{
    protected override void OnInteract(Collider2D collision)
    {
        base.OnInteract(collision);

        if(collision.TryGetComponent(out ITrapHurtable trapHurtable))
        {
            trapHurtable.GetHurtByTrap();
        }
    }
}