using UnityEngine;
using UnityEngine.InputSystem;

public class TownObjectScript : MonoBehaviour
{
    public Canvas sceneUI;
    public bool clicked;
    public TownManagerScript townManagerScript;
    public InputActionAsset InputActions;

    private InputAction clickAction;

    public bool temp;

    void Start()
    {
        clicked = false;
        temp = false;
        sceneUI.enabled = false;
        clickAction = InputActions.FindAction("Click");
    }

    void Update()
    {
        if (clickAction.triggered)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                clicked = true;
                if (clicked && !temp) {
                    temp = true;
                    townManagerScript.townClicked();
                }
            }
        }
    }

    
}
