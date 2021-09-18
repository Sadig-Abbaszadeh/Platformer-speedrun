using System;
using UnityEngine;

public static class AnimParameters
{
    public static readonly int runSpeed = Animator.StringToHash("runSpeed"),
        isAirborne = Animator.StringToHash("isAirborne"),
        airSpeed = Animator.StringToHash("airSpeed"),
        doubleJump = Animator.StringToHash("doubleJump"),
        checkpointActivationAnim = Animator.StringToHash("CP_pole_activation");
}