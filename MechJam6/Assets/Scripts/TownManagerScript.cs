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

    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("Location Settings")]
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private GameObject textCanvas;
    private int randomLocation = 1;

    // locations:
    // 1 = home
    // 2 = town - salt
    // 3 = town - flame
    // 4 = town - drift
    // 5 = encounter

    void Start()
    {
        textCanvas.SetActive(false);
        clickAction = InputActions.FindAction("Click");
    }

    void Update()
    {
        if (clickAction.triggered && pass)
        {   
            textCanvas.SetActive(false);
            pass = false;

            StartCoroutine(FadeToClear());
        }
    }

    public void townClicked()
    {
        foreach (var obj in townObjectScript)
        {
            if (obj.clicked && obj.sceneUI.enabled)
            {
                obj.clicked = false;
                obj.temp = false;
                obj.sceneUI.enabled = false;

            }
            else if (obj.clicked && !obj.sceneUI.enabled)
            {
                StartCoroutine(FadeToBlack());
            }

        }
        randomLocation = Random.Range(1, 6);
        Debug.Log("Random location selected: " + randomLocation);
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
            foreach (var obj in townObjectScript)
            {
                if (obj.clicked && !obj.sceneUI.enabled)
                {
                    obj.sceneUI.enabled = true;
                    Debug.Log("Town object clicked at scene index: " + obj.sceneUI.name);

                }
            }
        }
    }


    IEnumerator FadeToBlack()
    {
        Debug.Log("Fading to black");
        float elapsed = 0f;
        Color color = Color.black;
        color.a = 0f;
        fadeImage.color = color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1f;
        fadeImage.color = color;
        WaitForSeconds wait = new WaitForSeconds(fadeDuration);
        yield return wait;
        pass = true;
        textCanvas.SetActive(true);
        encounter();
    }
    IEnumerator FadeToClear()
    {
        Debug.Log("Fading to clear");
        float elapsed = 0f;
        Color color = fadeImage.color;
        fadeImage.color = color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(1f - (elapsed / fadeDuration));
            fadeImage.color = color;
            yield return null;
        }
        color.a = 0f;
        fadeImage.color = color;

    }
}
