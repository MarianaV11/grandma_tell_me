using UnityEngine;

public class CharacterMove
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float moveSpeed;

    public CharacterMove(GameObject personagem, float velocidade)
    {
        rb = personagem.GetComponent<Rigidbody2D>();
        animator = personagem.GetComponent<Animator>();
        spriteRenderer = personagem.GetComponent<SpriteRenderer>();
        moveSpeed = velocidade;
    }

    public bool MoverPara(Vector2 destino, float deltaTime)
    {
        Vector2 pos = rb.position;
        Vector2 direcao = (destino - pos).normalized;
        Vector2 movimento = direcao * (moveSpeed * deltaTime);

        if (Vector2.Distance(pos, destino) > 0.1f)
        {
            rb.MovePosition(pos + movimento);
            AtualizarAnimacao(direcao);
            return false; // Ainda não chegou
        }

        AtualizarAnimacao(Vector2.zero); // Idle
        return true; // Chegou
    }

    private void AtualizarAnimacao(Vector2 direcao)
    {
        if (direcao != Vector2.zero)
        {
            if (Mathf.Abs(direcao.x) > Mathf.Abs(direcao.y))
                animator.Play("walking_side");
            else if (direcao.y > 0)
                animator.Play("walking_up");
            else
                animator.Play("walking_down");

            if (direcao.x != 0)
                spriteRenderer.flipX = direcao.x < 0;
        }
        else
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            if (state.IsName("walking_up"))
                animator.Play("idle_up");
            else if (state.IsName("walking_down"))
                animator.Play("idle_down");
            else
                animator.Play("idle_side");
        }
    }
}