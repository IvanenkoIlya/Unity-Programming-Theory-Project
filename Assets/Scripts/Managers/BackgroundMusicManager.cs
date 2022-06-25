using System.Collections;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
   public static BackgroundMusicManager Instance;

   [SerializeField] [Range(0.0f, 1.0f)] float maxVolume;
   [SerializeField] float fadeDuration;
   AudioSource bgm;


   private void Awake()
   {
      if(Instance != null)
      {
         Destroy(gameObject);
         return;
      }

      Instance = this;
      bgm = GetComponent<AudioSource>();
      DontDestroyOnLoad(gameObject);
   }

   public IEnumerator FadeInMusic()
   {
      yield return FadeMusic(0.0f, maxVolume);
   }

   public IEnumerator FadeOutMusic()
   {
      yield return FadeMusic(maxVolume, 0.0f);
   }

   private IEnumerator FadeMusic(float startVolume, float endVolume)
   {
      IEnumerator fadeEnumerator = FadeUtil.FadeTo(startVolume, endVolume, fadeDuration);
      while (fadeEnumerator.MoveNext())
      {
         bgm.volume = (float)fadeEnumerator.Current;
         yield return null;
      }
   }
}
