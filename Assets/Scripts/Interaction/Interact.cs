using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class Interact : MonoBehaviour
{
   [SerializeField] protected float fadeDuration;
   [SerializeField] float interactionRadius;
   [SerializeField] Sprite interactPromptSprite;

   private CircleCollider2D circleCollider;
   private GameObject m_interactPrompt;
   private Coroutine fadeCoroutine;
   private int m_ColCount = 0;

   protected bool interactionEnabled = true;

   protected GameObject interactPrompt
   {
      get
      {
         if (m_interactPrompt == null)
            CreateInteractPrompt();
         return m_interactPrompt;
      }
   }

   #region Unity Messages
   private void OnValidate()
   {
      if (circleCollider == null)
         GetCircleCollider();
      circleCollider.radius = interactionRadius;
   }

   private void OnEnable()
   {
      if(Application.IsPlaying(gameObject))
         PlayerInputManager.Instance.PlayerControls.Player.Interact.performed += ctx => OnInteract(ctx);
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (interactionEnabled && collision.CompareTag("Player"))
      {
         m_ColCount++;
         if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
         fadeCoroutine = StartCoroutine(ShowInteractPrompt(fadeDuration));
         OnEnterRange();
      }
   }

   private void OnTriggerExit2D(Collider2D collision)
   {
      if (interactionEnabled && collision.CompareTag("Player"))
      {
         m_ColCount--;
         if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
         fadeCoroutine = StartCoroutine(HideInteractPrompt(fadeDuration));
         OnExitRange();
      }
   }
   #endregion

   #region Initialization
   private void GetCircleCollider()
   {
      circleCollider = GetComponent<CircleCollider2D>();
      if (circleCollider == null)
         circleCollider = gameObject.AddComponent<CircleCollider2D>();
      circleCollider.isTrigger = true;
   }

   private void CreateInteractPrompt()
   {
      var parentRenderer = GetComponentInChildren<SpriteRenderer>();

      m_interactPrompt = new GameObject();
      m_interactPrompt.transform.parent = transform;
      m_interactPrompt.name = "Interact prompt";

      // Setup sprite renderer and set it to start invisible
      SpriteRenderer renderer = m_interactPrompt.AddComponent<SpriteRenderer>();
      renderer.sprite = interactPromptSprite;
      renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
      renderer.sortingLayerName = parentRenderer.sortingLayerName;

      // Set position of interact prompt above object
      float promptHeight = parentRenderer.bounds.size.y + renderer.bounds.size.y;
      m_interactPrompt.transform.localPosition = new Vector3(0, promptHeight, 0);

      m_interactPrompt.SetActive(false);
   }
   #endregion

   #region Virtual methods
   protected virtual void OnInteract()
   {
      Debug.Log("OnInteract");
   }

   protected virtual IEnumerator ShowInteractPrompt(float duration)
   {
      interactPrompt.SetActive(true);
      yield return FadeTo(1.0f, duration);
      fadeCoroutine = null;
   }

   protected virtual IEnumerator HideInteractPrompt(float duration)
   {
      yield return FadeTo(0.0f, duration);
      interactPrompt.SetActive(false);
      fadeCoroutine = null;
   }

   protected virtual void OnEnterRange()
   {
      Debug.Log("OnEnterRange");
   }

   protected virtual void OnExitRange()
   {
      Debug.Log("OnExitRange");
   }
   #endregion

   #region Helper methods
   private void OnInteract(InputAction.CallbackContext ctx)
   {
      if (m_ColCount > 0)
         OnInteract();
   }

   private IEnumerator FadeTo(float targetAlpha, float duration)
   {
      return FadeTo(targetAlpha, duration, interactPrompt);
   }

   protected IEnumerator FadeTo(float targetAlpha, float duration, GameObject target)
   {
      return FadeTo(targetAlpha, duration, target.GetComponent<SpriteRenderer>().material);
   }

   protected IEnumerator FadeTo(float targetAlpha, float duration, Material target)
   {
      Color startingColor = target.color;
      float startingAlpha = startingColor.a;
      for (float t = 0.0f; t < 1.0; t += Time.deltaTime / duration)
      {
         Color newColor = new Color(startingColor.r, startingColor.g, startingColor.b, Mathf.Lerp(startingAlpha, targetAlpha, t));
         target.color = newColor;
         yield return null;
      }
   }
   #endregion
}
