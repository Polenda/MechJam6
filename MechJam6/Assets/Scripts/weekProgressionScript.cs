using TMPro;
using UnityEngine;

public class weekProgressionScript : MonoBehaviour
{
    public TextMeshProUGUI weekText;
    public GlobalManagerScript globalManagerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weekText.text = "Week: " + globalManagerScript.week;
    }

    // Update is called once per frame
    public void refresh()
    {
        weekText.text = "Week: " + globalManagerScript.week;
    }
}