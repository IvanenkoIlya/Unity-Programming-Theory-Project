using System.Collections;
using UnityEngine;

public class Chest : Interact
{
   public bool stayOpen = true;

   private Animator animator;
   private Animator contentsAnimator;
   private ParticleSystem particles;
   private bool isOpen;

   private void Start()
   {
      animator = GetComponent<Animator>();
      contentsAnimator = transform.GetChild(1).GetComponent<Animator>();
      particles = GetComponent<ParticleSystem>();

      isOpen = false;
   }

   protected override void OnInteract()
   {
      if (!isOpen)
      {
         isOpen = true;
         StartCoroutine(HideInteractPrompt(0.1f));

         StartCoroutine(GiveSword());
      }
   }

   private IEnumerator GiveSword()
   {
      animator.SetBool("b_IsOpen", isOpen);
      contentsAnimator.SetBool("b_ShowLoot", true);
      particles.Play();
      yield return new WaitForSeconds(1.75f);
      GameObject.Find("PrototypeHero").GetComponent<PrototypeHeroDemo>().GiveSword();
   } 

   protected override void OnExitRange()
   {
      if(isOpen)
      {
         if (!stayOpen)
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
}
