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
      Color newColor = startingColor;
      IEnumerator fadeEnum = FadeTo(startingColor.a, targetAlpha, duration);
      while (fadeEnum.MoveNext())
      {
         newColor.a = (float)fadeEnum.Current;
         yield return newColor;
      }
   }
}
