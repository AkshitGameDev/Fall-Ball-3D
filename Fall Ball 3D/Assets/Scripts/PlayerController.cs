using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerController : MonoBehaviour
{
    [Header("Bounce (linear)")]
    [Range(0f, 10f)] public float bounceHeight = 2f;
    [Range(0.1f, 20f)] public float bounceSpeed = 2f;  // units per second along the 0..1 bounce phase

    [Header("Fall (constant speed)")]
    [Range(0.1f, 30f)] public float fallSpeed = 6f;
    public float groundCheckDistance = 2f;
    public LayerMask groundMask;

    [Header("Tags")]
    public string groundTag = "ground";
    public string enemyTag = "enemy";
    public string stageTag = "stage";

    private Collider col;
    private Vector3 basePos;
    private bool isFalling;
    private bool isFrozen;

    // Linear bounce state
    private float t = 0f;     // 0..1 (0 = base, 1 = top)
    private bool rising = true;

    void Start()
    {
        col = GetComponent<Collider>();
        basePos = transform.position;
        t = 0f;
        rising = true; // start from base -> go up
    }

    void Update()
    {
        if (isFrozen) return;

        bool inputDown = Input.GetMouseButton(0) || Input.touchCount > 0;

        if (inputDown)
        {
            // start/keep falling from CURRENT position (no snap)
            if (!isFalling)
            {
                isFalling = true;
                SetTrigger(true); // pass through ground
            }
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            return;
        }

        // released -> stop falling, resume bounce linearly
        if (isFalling)
        {
            isFalling = false;
            SetTrigger(false);

            // Re-anchor bounce to ground and sync phase t to current height
            if (TryGetGroundBelow(out var anchor))
            {
                basePos = anchor;

                float offset = (transform.position.y - basePos.y) / Mathf.Max(0.0001f, bounceHeight);
                t = Mathf.Clamp01(offset);

                // Always continue the pattern "down -> up -> down":
                // If we're at/near base, start rising; if above base, keep current direction if sensible.
                if (t <= 0.001f) rising = true;
                else if (t >= 0.999f) rising = false;
                // otherwise keep 'rising' as-is
            }
        }

        // BOUNCE (linear triangle wave around ground anchor)
        if (TryGetGroundBelow(out var anchor2))
        {
            basePos = anchor2;

            // advance linear phase
            float delta = (bounceSpeed / Mathf.Max(0.0001f, bounceHeight)) * Time.deltaTime; // convert world u/s to phase u/s
            t += (rising ? +delta : -delta);

            if (t >= 1f) { t = 1f; rising = false; }
            else if (t <= 0f) { t = 0f; rising = true; }

            Vector3 target = basePos + Vector3.up * (bounceHeight * t);
            transform.position = target;
        }
        // else: no ground below -> hold current position
    }

    private bool TryGetGroundBelow(out Vector3 anchor)
    {
        anchor = transform.position;

        Ray ray = new Ray(transform.position + Vector3.up * 0.05f, Vector3.down);
        if (Physics.Raycast(ray, out var hit, groundCheckDistance,
                groundMask.value != 0 ? groundMask : Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            // If no mask given, enforce tag
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
        return 0.5f;
    }

    private void SetTrigger(bool isTrigger)
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
        SetTrigger(false);
    }
}
