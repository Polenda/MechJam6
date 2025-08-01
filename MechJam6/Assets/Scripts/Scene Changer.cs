using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class csUi : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int Sceneid;

    public void SceneChanger()
    {
        Debug.Log("Scene change requested to scene ID: " + Sceneid);
        SceneManager.LoadScene(Sceneid);


    }

}
