using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;

public class TownManagerScript : MonoBehaviour
{
    public TownObjectScript[] townObjectScript;

    public InputActionAsset InputActions;
    private InputAction clickAction;
    private bool pass = false;

    [SerializeField] private fadeToBlackScript fadeScript;

    [Header("Location Settings")]
    public SpriteRenderer currentScene;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private GameObject textCanvas;
    public GameObject townObjectParent;
    private int randomLocation = 1;

    // locations:
    // 1 = home
    // 2 = town - salt
    // 3 = town - flame
    // 4 = town - drift
    // 5 = encounter

    void Start()
    {

        pass = false;
        textCanvas.SetActive(false);
        clickAction = InputActions.FindAction("Click");
    }

    void Update()
    {
        if (clickAction.triggered && pass)
        {
            textCanvas.SetActive(false);
            pass = false;

            StartCoroutine(fadeScript.FadeToClear());
        }
    }

    public void townClicked()
    {
        randomLocation = Random.Range(1, 6);
        Debug.Log("Random location selected: " + randomLocation);
        foreach (var obj in townObjectScript)
        {
            if (obj.clicked)
            {
                obj.clicked = false;
                obj.temp = false;

                townObjectParent.SetActive(false);
                StartCoroutine(foundTown());
            }

        }
        
    }

    private void encounter()
    {
        if (randomLocation == 5)
        {
            infoText.text = "You have encountered a wild creature!";
        }
        else
        {
            infoText.text = "You make it to the town.";
        }
    }

    IEnumerator foundTown()
    {
        yield return StartCoroutine(fadeScript.FadeToBlack());
        pass = true;
        textCanvas.SetActive(true);
        encounter();
    }
    
}
