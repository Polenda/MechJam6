using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class homeBaseScript : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public Canvas canvas;


    void Update()
    {
        if (canvas.enabled)
        {
            StartCoroutine(FadeToBlack());
        }
    }

    IEnumerator FadeToBlack()
    {
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
    }
}
