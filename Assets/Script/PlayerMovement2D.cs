using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    public float speed = 1f;

    [Header("Animation")]
    public Animator animator;               // priskirk per Inspector (Hosi Animator)
    public SpriteRenderer spriteRenderer;   // priskirk per Inspector (Hosi SpriteRenderer)

    // Animator state pavadinimai (turi sutapti su Animator states!)
    private const string STATE_TOP = "top";
    private const string STATE_SIDE = "side";
    private const string STATE_BOTTOM = "bottom";

    private Rigidbody2D rb;

    private enum Facing { Top, Side, Bottom }
    private Facing lastFacing = Facing.Bottom;
    private bool lastFlipX = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Animacijas valdom Update (input), fizika bus FixedUpdate
        Vector2 move = ReadMoveInput();

        if (move != Vector2.zero)
        {
            // Pasirenkam kryptį (prioritetas: vertikali jei ji didesnė, kitaip horizontali)
            if (Mathf.Abs(move.y) >= Mathf.Abs(move.x))
            {
                if (move.y > 0) SetMoving(Facing.Top, false);
                else           SetMoving(Facing.Bottom, false);
            }
            else
            {
                bool flip = move.x < 0;           // A = flipX, D = neflip
                SetMoving(Facing.Side, flip);
            }
        }
        else
        {
            // Nieko nespaudi -> rodom PIRMA kadrą iš paskutinės krypties rinkinio
            SetIdleFirstFrame();
        }
    }

    void FixedUpdate()
    {
        Vector2 move = ReadMoveInput();
        if (move.sqrMagnitude > 1f) move = move.normalized;
        rb.linearVelocity = move * speed; // Unity 6: linearVelocity
    }

    private Vector2 ReadMoveInput()
    {
        Vector2 move = Vector2.zero;
        var k = Keyboard.current;
        if (k == null) return move;

        if (k.aKey.isPressed || k.leftArrowKey.isPressed)  move.x -= 1;
        if (k.dKey.isPressed || k.rightArrowKey.isPressed) move.x += 1;
        if (k.sKey.isPressed || k.downArrowKey.isPressed)  move.y -= 1;
        if (k.wKey.isPressed || k.upArrowKey.isPressed)    move.y += 1;

        return move;
    }

    private void SetMoving(Facing facing, bool flipX)
    {
        lastFacing = facing;
        lastFlipX = flipX;

        if (spriteRenderer != null) spriteRenderer.flipX = flipX;
        if (animator == null) return;

        animator.speed = 1f; // animacija sukasi

        string state = FacingToState(facing);
        // Jei jau tame state – netampyk kas frame
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(state))
            animator.Play(state, 0, 0f);
    }

    private void SetIdleFirstFrame()
    {
        if (spriteRenderer != null) spriteRenderer.flipX = lastFlipX;
        if (animator == null) return;

        animator.speed = 0f; // sustabdom animaciją
        string state = FacingToState(lastFacing);

        // priverstinai uždedam 0 laiką (pirmas kadras) ir laikom sustabdytą
        animator.Play(state, 0, 0f);
        animator.Update(0f);
    }

    private string FacingToState(Facing facing)
    {
        return facing switch
        {
            Facing.Top => STATE_TOP,
            Facing.Side => STATE_SIDE,
            _ => STATE_BOTTOM,
        };
    }
}
