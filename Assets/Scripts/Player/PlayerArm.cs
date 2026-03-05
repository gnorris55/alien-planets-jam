using UnityEngine;

public class PlayerArm : MonoBehaviour
{


    [SerializeField] private Transform testTriangle;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform bulletSpawnTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameInput.Instance.OnShootInputPressed += GameInput_OnShootInputPressed;
    }

    private void GameInput_OnShootInputPressed(object sender, System.EventArgs e)
    {

        Vector3 shootDirection = (bulletSpawnTransform.position - transform.position).normalized;
        Transform bulletTransform = Instantiate(bulletPrefab, bulletSpawnTransform.position, Quaternion.identity);
        bulletTransform.GetComponent<Bullet>().Setup(shootDirection);


        CameraManager.Instance.ShakeCamera(1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.right = GetMouseDirectionFromPlayer();
    }

    private Vector2 GetMouseDirectionFromPlayer()
    {
        // convert mouse position into world coordinates
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get direction you want to point at
        Vector2 direction = (mouseScreenPosition - (Vector2)transform.position).normalized;

        // set vector of transform directly
        return direction;
    }
}
