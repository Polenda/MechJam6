using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class homeBaseScript : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public Canvas canvas;

    [Header("top level actions")]
    public InputActionAsset InputActions;

    private InputAction clickAction;
    private bool isFading = false;

    void Start()
    {
        clickAction = InputActions.FindAction("Click");
    }
    void Update()
    {
        if (canvas.enabled && !isFading)
        {
            StartCoroutine(FadeToBlack());
        }
    }

    IEnumerator FadeToBlack()
    {
        isFading = true;
        float elapsed = 0f;
        Color color = new Color(0f, 0f, 0f, 0f);
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1f;
        fadeImage.color = color;
        WaitForSeconds wait = new WaitForSeconds(3);
        yield return wait;
        StartCoroutine(FadeToClear());
        
    }
    IEnumerator FadeToClear()
{
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
