using System.Collections;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class csUi : MonoBehaviour
{
    [SerializeField] private fadeToBlackScript fadeScript;

    [SerializeField] private int Sceneid;

    public TownManagerScript townManagerScript;
    [SerializeField] private Sprite map;
    [SerializeField] private Sprite homeMap;
    [SerializeField] private SpriteRenderer mapRenderer;
    public GlobalManagerScript globalManager;

    [SerializeField] private InputActionAsset InputActions;
    private InputAction clickAction;

    private bool pass = false;

    [SerializeField] private Button homeButtonObject;
    [SerializeField] private AudioClip travelClip;
    [SerializeField] private AudioClip homeClip;

    void Start()
    {
        InputActions = townManagerScript.InputActions;
        clickAction = InputActions.FindAction("Click");
        mapRenderer.sprite = homeMap;
        globalManager.Refresh();
        homeButtonObject.interactable = false;

    }

    void Update()
    {
        if (clickAction.triggered && pass)
        {
            pass = false;

            townManagerScript.textCanvas.SetActive(false);

            StartCoroutine(fadeScript.FadeToClear());
        }
    }

    public void SceneChanger()
    {
        Debug.Log("Scene change requested to scene ID: " + Sceneid);
        SceneManager.LoadScene(Sceneid);

    }

    public void travelButton()
    {
        if (townManagerScript.townObjectParent.activeSelf)
        {
            Debug.Log("Travel button clicked, but town object parent is already active.");
            return;
        }
        StartCoroutine(waitForFade());
    }
    IEnumerator waitForFade()
    {
        Debug.Log("Travel button clicked, starting fade to black");
        yield return StartCoroutine(fadeScript.FadeToBlack());
        fadeScript.audioSource.Stop();
        fadeScript.audioSource.clip = travelClip;
        fadeScript.audioSource.Play();

        townManagerScript.townObjectParent.SetActive(true);
        townManagerScript.townDialogueScript.NPCSpriteRenderer.enabled = false;
        townManagerScript.townDialogueScript.NPCDialoguePanel.SetActive(false);
        townManagerScript.townObjectParent.SetActive(true);
        mapRenderer.sprite = map;
        globalManager.Refresh();

        homeButtonObject.interactable = true;
        townManagerScript.textCanvas.SetActive(true);
        townManagerScript.infoText.text = "You head out to travel to a new town.";
        pass = true;

    }



    public void homeButton()
    {
        if (mapRenderer.sprite == homeMap)
        {
            Debug.Log("Home button clicked, but already on home map.");
            return;
        }
        StartCoroutine(thisForFade());
    }
    IEnumerator thisForFade()
    {
        Debug.Log("Home button clicked, starting fade to black");
        yield return StartCoroutine(fadeScript.FadeToBlack());
        fadeScript.audioSource.Stop();
        fadeScript.audioSource.clip = homeClip;
        fadeScript.audioSource.Play();

        townManagerScript.townDialogueScript.NPCSpriteRenderer.enabled = false;
        townManagerScript.townDialogueScript.NPCDialoguePanel.SetActive(false);
        townManagerScript.townObjectParent.SetActive(false);
        mapRenderer.sprite = homeMap;
        globalManager.maxWaterAmount -= 30;
        globalManager.waterAmount += globalManager.maxWaterAmount;
        globalManager.week += 1;

        globalManager.Refresh();

        townManagerScript.textCanvas.SetActive(true);
        townManagerScript.infoText.text = "You return home, less water is now available for you to trade.";
        pass = true;

        homeButtonObject.interactable = false;
    }
}
