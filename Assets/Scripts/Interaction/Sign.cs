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
      fadeCoroutine = StartCoroutine(FadeTextTo(1.0f));
   }

   protected override void OnExitRange()
   {
      if (fadeCoroutine != null)
         StopCoroutine(fadeCoroutine);
      fadeCoroutine = StartCoroutine(FadeOutText());
   }

   private IEnumerator FadeOutText()
   {
      yield return FadeTextTo(0.0f);
      floatingText.enabled = false;
      fadeCoroutine = null;
   }

   private IEnumerator FadeTextTo(float targetAlpha)
   {
      IEnumerator fadeEnumerator = FadeUtil.FadeTo(floatingText.color, targetAlpha, fadeDuration);
      while (fadeEnumerator.MoveNext())
      {
         floatingText.color = (Color)fadeEnumerator.Current;
         yield return null;
      }
   }
}
