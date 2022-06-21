using System.Collections;
using TMPro;
using UnityEngine;

public class Sign : Interact
{
   [SerializeField] string text;

   private TextMeshPro floatingText;
   private Coroutine fadeCoroutine;

   private void Start()
   {
      floatingText = GetComponentInChildren<TextMeshPro>();
      floatingText.color = new Color(floatingText.color.r, floatingText.color.g, floatingText.color.b, 0);
      floatingText.text = text;
      floatingText.enabled = false;
   }

   protected override void OnInteract()
   {
      StartCoroutine(HideInteractPrompt(0.2f));
      floatingText.enabled = true;
      fadeCoroutine = StartCoroutine(FadeTextTo(1.0f, fadeDuration, floatingText));
   }

   protected override void OnExitRange()
   {
      if (fadeCoroutine != null)
         StopCoroutine(fadeCoroutine);
      fadeCoroutine = StartCoroutine(FadeOutText());
   }

   private IEnumerator FadeOutText()
   {
      yield return FadeTextTo(0, fadeDuration, floatingText);
      floatingText.enabled = false;
      fadeCoroutine = null;
   }

   private IEnumerator FadeTextTo(float targetAlpha, float duration, TextMeshPro target)
   {
      Color startingColor = target.color;
      float startingAlpha = startingColor.a;
      for (float t = 0.0f; t < 1.0; t += Time.deltaTime / duration)
      {
         Color newColor = new Color(startingColor.r, startingColor.g, startingColor.b, Mathf.Lerp(startingAlpha, targetAlpha, t));
         target.color = newColor;
         yield return null;
      }
   }
}
