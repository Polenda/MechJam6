using System.Xml.Serialization;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ClosingScript : MonoBehaviour
{
    [SerializeField] private fadeToBlackScript fadeToBlackScript;
    [SerializeField] private GameObject textCanvas;
    [SerializeField] private TextMeshProUGUI infoText;

    public InputActionAsset InputActions;
    private InputAction clickAction;
    private bool isClosing = false;
    void Start()
    {
        clickAction = InputActions.FindAction("Click");
    }
    public void ClosingGame()
    {
        Debug.Log("Closing game");
        StartCoroutine(fadeToBlackScript.FadeToBlack());
        StartCoroutine(Closing());
    }

    IEnumerator Closing()
    {
        yield return new WaitForSeconds(1f);
        textCanvas.SetActive(true);
        infoText.text = "Every passing week, you accumulate more and more of the consequences of the choices you’ve had to make. The burden and the joy are yours. And so, the people of Elysium continue living, trying to make something of the situation they’re stuck in.";
        isClosing = true;
    }
    void Update()
    {
        if (isClosing && clickAction.triggered)
        {
            isClosing = false;
            SceneManager.LoadScene(0);
        }
    }
    

}
