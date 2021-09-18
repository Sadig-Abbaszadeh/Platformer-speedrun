using System;
using UnityEngine;

namespace DartsGames
{
    public class DontDestroyPermanent : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}