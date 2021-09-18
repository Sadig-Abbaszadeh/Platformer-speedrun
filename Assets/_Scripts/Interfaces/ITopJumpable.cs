using System;
using UnityEngine;

public interface ITopJumpable
{
    void OnCharacterJumpedOnThis(ITopJumper jumper);
}