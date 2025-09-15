using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerController : MonoBehaviour
{
    [Header("Bounce")]
    [Range(0f, 5f)] public float bounceHeight = 2f;
    [Range(0.1f, 10f)] public float bounceSpeed = 2f;
    public AnimationCurve easeOutCurve;

    [Header("Fall (constant speed)")]
    [Range(0.5f, 20f)] public float fallSpeed = 6f;     // lowered default
    public float groundCheckDistance = 2f;
    public LayerMask groundMask;

    [Header("Tags")]
    public string groundTag = "ground";
    public string enemyTag = "enemy";
    public string stageTag = "stage";

    private Vector3 bounceBase;
    private float timer;
    private bool isFalling;
    private bool isFrozen;
    private Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
        if (easeOutCurve == null || easeOutCurve.length == 0)
            easeOutCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        bounceBase = transform.position;
    }

    void Update()
    {
        if (isFrozen) return;

        bool inputDown = Input.GetMouseButton(0) || Input.touchCount > 0;

        if (inputDown)
        {
            if (!isFalling)
            {
                isFalling = true;
                SetColliderTrigger(true); // pass through ground
            }
            // CONSTANT SPEED FALL (no acceleration)
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            return;
        }

        // released -> go back to bounce if possible
        if (isFalling)
        {
            isFalling = false;
            timer = 0f;
            SetColliderTrigger(false);
        }

        // bounce only if ground below
        if (TryGetGroundBelow(out var anchor))
        {
            bounceBase = anchor;
            timer += Time.deltaTime * bounceSpeed;
            float t = Mathf.PingPong(timer, 1f);
            float curve = easeOutCurve.Evaluate(t);
            transform.position = bounceBase + Vector3.up * bounceHeight * curve;
        }
    }

    private bool TryGetGroundBelow(out Vector3 anchor)
    {
        anchor = transform.position;
        Ray ray = new Ray(transform.position + Vector3.up * 0.05f, Vector3.down);
        if (Physics.Raycast(ray, out var hit, groundCheckDistance,
                groundMask.value != 0 ? groundMask : Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (groundMask.value == 0 && !hit.collider.CompareTag(groundTag)) return false;
            anchor = hit.point + Vector3.up * GetHalfHeight();
            return true;
        }
        return false;
    }

    private float GetHalfHeight()
    {
        if (col is SphereCollider s) return s.radius * Mathf.Abs(transform.localScale.y);
        if (col is CapsuleCollider c) return (c.height * 0.5f) * Mathf.Abs(transform.localScale.y);
        if (col is BoxCollider b) return (b.size.y * 0.5f) * Mathf.Abs(transform.localScale.y);
        return 0.5f * Mathf.Abs(transform.localScale.y);
    }

    private void SetColliderTrigger(bool isTrigger)
    {
        if (col != null) col.isTrigger = isTrigger;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag) || other.CompareTag(stageTag)) Freeze();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(enemyTag) || collision.collider.CompareTag(stageTag)) Freeze();
    }

    private void Freeze()
    {
        isFrozen = true;
        isFalling = false;
        SetColliderTrigger(false);
    }
}
