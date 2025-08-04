using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Analytics;
public class fadeToBlackScript : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;


    public IEnumerator FadeToBlack()
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
    }

    public IEnumerator FadeToClear()
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
