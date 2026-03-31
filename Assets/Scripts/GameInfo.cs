using UnityEngine;
using UnityEngine.UI;

public class GameInfo : MonoBehaviour
{
    [SerializeField] private Button showGameInfoButton;
    [SerializeField] private Button hideGameInfoButton;
    [SerializeField] private GameObject gameInfoContents;


    private void Start()
    {
        showGameInfoButton.onClick.AddListener(ShowGameInfo);
        hideGameInfoButton.onClick.AddListener(HideGameInfo);

        ShowGameInfo();
    }


    private void ShowGameInfo()
    {
        showGameInfoButton.gameObject.SetActive(false);
        gameInfoContents.SetActive(true);
    }

    private void HideGameInfo()
    {
        gameInfoContents.SetActive(false);
        showGameInfoButton.gameObject.SetActive(true);
    }


}
