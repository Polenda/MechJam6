using UnityEngine;
using UnityEngine.SceneManagement;

public class TownManagerScript : MonoBehaviour
{
    public GameObject[] sceneObjects;
    public TownObjectScript townObjectScript;


    void Update()
    {
        if (townObjectScript.clicked)
        {
            SceneManager.LoadScene(townObjectScript.sceneIndex);
            townObjectScript.clicked = false; 
            Debug.Log("Town object clicked at scene index: " + townObjectScript.sceneIndex);

        }
    }
}
