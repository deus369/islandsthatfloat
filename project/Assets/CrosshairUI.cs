using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    public Image crosshairImage;
    private Color originalColor;

    void Awake()
    {
        if (crosshairImage == null)
        {
            return;
        }
        originalColor = crosshairImage.color;
    }

    public void ResetCrosshairColor()
    {
        crosshairImage.color = originalColor;
    }

    public void SetCrosshairColor(Color color)
    {
        if (crosshairImage == null)
        {
            return;
        }
        crosshairImage.color = color;
    }
}
