using TMPro;
using UnityEngine;

public class PlayerStateDisplay : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI playerStateText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player.Instance.OnPlayerStateChanged += Player_OnPlayerStateChanged;
    }

    private void Player_OnPlayerStateChanged(object sender, Player.OnPlayerStateChangedArgs e)
    {
        playerStateText.text = "Current State: " + e.playerState.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
