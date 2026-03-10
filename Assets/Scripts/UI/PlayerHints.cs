using TMPro;
using UnityEngine;

public class PlayerHints : MonoBehaviour
{

    public static PlayerHints Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI hintText;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        hintText.gameObject.SetActive(false);
    }

    public void DisplayHint(string text)
    {
        hintText.gameObject.SetActive(true);
        hintText.text = text;
    }

    public void HideHint()
    {
        hintText.gameObject.SetActive(false);
    }
}
