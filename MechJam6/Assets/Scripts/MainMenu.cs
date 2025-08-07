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
        infoText.text = "INtro Text";
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
