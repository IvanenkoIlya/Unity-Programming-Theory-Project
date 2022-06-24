using System.Collections;
using UnityEngine;

public class DataCoroutine<T>
{
   public Coroutine coroutine { get; private set; }
   public T result;
   private IEnumerator target;

   public DataCoroutine(MonoBehaviour owner, IEnumerator targetCoroutine)
   {
      target = targetCoroutine;
      coroutine = owner.StartCoroutine(Run());
   }

   private IEnumerator Run()
   {
      while (target.MoveNext())
      {
         result = (T)target.Current;
         yield return result;
      }
   }
}
