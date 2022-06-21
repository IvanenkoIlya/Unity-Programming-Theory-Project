using UnityEngine;

public class Chest : Interact
{
   public bool stayOpen = true;

   private Animator animator;
   private bool isOpen;

   private void Start()
   {
      animator = GetComponent<Animator>();
      isOpen = false;
   }

   protected override void OnInteract()
   {
      if (!isOpen)
      {
         isOpen = true;
         StartCoroutine(HideInteractPrompt(0.1f));
         animator.SetBool("b_IsOpen", isOpen);

         // TODO give item / open inventory / etc.
      }
   }

   protected override void OnExitRange()
   {
      if(!stayOpen)
      {
         isOpen = false;
         animator.SetBool("b_IsOpen", isOpen);
      }
      else
      {
         interactionEnabled = false;
      }
   }
}
