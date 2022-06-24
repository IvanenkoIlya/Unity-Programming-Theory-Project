using UnityEngine;
using UnityEngine.UI;

public class MoveText : MonoBehaviour
{
   [SerializeField] Vector3 offset;
   GameObject text;
   Button button;

   private Vector3 originalPosition;
   private Vector3 targetPosition;

   void Awake()
   {
      text = transform.GetChild(0).gameObject;
      originalPosition = text.transform.position;
      targetPosition = originalPosition + offset;
   }

   //protected override void DoStateTransition(SelectionState state, bool instant)
   //{
   //   base.DoStateTransition(state, instant);

   //   switch(state)
   //   {
   //      case SelectionState.Normal:
   //      case SelectionState.Disabled:
   //         text.transform.position = originalPosition;
   //         break;
   //      case SelectionState.Highlighted:
   //      case SelectionState.Selected:
   //      case SelectionState.Pressed:
   //         text.transform.position = targetPosition;
   //         break;

   //   }
   //}
}
