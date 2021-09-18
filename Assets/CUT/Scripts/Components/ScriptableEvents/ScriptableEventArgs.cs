using System;
using UnityEngine;

namespace DartsGames
{
    /// <summary>
    /// Read and write data from/to here before/after event invokation to pass data with scriptable events. Use event args classes or regular classes or structs.
    /// </summary>
    public static class ScriptableEventArgs
    {
        // example: when changing score set this field's value and then invoke, get the value in the invokation method.
        public static int Score;
    }
}