using UnityEngine;

[ExecuteInEditMode]
public abstract class EffectZone : MonoBehaviour
{
   public Color zoneColor;

   private void OnValidate()
   {
      var sr = GetComponent<SpriteRenderer>();
      sr.color = zoneColor;
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.tag == "Player")
         ApplyEffect(collision.gameObject);
   }

   private void OnTriggerExit2D(Collider2D collision)
   {
      if (collision.gameObject.tag == "Player")
         RemoveEffect(collision.gameObject);
   }

   protected virtual void ApplyEffect(GameObject player)
   {
      Debug.Log("Player entered zone");
   }

   protected virtual void RemoveEffect(GameObject player)
   {
      Debug.Log("Player left zone");
   }
}
