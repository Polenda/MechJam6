using TMPro;
using UnityEngine;

public class EndGameScript : MonoBehaviour
{
    public static EndGameScript Instance;
    public int myBool = 0;
    [SerializeField] private TextMeshProUGUI text;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (myBool == 0)
        {
            text.text = "No Previous Game";
        }
        else if (myBool == 1)
        {
            text.text = "You survived!";
        }
        else if (myBool == 2)
        {
            text.text = "You did not survive.";
        }
        else
        {
            text.text = "Unknown game state.";
        }
    }
}
