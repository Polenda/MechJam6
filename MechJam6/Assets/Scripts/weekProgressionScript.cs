using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class weekProgressionScript : MonoBehaviour
{

    public GameObject textCanvas;
    public TextMeshProUGUI infoText;
    public Button ChoiceLeftButton;
    public Button ChoiceRightButton;
    public fadeToBlackScript fadeScript; 
    public csUi csUi;
    public ClosingScript closingScript;


    public void AskWeekQuestion()
    {
        if (csUi.globalManager.week == 3)
        {
            fadeScript.StartCoroutine(fadeScript.FadeToBlack());
            EndGameScript.Instance.myBool = 1;
            closingScript.ClosingGame();
        }

        textCanvas.SetActive(true);
        infoText.text = "Do you want to progress to the next week?";
        ChoiceLeftButton.interactable = true;
        ChoiceRightButton.interactable = true;

        ChoiceLeftButton.GetComponentInChildren<TextMeshProUGUI>().text = "Yes";
        ChoiceRightButton.GetComponentInChildren<TextMeshProUGUI>().text = "No";

        ChoiceLeftButton.onClick.RemoveAllListeners();
        ChoiceRightButton.onClick.RemoveAllListeners();

        ChoiceLeftButton.onClick.AddListener(() => ProgressWeek());
        ChoiceRightButton.onClick.AddListener(() => GoBackToMap());
    }

    private void ProgressWeek()
    {
        csUi.townManagerScript.updateTowns();
        ChoiceLeftButton.interactable = false;
        ChoiceRightButton.interactable = false;
        csUi.homeButton();

        textCanvas.SetActive(false);
    }

    private void GoBackToMap()
    {
        textCanvas.SetActive(false);
        csUi.travelButton(); // Calls the homeButton logic from your SceneChanger script
    }

    // Call AskWeekQuestion() when you want to prompt the player
}