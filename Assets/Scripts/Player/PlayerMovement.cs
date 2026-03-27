using Unity.Burst.CompilerServices;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
//using UnityEditor.XR;
//using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{


    public static PlayerMovement Instance { get; private set; }

    public enum MovementStates
    {
        idle,
        walking,
        flying
    }

    public event EventHandler<MovementStates> OnMovementStateChanged;
    public event EventHandler<bool> OnDirectionChanged;

    [SerializeField] private float speed = .1f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private ParticleSystem jetPackFireParticleSystem;
    [SerializeField] private ParticleSystem jetPackFireParticleSystem2;
    [SerializeField] private AudioSource jetPackAudioSource;

    private bool playerCanMove;
    private bool canUseJetPack = true;
    private float jetPackActivationTimer;

    private float circleRadius;
    private float circleCastDistance = 0.1f;

    private Vector2 lastMovement = Vector2.zero;
    private Vector3 playerDirection;
    private Player player;
    private MovementStates currentMovementState = MovementStates.idle;
   


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = GetComponent<Player>();
        circleRadius = GetComponent<CircleCollider2D>().radius;
        player.OnPlayerStateChanged += Player_OnPlayerStateChanged;

    }

    private void Player_OnPlayerStateChanged(object sender, Player.OnPlayerStateChangedArgs e)
    {
        if (e.playerState == Player.PlayerStates.inactive)
        {
            playerCanMove = false;
        }
        else
        {
            playerCanMove = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if ()
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

        HandleMovementStates(playerMovement);
        SetJetPackEffects(playerMovement);
        

        lastMovement = playerMovement;
    }

    private void HandleMovementStates(Vector2 playerMovement)
    {

        if (OnGround())
        {
            
            if (Mathf.Abs(playerMovement.x) > 0)
            {
                if (currentMovementState != MovementStates.walking)
                {
                    currentMovementState = MovementStates.walking;
                    OnMovementStateChanged?.Invoke(this, currentMovementState);
                }
            }
            else if (currentMovementState != MovementStates.idle)
            {
                currentMovementState = MovementStates.idle;
                OnMovementStateChanged?.Invoke(this, currentMovementState);
            }
        }
        if (lastMovement.x >= 0 && playerMovement.x < 0)
        {
            OnDirectionChanged?.Invoke(this, true);
        }
        else if (lastMovement.x <= 0 && playerMovement.x > 0)
        {
            OnDirectionChanged?.Invoke(this, false);

        }

    }

    
    private void SetJetPackEffects(Vector2 playerMovement)
    {
        if (playerMovement.y > 0 && Player.Instance.hasFuel())
        {
            CameraManager.Instance.SetShakeCamera(0.35f);
            jetPackFireParticleSystem.gameObject.SetActive(true);
            jetPackFireParticleSystem.Play();
            jetPackFireParticleSystem2.gameObject.SetActive(true);
            jetPackFireParticleSystem2.Play();
            if (lastMovement.y == 0)
            {
                jetPackAudioSource.Play();

            }

        }
        else if (playerMovement.y == 0 && lastMovement.y > 0) {
            CameraManager.Instance.SetShakeCamera(0.0f);
            jetPackFireParticleSystem.gameObject.SetActive(false);
            jetPackFireParticleSystem.Pause();
            jetPackFireParticleSystem2.gameObject.SetActive(false);
            jetPackFireParticleSystem2.Pause();
            if (lastMovement.y > 0)
            {
                jetPackAudioSource.Pause();
            }
        }

    }

    private void SpaceMovement(Vector2 playerMovement)
    {

        Player.Instance.UseJetPackFuel(Time.deltaTime*1f);
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
            Player.Instance.ActivateJetPack();

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

        // if the player is not on the ground of the planet, apply gravity to their movement
        if (!OnGround()) { }
        {
            transform.position += toPlanetDirection * currentPlanet.GetGravityScalar() * Time.deltaTime;
        }
        

        if (playerMovement.y > 0 && canUseJetPack && Player.Instance.hasFuel())
        {
            transform.position += toPlayerDirection * jumpForce * Time.deltaTime;
            Player.Instance.ActivateJetPack();
        }


        // if the player moves it will rotate them around the planet
        transform.position = player.GetCurrentPlanet().GetPlanetPosition(-playerMovement.x, transform.position, circleRadius, speed);

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
