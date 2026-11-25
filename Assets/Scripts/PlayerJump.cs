using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public SFXManager sfxManager;

    public float jumpForce = 10f;

    public Transform groundCheck;
    public float groundCheckDistance = 0.1f;
    public LayerMask floorMask;

    public ParticleSystem floorParticleSystem;

    Rigidbody2D rb;
    Animator animator;

    string state;

    public float yVelocityMin;

    bool inputStarted;

    void Awake(){
        state = "idle";
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update(){
        bool down = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
        bool held = Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space) || (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary));
        bool up = Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);

        if (down)
            inputStarted = true;

        if (up){
            TryJump();
            inputStarted = false;
        }

        if (held && !inputStarted)
            TryHold();

        inputStarted = false;

        if (rb.linearVelocity.y < 0f && state == "jumping"){
            state = "falling";

            ResetEveryTrigger();
            animator.SetTrigger("Land");
            return;
        }

        if (IsGrounded() && state == "falling"){
            state = "idle";

            floorParticleSystem.Play();

            //CHECK WANTHOLD
            if (held || down){
                Hold();
            }
            return;
        }

        
    }

    void ResetEveryTrigger(){
        animator.ResetTrigger("Jump");
        animator.ResetTrigger("Land");
        animator.ResetTrigger("Hold");
    }

    void TryHold(){ 
        if (state == "idle")
            Hold();
    }
    void Hold(){
        ResetEveryTrigger();
        animator.SetTrigger("Hold");
    }

    void TryJump(){
        if (state != "idle")
            return;

        state = "jumping";

        ResetEveryTrigger();
        animator.SetTrigger("Jump");
        Jump();
    }

    void Jump(){
        sfxManager.ApplySFX("jump");

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        floorParticleSystem.Stop();
    }

    public bool IsGrounded(){
        return Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            groundCheckDistance,
            floorMask
        );
    }

    public void ResetAnimation(){
        ResetEveryTrigger();
        animator.Play("Idle", 0, 0f);
        state = "idle";
    }
}