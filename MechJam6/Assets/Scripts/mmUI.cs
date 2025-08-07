using UnityEngine;
using UnityEngine.SceneManagement;

public class mmUI : MonoBehaviour

{
    [SerializeField] private int Sceneid;

    public void SceneChanger()
    {
        Debug.Log("Scene change requested to scene ID: " + Sceneid);
        SceneManager.LoadScene(Sceneid);

    }
}
