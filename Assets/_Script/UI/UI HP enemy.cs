using UnityEngine;
using UnityEngine.UI;

public class UIHPenemy : MonoBehaviour
{
    public Image fillImage;
    private void Start()
    {
        fillImage = GetComponent<Image>();
    }
    public void SetHealth(float healthPercent)
    {
        fillImage.fillAmount = healthPercent;
    }
}
