using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("Visual bounce")]
    public float bounceStepDuration = 0.1f; // time to go base -> bounce or bounce -> base
    public float basePos = 1f;
    public float bouncePos = 5f;

    [Header("Falling (parent)")]
    public float fallSpeed = 2f;            // units/sec while input is held
    public float ballSnapDuration = 0.1f;  // time to snap ball to base when falling starts

    [Header("Tags")]
    public string blockTag = "block";
    public string enemyTag = "enemy";
    public string stageTag = "stage";

    GameObject ball;

    Tween bounceTw;   // idle bounce sequence (on ball)
    Tween fallTw;     // continuous fall (on parent)
    Tween snapTw;     // snap ball to base when entering falling
    bool falling;     // input-held state
    bool isHostile;   // destroy blocks only when true

    

    void Start()
    {
        ball = transform.GetChild(0).gameObject;

        // Start ball at base (so it never starts frozen at the top)
        var lp = ball.transform.localPosition;
        ball.transform.localPosition = new Vector3(lp.x, basePos, lp.z);

        StartIdleLoop();
    }

    void Update()
    {
        bool inputHeld = IsHeld();

        CameraFlow.instance.restructureCamPos("move", false, transform.position.y);
        if (inputHeld != falling) // state changed
        {
            if (CameraFlow.instance != null) falling = inputHeld;

            // Stop tweens but DO NOT complete them (keeps the parent at its current Y)
            bounceTw?.Kill(false);
            fallTw?.Kill(false);
            snapTw?.Kill(false);

            if (falling)
            {
                isHostile = true;
                
                // snap the BALL down to base from current local Y
                snapTw = ball.transform
                    .DOLocalMoveY(basePos, 0.25f)
                    .SetEase(Ease.Linear);

                // continuous fall of the PARENT
                float bigDistance = 1000f, duration = bigDistance / Mathf.Max(0.01f, fallSpeed);
                fallTw = transform
                    .DOMoveY(transform.position.y - bigDistance, duration)
                    .SetEase(Ease.Linear);
               
            }
            else
            {
                isHostile = false;

                // ***** KEY PART: align ball's WORLD Y to the parent's current baseline (point 2)
                var pos = ball.transform.position;
                float targetWorldY = transform.position.y + basePos; // base relative to new parent height
                ball.transform.position = new Vector3(pos.x, targetWorldY, pos.z);

                StartIdleLoop(); // now bounce starts from point 2
            }
        }
    }

    void StartIdleLoop()
    {
        // Ball bounces between base and bounce while idle
        var seq = DOTween.Sequence();
        seq.Append(ball.transform.DOLocalMoveY(bouncePos, bounceStepDuration).SetEase(Ease.InSine));
        seq.Append(ball.transform.DOLocalMoveY(basePos, bounceStepDuration).SetEase(Ease.OutQuad));
        bounceTw = seq.SetLoops(-1);
    }

    bool IsHeld()
    {
        if (Input.GetMouseButton(0)) return true;
        if (Input.touchCount > 0)
        {
            var ph = Input.GetTouch(0).phase;
            if (ph != TouchPhase.Ended && ph != TouchPhase.Canceled) return true;
        }
        if (Input.GetKey(KeyCode.Space)) return true;
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isHostile && collision.gameObject.CompareTag(blockTag))
            Destroy(collision.gameObject);
    }

    void OnCollisionStay(Collision collision)
    {
        if (isHostile && collision.gameObject.CompareTag(blockTag))
            Destroy(collision.gameObject);
    }

    void OnDisable()
    {
        bounceTw?.Kill();
        fallTw?.Kill();
        snapTw?.Kill();
    }
}
