using System.Collections;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class csUi : MonoBehaviour
{
    [SerializeField] private fadeToBlackScript fadeScript;

    [SerializeField] private int Sceneid;

    [SerializeField] private TownManagerScript townManagerScript;
    [SerializeField] private Sprite map;
    [SerializeField] private Sprite homeMap;
    [SerializeField] private SpriteRenderer mapRenderer;
    [SerializeField] private GlobalManagerScript globalManager;

    public void SceneChanger()
    {
        Debug.Log("Scene change requested to scene ID: " + Sceneid);
        SceneManager.LoadScene(Sceneid);

    }

    public void travelButton()
    {
        StartCoroutine(waitForFade());
    }
    IEnumerator waitForFade()
    {
        Debug.Log("Travel button clicked, starting fade to black");
        yield return StartCoroutine(fadeScript.FadeToBlack());
        townManagerScript.townObjectParent.SetActive(true);
        townManagerScript.townDialogueScript.NPCSpriteRenderer.enabled = false;
        townManagerScript.townDialogueScript.NPCDialoguePanel.SetActive(false);
        townManagerScript.townObjectParent.SetActive(true);
        mapRenderer.sprite = map;
        globalManager.Refresh();
        StartCoroutine(fadeScript.FadeToClear());

    }

    public void homeButton()
    {
        StartCoroutine(thisForFade());
    }
    IEnumerator thisForFade()
    {
        Debug.Log("Home button clicked, starting fade to black");
        yield return StartCoroutine(fadeScript.FadeToBlack());
        townManagerScript.townDialogueScript.NPCSpriteRenderer.enabled = false;
        townManagerScript.townDialogueScript.NPCDialoguePanel.SetActive(false);
        townManagerScript.townObjectParent.SetActive(false);
        mapRenderer.sprite = homeMap;
        globalManager.maxWaterAmount -= 20;
        globalManager.waterAmount = globalManager.maxWaterAmount;
        
        globalManager.Refresh();
        StartCoroutine(fadeScript.FadeToClear());
        
    }
}
