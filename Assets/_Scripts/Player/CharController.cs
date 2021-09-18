using System;
using UnityEngine;

public class CharController : MonoBehaviour, IFruitEater, ITrapHurtable, ITopJumper
{
    public Action<CharController> OnCharacterDied;

    #region FruitEater
    public void EatFruit(FruitController fruit)
    {
        Debug.Log(fruit.name);
    }
    #endregion

    #region TrapHurtable
    public void GetHurtByTrap() => GetHurt();
    #endregion

    private void GetHurt()
    {
        // for now we die with one shot, no matter the damage source.
        Die();
    }

    private void Die()
    {
        Debug.Log("died");
        OnCharacterDied?.Invoke(this);
    }
}