using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    [SerializeField] private Transform spawnLocationTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform GameOverScreen;
    [SerializeField] private Transform GameWonScreen;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RespawnPlayer();
        GameOverScreen.gameObject.SetActive(false);
        GameWonScreen.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // 2. If we are running as a standalone build
            Application.Quit();
        #endif
    }

    public void PlayerLostGame()
    {
        Player.Instance.SetState(Player.PlayerStates.inactive);
        GameOverScreen.GetComponent<GameOverUI>().Show();
    }

    public void PlayerWonGame()
    {
        Player.Instance.SetState(Player.PlayerStates.inactive);
        GameWonScreen.GetComponent<GameOverUI>().Show();
    }

    public void RespawnPlayer()
    {
        if (spawnLocationTransform != null)
        {
            playerTransform.position = spawnLocationTransform.position;
        }
    }
}
