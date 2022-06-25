using UnityEngine;

// INHERITANCE
public class JumpEffectZone : EffectZone
{
   [SerializeField] float jumpMultiplier;

   // POLYMORPHISM
   protected override void ApplyEffect(GameObject player)
   {
      var playerController = player.GetComponent<PrototypeHeroDemo>();
      playerController.m_jumpForce *= jumpMultiplier;
   }

   // POLYMORPHISM
   protected override void RemoveEffect(GameObject player)
   {
      var playerController = player.GetComponent<PrototypeHeroDemo>();
      playerController.m_jumpForce /= jumpMultiplier;
   }
}
