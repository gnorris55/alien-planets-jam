using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private bool isOnFloor = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerMovement.Instance.OnPlayerMoved += PlayerMovement_OnPlayerMoved;

    }

    private void PlayerMovement_OnPlayerMoved(object sender, PlayerMovement.OnPlayerMovedArgs e)
    {
        Debug.Log("Player has moved");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerMovement = PlayerMovement.Instance.GetMovementVector();

        // if the player is moving
        if (Mathf.Abs(playerMovement.x) > 0 && isOnFloor)
        {
            // trigger running animation

        }


        if (playerMovement.x < 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }
        else if (playerMovement.x > 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
    }
}
