using UnityEngine;
using UnityEngine.EventSystems;

public class MoveText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   [SerializeField] GameObject text;
   [SerializeField] Vector3 offset;

   private Vector3 originalPosition;

   private void Start()
   {
      originalPosition = text.transform.position;
   }

   public void OnPointerEnter(PointerEventData eventData)
   {
      if (text != null)
         text.transform.position += offset;
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      if (text != null)
         text.transform.position -= offset;
   }

   private void OnDisable()
   {
      text.transform.position = originalPosition;
   }
}
