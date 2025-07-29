using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class csUi : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public string SceneName;

    public void SceneChanger()
    {

        SceneManager.LoadScene(SceneName);


    }

}
