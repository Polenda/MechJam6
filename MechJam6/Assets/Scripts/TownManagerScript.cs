using UnityEngine;
using UnityEngine.SceneManagement;

public class TownManagerScript : MonoBehaviour
{
    public TownObjectScript[] townObjectScript;


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
                obj.sceneUI.enabled = true;
                Debug.Log("Town object clicked at scene index: " + obj.sceneUI.name);

            }

        }
    }
}
