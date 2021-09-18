using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private GroundCheck groundCheck;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField, Tooltip("Transform that will be scaled -1 and 1 depending on run direction")]
    private Transform rotationTransform;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private float moveSpeed, jumpForce, doubleJumpForce;

    private Vector2 velocity = Vector2.zero;

    private Vector3 faceForwards = Vector3.one,
        faceBackwards = new Vector3(-1, 1, 1);

    private bool doubleJumped = false;

    private void Awake()
    {
        inputManager.OnSpace += OnSpace;
        groundCheck.OnGroundChange += GroundChanged;
    }

    private void GroundChanged(bool grounded)
    {
        if (grounded)
            doubleJumped = false;
    }

    private void Update()
    {
        var sign = 1;

        velocity.x = inputManager.HorizontalInput * moveSpeed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity;

        if (velocity.x > 0)
        {
            rotationTransform.localScale = faceForwards;
        }
        else if (velocity.x < 0)
        {
            rotationTransform.localScale = faceBackwards;
            sign = -1;
        }

        playerAnimator.SetFloat(AnimParameters.runSpeed, velocity.x * sign);
        playerAnimator.SetFloat(AnimParameters.airSpeed, velocity.y);
    }

    private void OnSpace()
    {
        if (groundCheck.IsGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else if(!doubleJumped)
        {
            doubleJumped = true;

            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
            playerAnimator.SetTrigger(AnimParameters.doubleJump);
        }
    }
}