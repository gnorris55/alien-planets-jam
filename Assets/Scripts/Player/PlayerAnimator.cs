using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    [SerializeField] private SpriteRenderer playerArmSprite;
    [SerializeField] private Transform playerArmTransform;

    private SpriteRenderer spriteRenderer;
    private Animator playerAnimation;
    private PlayerMovement playerMovement;

    private bool isOnFloor = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = PlayerMovement.Instance;

        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimation = GetComponent<Animator>();

        playerMovement.OnMovementStateChanged += PlayerMovement_OnMovementStateChanged;
        playerMovement.OnDirectionChanged += PlayerMovement_OnDirectionChanged;
    }

    private void PlayerMovement_OnDirectionChanged(object sender, bool flipX)
    {
        spriteRenderer.flipX = flipX;
        /*
        if (flipX)
        {
            playerArmTransform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            playerArmTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
        */
        playerArmSprite.flipY = flipX;
    }

    private void PlayerMovement_OnMovementStateChanged(object sender, PlayerMovement.MovementStates movementState)
    {
        if (movementState == PlayerMovement.MovementStates.walking)
        {
            playerAnimation.SetBool("IsWalking", true);
        }
        else if (movementState == PlayerMovement.MovementStates.idle)
        {
            playerAnimation.SetBool("IsWalking", false);

        }
    }

    // Update is called once per frame
    void Update()
    {
        //bool playerIs = playerMovement.IsWalking();

    }
}
