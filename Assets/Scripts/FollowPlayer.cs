using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
   [SerializeField] private GameObject player;
   [SerializeField] private float leftBound = -7.5f;
   [SerializeField] private float rightBound = 17.5f;

   public bool followPlayer = true;
   public Vector3 offset;

   private void Start()
   {
      if (player == null && !TryFindPlayer(out player))
         Debug.LogWarning($"[{name}] Unable to find player to follow");
   }

   private void LateUpdate()
   {
      if (player != null && followPlayer)
      {
         Vector3 expectedPos = player.transform.position + offset;
         expectedPos.x = Mathf.Clamp(expectedPos.x, leftBound, rightBound);
         transform.position = expectedPos;
      }
   }

   private bool TryFindPlayer(out GameObject player)
   {
      player = GameObject.FindGameObjectWithTag("Player");
      return player != null;
   }
}
