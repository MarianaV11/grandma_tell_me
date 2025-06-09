using UnityEngine;

public class GranddaughterController : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Vector2 _movement;
    
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Captura do input WASD
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
    
        // Define animação idle ou walk dependendo do movimento
        UpdateAnimation();
    }
    
    void FixedUpdate()
    {
        // Movimento do personagem
        Vector2 deltaMovement = _movement * (moveSpeed * Time.fixedDeltaTime);
        _rb.MovePosition(_rb.position + deltaMovement);
    }

    void UpdateAnimation()
    {
        if (_movement != Vector2.zero)
        {
            if (_movement.y > 0)
                _animator.Play("walking_up");
            else if (_movement.y < 0)
                _animator.Play("walking_down");
            else 
                _animator.Play("walking_side");
            
            // Espelha o sprite dependendo da direção horizontal
            if (_movement.x != 0)
                _spriteRenderer.flipX = _movement.x < 0;

        }
        else
        {
            // Idle com base na ultima direção
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            
            if (stateInfo.IsName("walking_up"))
                _animator.Play("idle_up");
            else if (stateInfo.IsName("walking_down"))
                _animator.Play("idle_down");
            else if (stateInfo.IsName("walking_side"))
                _animator.Play("idle_side");
        }
    }
}
