using Unity.Burst.CompilerServices;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.InputSystem;
//using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{


    public static PlayerMovement Instance { get; private set; }
    public event EventHandler<OnPlayerMovedArgs> OnPlayerMoved;

    public class OnPlayerMovedArgs : EventArgs
    {
        int type = 0;
    }

    [SerializeField] private float speed = .1f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private LayerMask groundLayer;
    
    private Planet currentPlanet;
    private float jetPackActivationTimer;
    private bool canUseJetPack = true;

    private float standardGravity = 3;
    private Rigidbody2D rigidBody;
    private bool canJump = false;
    private bool isMoving = false;

    private float circleRadius;
    private float circleCastDistance = 0.01f;
    private Vector2 lastMovement = Vector2.zero;
    private Vector3 playerDirection;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        circleRadius = GetComponent<CircleCollider2D>().radius;

    }


    
    // Update is called once per frame
    void Update()
    {

        Vector2 playerMovement = GameInput.Instance.GetMovement();

        if (currentPlanet != null)
        {
            PlanetMovement(playerMovement);
        }
        else
        {
            SpaceMovement(playerMovement);
        }


        if (jetPackActivationTimer > 0)
        {
            jetPackActivationTimer -= Time.deltaTime;
            if (jetPackActivationTimer <= 0)
            {
                canUseJetPack = true;
            }
        }

        SetCameraEffects(playerMovement);
        

        lastMovement = playerMovement;



    }

    
    private void SetCameraEffects(Vector2 playerMovement)
    {
        if (playerMovement.y > 0)
        {
            CameraManager.Instance.SetShakeCamera(0.35f);
        }
        else if (playerMovement.y == 0 && lastMovement.y > 0) {
            CameraManager.Instance.SetShakeCamera(0.0f);
        }

    }

    private void SpaceMovement(Vector2 playerMovement)
    {
        if(playerMovement.x > 0)
        {
            transform.Rotate(0, 0, -180f*Time.deltaTime);  
        }
        else if(playerMovement.x < 0)
        {
            transform.Rotate(0, 0, 180f*Time.deltaTime);  
        }

        if (playerMovement.y > 0)
        {
            playerDirection = transform.up;
        }
        transform.position += playerDirection * jumpForce * Time.deltaTime;
        // not on planet movement

    }

    private void PlanetMovement(Vector2 playerMovement)
    {


        Vector3 toPlanet = (currentPlanet.transform.position - transform.position);
        Vector3 toPlanetDirection = toPlanet.normalized;
        Vector3 toPlayerDirection = -toPlanetDirection;


        if (OnGround())
        {
            //Debug.Log("on ground");

        }
        // if the player is not on the ground of the planet, apply gravity to their movement
        else
        {
            transform.position += toPlanetDirection * currentPlanet.GetGravityScalar() * Time.deltaTime;
        }
        

        if (playerMovement.y > 0 && canUseJetPack)
        {
            transform.position += toPlayerDirection * jumpForce * Time.deltaTime;
        }

        float distanceFromPlanet = (transform.position - currentPlanet.transform.position).magnitude;


        // if the player moves it will rotate them around the planet
        transform.position = GetNewPlanetPosition(-playerMovement.x, toPlayerDirection, distanceFromPlanet) + currentPlanet.transform.position;

        // rotate the player
        float angle = Mathf.Atan2(toPlayerDirection.y, toPlanetDirection.x) * Mathf.Rad2Deg - 90;
        //transform.rotation = Quaternion.Euler(0, 0, -angle);
        transform.up = Vector3.Slerp(transform.up,  toPlayerDirection, Time.deltaTime * 5f);
    }


    private Vector3 GetNewPlanetPosition(float horizontalMovement, Vector2 currentDir, float distanceFromPlanet)
    {
        float angle = Mathf.Atan2(currentDir.y, currentDir.x);
        Debug.Log(angle);

        // This is the standard planet radius
        float radiusEffect = (4 * Mathf.PI) / (2 * Mathf.PI * currentPlanet.GetPlanetRadius());

        float rotationAmount = horizontalMovement * speed * radiusEffect* Time.deltaTime;


        angle += rotationAmount;

        float positionRadius = Mathf.Clamp(distanceFromPlanet, currentPlanet.GetComponent<CircleCollider2D>().radius, 100 + circleRadius);
        
        return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * positionRadius;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlanetAtmosphere")
        {
            PlanetAtmosphere planetAtmosphere = collision.gameObject.GetComponent<PlanetAtmosphere>();
            currentPlanet = planetAtmosphere.GetPlanet();

            Debug.Log("entering: " + currentPlanet.GetPlanetName());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlanetAtmosphere")
        {
            Debug.Log("exited: " + currentPlanet.GetPlanetName());
            currentPlanet = null;
            canUseJetPack = false;
            jetPackActivationTimer = .5f;
        }
    }
    private bool OnGround()
    {
        return Physics2D.CircleCast(transform.position, circleRadius, Vector2.down, circleCastDistance, groundLayer);

    }

    
    public Vector2 GetMovementVector()
    {
        return lastMovement;
    }
    
}
