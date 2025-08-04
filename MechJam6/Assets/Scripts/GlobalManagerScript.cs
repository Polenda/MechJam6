using UnityEngine;

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
    


}
