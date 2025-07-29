using System;
using UnityEngine;
using UnityEngine.UI;

public class SafeAreaScript : MonoBehaviour
{
    [SerializeField] RectTransform _CanvasRect;
    RectTransform rectTransform;
    public float sim;
    Vector2 size;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        float width = _CanvasRect.rect.width / Screen.width;
        float height = _CanvasRect.rect.height / Screen.height;

        float offsetTop = (Screen.safeArea.yMax - Screen.height) * height;
        float offsetBottom = Screen.safeArea.yMin * height;
        float offsetLeft = Screen.safeArea.xMin * width;
        float offsetRight = (Screen.safeArea.xMax - Screen.width) * width;

        rectTransform.offsetMax = new Vector2(offsetRight, offsetTop);
        rectTransform.offsetMin = new Vector2(offsetLeft, offsetBottom);

        CanvasScaler canvasScaler = _CanvasRect.GetComponent<CanvasScaler>();
        canvasScaler.referenceResolution = new Vector2(canvasScaler.referenceResolution.x, canvasScaler.referenceResolution.y +MathF.Abs(offsetTop) + MathF.Abs(offsetBottom));
    }
}
