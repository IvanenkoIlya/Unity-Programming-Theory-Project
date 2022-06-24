using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
   public static SceneFadeManager Instance;

   [SerializeField] float fadeDuration = 0.5f;

   private Image fadeOverlay;

   void Start()
   {
      if(Instance != null)
      {
         Destroy(gameObject);
         return;
      }

      Instance = this;
      DontDestroyOnLoad(gameObject);

      fadeOverlay = GetComponentInChildren<Image>();
   }

   public IEnumerator FadeIn()
   {
      if (gameObject.activeSelf)
      {
         yield return new WaitForSeconds(0.5f);

         IEnumerator fadeEnumerator = FadeUtil.FadeTo(fadeOverlay.color, 0.0f, fadeDuration);
         while (fadeEnumerator.MoveNext())
         {
            fadeOverlay.color = (Color)fadeEnumerator.Current;
            yield return null;
         }
         gameObject.SetActive(false);
      }
   }

   public IEnumerator FadeOut()
   {
      if (!gameObject.activeSelf)
      {
         gameObject.SetActive(true);
         IEnumerator fadeEnumerator = FadeUtil.FadeTo(fadeOverlay.color, 1.0f, fadeDuration);
         while (fadeEnumerator.MoveNext())
         {
            fadeOverlay.color = (Color)fadeEnumerator.Current;
            yield return null;
         }
      }
   }
}
