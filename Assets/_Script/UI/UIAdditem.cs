using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Additem : MonoBehaviour
{
    public Image notifyText;
    public CanvasGroup canvasGroup;
    public float moveDistance = 100f; 
    public float duration = 1.5f;
    public float fadeDuration = 0.5f;

    RectTransform rectTransform;
    Vector2 startPos;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        canvasGroup= GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
     
    }

    public void ShowMessage(Sprite item)
    {
        StopAllCoroutines();
        StartCoroutine(Animate(item));
    }

    IEnumerator Animate(Sprite item)
    {
        notifyText.sprite = item;
        rectTransform.anchoredPosition = startPos;
        canvasGroup.alpha = 1f;

        float timer = 0f;
        while (timer < duration)
        {
            float t = timer / duration;
            rectTransform.anchoredPosition = startPos + Vector2.up * (moveDistance * t);
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        while (timer < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        rectTransform.anchoredPosition = startPos;
    }
}
