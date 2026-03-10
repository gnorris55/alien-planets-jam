using UnityEngine;
using DG.Tweening;

public class ItemVisualMovement : MonoBehaviour
{
    [SerializeField] float speed = 3;

    private Vector3 startingLocation;
    private Vector3 endingLocation;
    private Vector3 movementDirection;
    private bool reachedLocation = false;
    private SpriteRenderer spriteRenderer;

    public void SetUp(Vector3 startingLocation, Vector3 endingLocation)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        this.startingLocation = startingLocation;
        this.endingLocation = endingLocation;

        transform.position = startingLocation;

        movementDirection = (endingLocation - startingLocation).normalized;
    }

    private void Update()
    {
        if (!reachedLocation)
        {
            transform.position += movementDirection * Time.deltaTime * speed;

            if (Vector3.Distance(transform.position, endingLocation) < 0.05)
            {
                float killTime = 0.25f;
                Destroy(gameObject, killTime);
                reachedLocation = true;
                spriteRenderer.DOFade(0f, killTime);
                spriteRenderer.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), killTime);
            }
        }
    }


}
