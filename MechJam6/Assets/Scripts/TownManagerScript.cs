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

    public fadeToBlackScript fadeScript;

    [Header("Location Settings")]
    public SpriteRenderer currentScene;
    public TextMeshProUGUI infoText;
    public GameObject textCanvas;
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

    [SerializeField] private AudioClip encounterClip;
    [SerializeField] private AudioClip townClip;

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

            Debug.Log("Encounter NPC dialogue started for town: " + chosenTown);
            StartCoroutine(clearFade()); 
        }
    }

    public void updateTowns()
    {
        foreach (var obj in townObjectScript)
        {
            obj.temp = false;
        }
    }

    IEnumerator clearFade()
    {
        yield return StartCoroutine(fadeScript.FadeToClear());
        Debug.Log("Fade to clear completed.");
        townDialogueScript.doOnce = true;
        if (townDialogueScript.currentDialogueFileFileName == "encounterDialogue")
        {
            townDialogueScript.pass = true;
        }
        else
        {
            townDialogueScript.end = true;
        }
    }

    public void townClicked()
    {
        randomLocation = Random.Range(1, 5);
        Debug.Log("Random location selected: " + randomLocation);
        for (int i = 0; i < townObjectScript.Length; i++)
        {
            var obj = townObjectScript[i];
            if (obj.clicked && !obj.temp)
            {
                obj.clicked = false;
                obj.temp = true;

                townObjectParent.SetActive(false);
                chosenTown = i;
                StartCoroutine(foundTown());
                break;
            }
        }

    }

    public void encounter()
    {
        fadeScript.audioSource.Stop();
        fadeScript.audioSource.clip = townClip;
        fadeScript.audioSource.Play();
        textCanvas.SetActive(true);
        infoText.text = "You make it to the town.";
        pass = true;
        townDialogueScript.NPCSpriteRenderer.enabled = true;
        Debug.Log("chosen town: " + chosenTown);
        if (chosenTown == 0)
        {
            return;
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

    IEnumerator foundTown()
    {
        yield return StartCoroutine(fadeScript.FadeToBlack());
        fadeScript.audioSource.Stop();
        fadeScript.audioSource.clip = encounterClip;
        fadeScript.audioSource.Play();
        pass = true;
        textCanvas.SetActive(true);
        infoText.text = "You have encountered something!";
        townDialogueScript.encounterNPCDialogue(chosenTown + 2);
    }
    
}
