using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Analytics;
public class fadeToBlackScript : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    public AudioSource audioSource;


    public IEnumerator FadeToBlack()
    {
        Debug.Log("Fading to black");
        float elapsed = 0f;
        Color color = Color.black;
        color.a = 0f;
        fadeImage.color = color;

        float startVolume = 0.2f;
        float endVolume = 0f;
        if (audioSource != null) audioSource.volume = startVolume;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            color.a = t;
            fadeImage.color = color;
            if (audioSource != null) audioSource.volume = Mathf.Lerp(startVolume, endVolume, t);
            yield return null;
        }
        color.a = 1f;
        fadeImage.color = color;
        if (audioSource != null) audioSource.volume = endVolume;
        yield return new WaitForSeconds(fadeDuration);
    }

    public IEnumerator FadeToClear()
    {
        Debug.Log("Fading to clear");
        float elapsed = 0f;
        Color color = fadeImage.color;
        fadeImage.color = color;

        float startVolume = 0f;
        float endVolume = 0.2f;
        if (audioSource != null) audioSource.volume = startVolume;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            color.a = 1f - t;
            fadeImage.color = color;
            if (audioSource != null) audioSource.volume = Mathf.Lerp(startVolume, endVolume, t);
            yield return null;
        }
        color.a = 0f;
        fadeImage.color = color;
        if (audioSource != null) audioSource.volume = endVolume;
    }
    
}
