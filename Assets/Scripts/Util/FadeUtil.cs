using System.Collections;
using UnityEngine;

public class FadeUtil
{
   public static IEnumerator FadeTo(float startingAlpha, float targetAlpha, float duration)
   {
      for(float i = 0.0f; i < 1.0f; i += Time.deltaTime / duration)
      {
         yield return Mathf.Lerp(startingAlpha, targetAlpha, i);
      }
   }

   public static IEnumerator FadeTo(Color startingColor, float targetAlpha, float duration)
   {
      float startingAlpha = startingColor.a;
      Color newColor = new Color(startingColor.r, startingColor.g, startingColor.b, startingAlpha);
      for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / duration)
      {
         newColor.a = Mathf.Lerp(startingAlpha, targetAlpha, i);
         yield return newColor;
      }
   }
}
