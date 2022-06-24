using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
   public static PlayerInputManager Instance;

   public PlayerControls PlayerControls { get; private set; }

   private void Awake()
   {
      if (Instance != null)
      {
         Destroy(gameObject);
         return;
      }
      
      Instance = this;
      PlayerControls = new PlayerControls();
      PlayerControls.UI.Pause.performed += ctx => GameManager.Instance.TogglePause();
      DontDestroyOnLoad(this);
   }
}
