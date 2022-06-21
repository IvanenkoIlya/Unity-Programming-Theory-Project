using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
   public static PlayerInputManager Instance;

   public PlayerControls PlayerControls { get; private set; }
   //public InputDevice LastUsedDevice { get; private set; }

   private void Awake()
   {
      if (Instance != null)
      {
         Destroy(gameObject);
         return;
      }
      
      Instance = this;
      PlayerControls = new PlayerControls();
      DontDestroyOnLoad(this);
   }
}
