using System;
using UnityEngine;

namespace DartsGames
{
    public class WaitUntil : CustomYieldInstruction
    {
        private Func<bool> condition;

        public override bool keepWaiting => !condition();

        public WaitUntil(Func<bool> condition)
        {
            this.condition = condition;
        }
    }
}