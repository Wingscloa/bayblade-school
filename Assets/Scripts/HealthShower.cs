using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthShower : MonoBehaviour
{
    public TMP_Text player1HealthText;
    public TMP_Text player2HealthText;

    public BeybladeCombat player1Combat;
    public BeybladeCombat player2Combat;

    void Update()
    {
        player1HealthText.text = "Player 1 HP: " + player1Combat.GetHealth().ToString();
        player2HealthText.text = "Player 2 HP: " + player2Combat.GetHealth().ToString();
    }
}
