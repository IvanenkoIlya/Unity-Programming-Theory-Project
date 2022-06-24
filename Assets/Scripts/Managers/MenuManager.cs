using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
   [SerializeField] List<GameObject> menuList;
   [SerializeField] float fadeDuration;

   private GameObject activeMenu;

   void Start()
   {
      Debug.Log("Start MenuManager");
      foreach(var menu in menuList)
      {
         var canvasGroup = menu.GetComponent<CanvasGroup>();
         canvasGroup.alpha = 0;
      }
   }

   private void Awake()
   {
      SwitchToMenu(0);
   }

   public void SwitchToMenu(int menuIndex)
   {
      if (menuIndex < menuList.Count)
         StartCoroutine(SwitchToMenu(menuList[menuIndex]));
      else
         Debug.Log($"Menu index {menuIndex} is out of bounds");
   }

   public void SwitchToMenu(string menuName)
   {
      var menu = menuList.FirstOrDefault(x => x.name == menuName);

      if(menu == null)
      {
         Debug.LogWarning($"No menu with name \"{menuName}\" found");
         return;
      }

      StartCoroutine(SwitchToMenu(menu));
   }

   IEnumerator SwitchToMenu(GameObject menu)
   {
      Debug.Log($"Switching to {menu.name}");
      if (activeMenu != null)
      {
         yield return FadeTo(0.0f, fadeDuration, activeMenu.GetComponent<CanvasGroup>());
         activeMenu.SetActive(false);
      }
      activeMenu = menu;
      activeMenu.SetActive(true);
      yield return FadeTo(1.0f, fadeDuration, activeMenu.GetComponent<CanvasGroup>());
   }

   private IEnumerator FadeTo(float targetAlpha, float duration, CanvasGroup canvasGroup)
   {
      float currentAlpha = canvasGroup.alpha;
      for(float i = 0.0f; i < 1.0; i += Time.deltaTime / duration)
      {
         canvasGroup.alpha = Mathf.Lerp(currentAlpha, targetAlpha, i);
         yield return null;
      }
   }

   public void SwitchToMainMenu()
   {
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
      StartCoroutine(StartGameCoroutine());
   }

   public IEnumerator StartGameCoroutine()
   {
      yield return FadeTo(0.0f, fadeDuration, activeMenu.GetComponent<CanvasGroup>());
      SceneManager.LoadScene(1);
   }

   public void ExitGame()
   {
#if UNITY_EDITOR
      EditorApplication.ExitPlaymode();
#else
      Application.Quit();
#endif
   }
}
