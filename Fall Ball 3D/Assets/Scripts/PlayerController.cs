using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerController : MonoBehaviour
{
    [Header("Bounce (linear)")]
    [Range(0f, 10f)] public float bounceHeight = 2f;
    [Range(0.1f, 20f)] public float bounceSpeed = 2f; // world units/sec

    [Header("Fall (constant speed)")]
    [Range(0.1f, 30f)] public float fallSpeed = 6f;
    public float groundCheckDistance = 2f;
    public LayerMask groundMask;

    [Header("Tags (only used if groundMask == 0)")]
    public string groundTag = "ground";
    public string enemyTag = "enemy";
    public string stageTag = "stage";

    private Collider col;

    // Bounce state
    private bool isFalling, isFrozen, rising;
    private float t;                 // 0..1 phase
    private Vector3 bounceBase;      // fixed for the current bounce cycle
    private bool hasAnchor;          // do we have a valid base?

    // NEW: the BlockController beneath the player (if any)
    public BlockController currentBlockBelow { get; private set; }

    void Start()
    {
        col = GetComponent<Collider>();
        rising = true; t = 0f;

        if (TryGetGroundBelow(out var anchor, out var block))
        {
            bounceBase = anchor; hasAnchor = true;
            currentBlockBelow = block;
        }
    }

    void FixedUpdate()
    {
        if (isFrozen) return;

        // One raycast per physics tick (also fetch BlockController)
        bool groundHit = TryGetGroundBelow(out var groundAnchor, out var blockBelow);
        currentBlockBelow = blockBelow; // keep this updated every tick

        bool inputDown = Input.GetMouseButton(0) || Input.touchCount > 0;

        if (inputDown)
        {
            if (!isFalling) { isFalling = true; SetTrigger(true); }
            transform.position += Vector3.down * fallSpeed * Time.fixedDeltaTime;
            hasAnchor = false;
            return;
        }

        if (isFalling)
        {
            isFalling = false;
            SetTrigger(false);

            if (groundHit)
            {
                bounceBase = groundAnchor;
                hasAnchor = true;

                float offset = (transform.position.y - bounceBase.y) / Mathf.Max(0.0001f, bounceHeight);
                t = Mathf.Clamp01(offset);

                if (t <= 0.001f) rising = true;
                else if (t >= 0.999f) rising = false;
            }
        }

        if (!hasAnchor)
        {
            if (groundHit) { bounceBase = groundAnchor; hasAnchor = true; }
            else return;
        }

        // convert world speed -> phase delta
        float phaseDelta = (bounceSpeed / Mathf.Max(0.0001f, bounceHeight)) * Time.fixedDeltaTime;
        t += rising ? phaseDelta : -phaseDelta;

        if (t >= 1f) { t = 1f; rising = false; }
        else if (t <= 0f) { t = 0f; rising = true; }

        Vector3 target = bounceBase + Vector3.up * (bounceHeight * t);
        transform.position = target;
    }

    // UPDATED: also returns the BlockController found on the hit object (or its parents)
    private bool TryGetGroundBelow(out Vector3 anchor, out BlockController block)
    {
        anchor = transform.position;
        block = null;

        Ray ray = new Ray(transform.position + Vector3.up * 0.05f, Vector3.down);
        if (Physics.Raycast(ray, out var hit, groundCheckDistance,
                groundMask.value != 0 ? groundMask : Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            // If no mask, enforce tag for anchoring bounce
            if (groundMask.value == 0 && !hit.collider.CompareTag(groundTag))
                return false;

            anchor = hit.point + Vector3.up * GetHalfHeight();

            // Try to get BlockController on the hit object or its parents
            block = hit.collider.GetComponent<BlockController>()
                    ?? hit.collider.GetComponentInParent<BlockController>();

            return true;
        }
        return false;
    }

    private float GetHalfHeight()
    {
        if (col is SphereCollider s) return s.radius * Mathf.Abs(transform.localScale.y);
        if (col is CapsuleCollider c) return (c.height * 0.5f) * Mathf.Abs(transform.localScale.y);
        if (col is BoxCollider b) return (b.size.y * 0.5f) * Mathf.Abs(transform.localScale.y);
        return 0.5f;
    }

    private void SetTrigger(bool isTrigger)
    {
        if (col != null) col.isTrigger = isTrigger;
    }

    // (Optional) If you no longer want to stop on enemy/stage, remove these two handlers.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag) || other.CompareTag(stageTag)) Freeze();
    }
    private void OnCollisionEnter(Collision c)
    {
        if (c.collider.CompareTag(enemyTag) || c.collider.CompareTag(stageTag)) Freeze();
    }

    private void Freeze()
    {
        isFrozen = true;
        isFalling = false;
        SetTrigger(false);
    }
}
