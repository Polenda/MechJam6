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
    // 0 = home
    // 1 = town - salt
    // 2 = town - flame
    // 3 = town - drift
    // 4 = encounter

    private int chosenTown;
    public TownDialogueScript townDialogueScript;

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
            townDialogueScript.NPCSpriteRenderer.enabled = true;
            townDialogueScript.NPCDialoguePanel.SetActive(true);

            StartCoroutine(fadeScript.FadeToClear());
        }
    }

    public void townClicked()
    {
        randomLocation = Random.Range(1, 5);
        Debug.Log("Random location selected: " + randomLocation);
        for (int i = 0; i < townObjectScript.Length; i++)
        {
            var obj = townObjectScript[i];
            if (obj.clicked)
            {
                obj.clicked = false;
                obj.temp = false;

                townObjectParent.SetActive(false);
                chosenTown = i;
                StartCoroutine(foundTown());
                break;
            }
        }
        
    }

    private void encounter()
    {
        if (randomLocation == 4)
        {
            infoText.text = "You have encountered a wild creature!";
            townDialogueScript.encounterNPCDialogue();
        }
        else
        {
            infoText.text = "You make it to the town.";
            townDialogueScript.NPCSpriteRenderer.enabled = true;
            if (chosenTown == 0)
            {
                townDialogueScript.homeNPCDialogue();
            }
            else if (chosenTown == 1)
            {
                townDialogueScript.saltNPCDialogue();
            }
            else if (chosenTown == 2)
            {
                townDialogueScript.flameNPCDialogue();
            }
            else if (chosenTown == 3)
            {
                townDialogueScript.driftNPCDialogue();
            }
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
