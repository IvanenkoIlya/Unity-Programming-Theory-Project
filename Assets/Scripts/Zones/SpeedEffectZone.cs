using UnityEngine;

// INHERITANCE
public class SpeedEffectZone : EffectZone
{
   [SerializeField] float speedMultiplier;

   // POLYMORPHISM
   protected override void ApplyEffect(GameObject player)
   {
      player.GetComponent<PrototypeHeroDemo>().m_maxSpeed *= speedMultiplier;
   }

   // POLYMORPHISM
   override protected void RemoveEffect(GameObject player)
   {
      player.GetComponent<PrototypeHeroDemo>().m_maxSpeed /= speedMultiplier;
   }
}
