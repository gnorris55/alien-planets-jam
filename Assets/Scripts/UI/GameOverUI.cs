using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button exitGameButton;
    [SerializeField] private Button restartGameButton;

    public void Show()
    {
        Image image = GetComponent<Image>();
        gameObject.SetActive(true);
        image.DOColor(new Color(0, 0, 0, 1), 1f);

    }

    private void Awake()
    {
        exitGameButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
        restartGameButton.onClick.AddListener(() => GameManager.Instance.RestartGame());
    }

}
