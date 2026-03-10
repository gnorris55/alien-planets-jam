using Unity.Burst.CompilerServices;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
//using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{


    public static PlayerMovement Instance { get; private set; }

    [SerializeField] private float speed = .1f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private ParticleSystem jetPackFireParticleSystem;
    
    private bool canUseJetPack = true;
    private float jetPackActivationTimer;

    private float circleRadius;
    private float circleCastDistance = 0.01f;

    private Vector2 lastMovement = Vector2.zero;
    private Vector3 playerDirection;
    private Player player;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = GetComponent<Player>();
        circleRadius = GetComponent<CircleCollider2D>().radius;

    }
    
    // Update is called once per frame
    void Update()
    {

        Vector2 playerMovement = GameInput.Instance.GetMovement();

        if (player.GetCurrentPlanet() != null)
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

        SetJetPackEffects(playerMovement);
        

        lastMovement = playerMovement;
    }

    
    private void SetJetPackEffects(Vector2 playerMovement)
    {
        if (playerMovement.y > 0 && Player.Instance.hasFuel())
        {
            CameraManager.Instance.SetShakeCamera(0.35f);
            jetPackFireParticleSystem.gameObject.SetActive(true);
            jetPackFireParticleSystem.Play();
        }
        else if (playerMovement.y == 0 && lastMovement.y > 0) {
            CameraManager.Instance.SetShakeCamera(0.0f);
            jetPackFireParticleSystem.gameObject.SetActive(false);
            jetPackFireParticleSystem.Pause();
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
            Player.Instance.UseFuel();

        }
        transform.position += playerDirection * jumpForce * Time.deltaTime;
        // not on planet movement

    }

    private void PlanetMovement(Vector2 playerMovement)
    {

        Planet currentPlanet = player.GetCurrentPlanet();

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
        

        if (playerMovement.y > 0 && canUseJetPack && Player.Instance.hasFuel())
        {
            transform.position += toPlayerDirection * jumpForce * Time.deltaTime;
            Player.Instance.UseFuel();
        }


        // if the player moves it will rotate them around the planet
        transform.position = player.GetCurrentPlanet().GetPlanetPosition(-playerMovement.x, toPlayerDirection, transform.position, circleRadius, speed);

        transform.up = Vector3.Slerp(transform.up,  toPlayerDirection, Time.deltaTime * 5f);
    }

    //Move to planet location

    private bool OnGround()
    {
        return Physics2D.CircleCast(transform.position, circleRadius, Vector2.down, circleCastDistance, groundLayer);

    }

    public void EnteredPlanet()
    {

    }

    public void ExitedPlanet()
    {
        canUseJetPack = false;
        jetPackActivationTimer = .5f;
    }
    
    public Vector2 GetMovementVector()
    {
        return lastMovement;
    }
}
