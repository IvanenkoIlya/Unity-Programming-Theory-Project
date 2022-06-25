using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitZone : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.CompareTag("Player"))
         StartCoroutine(TransitionToMenu());
   }

   private IEnumerator TransitionToMenu()
   {
      yield return SceneFadeManager.Instance.FadeOut();
      SceneManager.LoadScene(0);
   }
}
