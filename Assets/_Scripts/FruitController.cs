using System;
using UnityEngine;

public class FruitController : InteractableObject, ISplitObject
{
    [SerializeField]
    private FruitType fruitType;
    [SerializeField]
    private GameObject mainBody, pickupAnimBody;

    public FruitType FruitType => fruitType;

    #region split object
    public Transform _Transform => transform;

    public void SetState(bool active)
    {
        Debug.Log("me");
        mainBody.SetActive(active);
        pickupAnimBody.SetActive(!active);
    }
    #endregion

    private void Awake()
    {
        LevelManager.Instance.splitObjectsList.Add(this);
    }

    protected override void OnInteract(Collider2D collision)
    {
        base.OnInteract(collision);

        if (collision.TryGetComponent(out IFruitEater fruitEater))
        {
            fruitEater.EatFruit(this);

            SetState(false);
        }
    }
}