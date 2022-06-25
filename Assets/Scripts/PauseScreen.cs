using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{

   private bool isExiting;

   // Start is called before the first frame update
   void Awake()
   {
      GameManager.OnPause += OnPaused;
      GameManager.OnResume += OnResume;
   }

   private void Start()
   {
      GetComponentInChildren<Image>().enabled = true;
      gameObject.SetActive(false);
      isExiting = false;
   }

   private void OnDestroy()
   {
      GameManager.OnPause -= OnPaused;
      GameManager.OnResume -= OnResume;
   }

   private void OnPaused()
   {
      if(!isExiting)
         gameObject.SetActive(true);
   }

   private void OnResume()
   {
      if(!isExiting)
         gameObject.SetActive(false);
   }

   public void UnpauseButton() 
   {
      GameManager.Instance.Unpause();
   }

   public void ExitGameButton()
   {
      isExiting = true;
      GameManager.Instance.Unpause();
      StartCoroutine(BackgroundMusicManager.Instance.FadeOutMusic());
      StartCoroutine(ReturnToMenu());
   }

   private IEnumerator ReturnToMenu()
   {
      yield return SceneFadeManager.Instance.FadeOut();
      SceneManager.LoadScene(0);
   }
}
