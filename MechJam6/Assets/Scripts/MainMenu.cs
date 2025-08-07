using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private fadeToBlackScript fadeScript;
    [SerializeField] private GameObject textCanvas;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private InputActionAsset inputActions;

    private InputAction clickAction;
    private bool readyToStart = false;

    void Awake()
    {
        clickAction = inputActions.FindAction("Click");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void gamePlay()
    {
        StartCoroutine(FadeAndShowText());
    }

    IEnumerator FadeAndShowText()
    {
        yield return StartCoroutine(fadeScript.FadeToBlack());
        textCanvas.SetActive(true);
        infoText.text = "It’s been 400 years since galactic society abandoned this solar system. The worsening solar flares have blasted apart the ozone and atmosphere of its dependent planets. The once-green fields of Elysium have been slowly dying, holes punched through the atmosphere see the ejection of gases and material from the surface of the planet. The remaining humans on this planet have had to rebuild themselves after waves of ecological disasters and technological apocalypses. Every day, it feels like the collective breaths of everyone on Elysium is being slowly sucked away…";
        readyToStart = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToStart && clickAction.triggered)
        {
            readyToStart = false;
            SceneManager.LoadScene(1);
        }
    }
}
