using UnityEngine;

public class JumpEffectZone : EffectZone
{
   [SerializeField] float jumpMultiplier;

   protected override void ApplyEffect(GameObject player)
   {
      var playerController = player.GetComponent<PrototypeHeroDemo>();
      playerController.m_jumpForce *= jumpMultiplier;
   }

   protected override void RemoveEffect(GameObject player)
   {
      var playerController = player.GetComponent<PrototypeHeroDemo>();
      playerController.m_jumpForce /= jumpMultiplier;
   }
}
