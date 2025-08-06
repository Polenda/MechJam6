using UnityEngine;
using TMPro;

public class GlobalManagerScript : MonoBehaviour
{
    [Header("Player Stats")]
    public int mechHealth = 4;
    public int waterAmount = 0;
    public int maxWaterAmount = 80;

    [Header("Player Bonds")]
    public int townDriftBond = 0;
    public int townFlameBond = 0;
    public int townSaltBond = 0;

    [Header("Player Story Progression")]
    public int week = 1;
    public int npc1Story = 0;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI weekText;
    [SerializeField] private TextMeshProUGUI mechHealthText;
    [SerializeField] private TextMeshProUGUI waterAmountText;
    void Start()
    {
        weekText.text = "Week: " + week;
        mechHealthText.text = "Health: " + mechHealth;
        waterAmountText.text = "Water: " + waterAmount + "/" + maxWaterAmount;
    }

    public void Refresh()
    {
        weekText.text = "Week: " + week;
        mechHealthText.text = "Health: " + mechHealth;
        waterAmountText.text = "Water: " + waterAmount + "/" + maxWaterAmount;
    }


}
