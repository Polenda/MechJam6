using UnityEngine;
using UnityEngine.InputSystem;

public class TownObjectScript : MonoBehaviour
{
    public int sceneIndex;
    public bool clicked;
    public InputActionAsset InputActions;

    private InputAction clickAction;

    void Start()
    {
        clicked = false;
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
            }
        }
    }

    
}
