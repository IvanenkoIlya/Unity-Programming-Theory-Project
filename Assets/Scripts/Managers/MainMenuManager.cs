using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
   [SerializeField] List<GameObject> menuList;
   [SerializeField] GameObject blackScreen;
   [SerializeField] float fadeDuration;

   private GameObject activeMenu;
   private int menuSceneIndex;

   void Awake()
   {
      foreach (var menu in menuList)
      {
         var canvasGroup = menu.GetComponent<CanvasGroup>();
         canvasGroup.alpha = 0;
      }
   }

   private void Start()
   {
      SwitchToMenu(0);
      StartCoroutine(BackgroundMusicManager.Instance.FadeInMusic());
      StartCoroutine(SceneFadeManager.Instance.FadeIn());
   }

   public void SwitchToMenu(int menuIndex)
   {
      if (menuIndex < menuList.Count)
         StartCoroutine(SwitchToMenu(menuList[menuIndex]));
      else
         Debug.LogWarning($"Menu index {menuIndex} is out of bounds");
   }

   public void SwitchToMenu(string menuName)
   {
      var menu = menuList.FirstOrDefault(x => x.name == menuName);

      if (menu == null)
      {
         Debug.LogWarning($"No menu with name \"{menuName}\" found");
         return;
      }

      StartCoroutine(SwitchToMenu(menu));
   }

   IEnumerator SwitchToMenu(GameObject menu)
   {
      if (activeMenu != null)
      {
         yield return FadeTo(0.0f);
         activeMenu.SetActive(false);
      }
      activeMenu = menu;
      activeMenu.SetActive(true);
      yield return FadeTo(1.0f);
   }

   private IEnumerator FadeTo(float targetAlpha)
   {
      IEnumerator fadeEnumerator = FadeUtil.FadeTo(activeMenu.GetComponent<CanvasGroup>().alpha, targetAlpha, fadeDuration);
      while (fadeEnumerator.MoveNext())
      {
         activeMenu.GetComponent<CanvasGroup>().alpha = (float)fadeEnumerator.Current;
         yield return null;
      }
   }

   public void SwitchToMainMenu()
   {
      if (SceneManager.GetActiveScene().buildIndex != menuSceneIndex)
         SceneManager.LoadScene(menuSceneIndex);
      SwitchToMenu("Main");
   }

   public void SwitchToAboutMenu()
   {
      SwitchToMenu("About");
   }

   public void SwitchToControlsMenu()
   {
      SwitchToMenu("Controls");
   }

   public void StartGame()
   {
      StartCoroutine(BackgroundMusicManager.Instance.FadeOutMusic());
      StartCoroutine(StartGameCoroutine());
   }

   private IEnumerator StartGameCoroutine()
   {
      yield return SceneFadeManager.Instance.FadeOut();
      SceneManager.LoadScene(1);
   }

   public void ExitGame()
   {
      StartCoroutine(BackgroundMusicManager.Instance.FadeOutMusic());
      StartCoroutine(ExitGameCoroutine());
   }

   private IEnumerator ExitGameCoroutine()
   {
      yield return SceneFadeManager.Instance.FadeOut();
#if UNITY_EDITOR
      EditorApplication.ExitPlaymode();
#else
      Application.Quit();
#endif
   }
}
