using UnityEngine;

public class SpeedEffectZone : EffectZone
{
   [SerializeField] float speedMultiplier;

   protected override void ApplyEffect(GameObject player)
   {
      player.GetComponent<PrototypeHeroDemo>().m_maxSpeed *= speedMultiplier;
   }

   override protected void RemoveEffect(GameObject player)
   {
      player.GetComponent<PrototypeHeroDemo>().m_maxSpeed /= speedMultiplier;
   }
}
