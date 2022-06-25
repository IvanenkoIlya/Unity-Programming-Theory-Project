using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;

   public static event Action OnPause;
   public static event Action OnResume;

   [SerializeField] private bool _isPaused;

   public bool IsPaused 
   { 
      get => _isPaused; 
      private set => _isPaused = value;
   }

   void Start()
   {
      if(Instance != null)
      {
         Destroy(gameObject);
         return;
      }

      Instance = this;
      StartCoroutine(BackgroundMusicManager.Instance.FadeInMusic());
      StartCoroutine(SceneFadeManager.Instance.FadeIn());
   }

   public void TogglePause()
   {
      if (!IsPaused)
         Pause();
      else
         Unpause();
   }

   public void Pause()
   {
      Time.timeScale = 0.0f;
      IsPaused = true;
      OnPause?.Invoke();
   }

   public void Unpause()
   {
      Time.timeScale = 1.0f;
      IsPaused = false;
      OnResume?.Invoke();
   }
}
