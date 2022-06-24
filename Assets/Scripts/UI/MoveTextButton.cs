using UnityEngine;
using UnityEngine.UI;

public class MoveTextButton : Button
{
   private GameObject text;
   private Vector3 originalPosition;
   private Vector3 targetPosition;

   protected override void Awake()
   {
      base.Awake();

      text = transform.GetChild(0).gameObject;
      originalPosition = text.transform.localPosition;
      targetPosition = originalPosition + new Vector3(0, -8);
   }

   protected override void DoStateTransition(SelectionState state, bool instant)
   {
      base.DoStateTransition(state, instant);

      switch (state)
      {
         case SelectionState.Normal:
         case SelectionState.Disabled:
            text.transform.localPosition = originalPosition;
            break;
         case SelectionState.Highlighted:
         case SelectionState.Selected:
         case SelectionState.Pressed:
            text.transform.localPosition = targetPosition;
            break;

      }
   }
}
