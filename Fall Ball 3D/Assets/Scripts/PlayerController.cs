using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("Visual bounce")]
    public float bounceStepDuration = 0.5f;
    public float basePos = 1f;
    public float bouncePos = 5f;

    [Header("Falling (parent)")]
    public float fallSpeed = 10f;       // units/sec while held
    public float ballSnapDuration = 0.1f;

    [Header("Tags")]
    public string blockTag = "block";
    public string enemyTag = "enemy";
    public string stageTag = "stage";

    // refs
    GameObject ball;

    // tweens
    Tween bounceTw;   // idle bounce (child)
    Tween fallTw;     // continuous fall (parent)
    Tween snapTw;     // snap ball to base on fall start

    // state
    bool falling;                     // input-held state
    [SerializeField] public bool isHostile; // destroy blocks when true
    bool correctingTween = false;
    bool isShuttingDown = false;

    // height bookkeeping
    float curYpos;
    float updateYpos;
    float totalNofBlocks;

    public static PlayerController instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ball = transform.GetChild(0).gameObject;

        // ensure ball starts at base
        var lp = ball.transform.localPosition;
        ball.transform.localPosition = new Vector3(lp.x, basePos, lp.z);

        StartIdleLoop();
    }

    public void PlayerSetting(float _totalNofBlocks)
    {
        totalNofBlocks = _totalNofBlocks;
        curYpos = totalNofBlocks * 2f + 1f;
        updateYpos = curYpos;
#if UNITY_EDITOR
        // Debug.Log($"PlayerSetting: total={totalNofBlocks} cur={curYpos} target={updateYpos}");
#endif
    }

    void Update()
    {
        // camera follow
        if (CameraFlow.instance) CameraFlow.instance.restructureCamPos("move", false, transform.position.y);

        if (correctingTween) return; // pause transitions during correction

        bool inputHeld = IsHeld();
        if (inputHeld == falling) return; // no state change

        falling = inputHeld;

        // kill active tweens without completing
        bounceTw?.Kill(false);
        fallTw?.Kill(false);
        snapTw?.Kill(false);

        if (falling)
        {
            isHostile = true;

            // 1) snap BALL (child) to base
            snapTw = ball.transform
                .DOLocalMoveY(basePos, Mathf.Max(0.01f, ballSnapDuration))
                .SetEase(Ease.Linear);

            // 2) continuous FALL of PARENT at constant speed
            float bigDistance = 1000f;
            float duration = bigDistance / Mathf.Max(0.01f, fallSpeed);

            fallTw = transform
                .DOMoveY(transform.position.y - bigDistance, duration)
                .SetEase(Ease.Linear)
                .OnKill(() =>
                {
                    // called when falling tween is stopped (input released)
                    if (!falling && !isShuttingDown) checkYpos();
                });
        }
        else
        {
            isHostile = false;

            // align BALL world Y to parent's current baseline before resuming bounce
            var pos = ball.transform.position;
            float targetWorldY = transform.position.y + basePos;
            ball.transform.position = new Vector3(pos.x, targetWorldY, pos.z);

            StartIdleLoop();
        }
    }

    public void checkYpos()
    {
        curYpos = transform.position.y;
        if (!Mathf.Approximately(curYpos, updateYpos))
        {
            correctingTween = true;
            transform
                .DOMoveY(updateYpos, 0.1f)
                .SetEase(Ease.Linear)
                .OnComplete(() => { correctingTween = false; });
        }
    }

    void StartIdleLoop()
    {
        var seq = DOTween.Sequence();
        seq.Append(ball.transform.DOLocalMoveY(bouncePos, bounceStepDuration).SetEase(Ease.InSine));
        seq.Append(ball.transform.DOLocalMoveY(basePos, bounceStepDuration).SetEase(Ease.OutQuad));
        bounceTw = seq.SetLoops(-1);
    }

    bool IsHeld()
    {
        return Input.GetMouseButton(0)
            || (Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended && Input.GetTouch(0).phase != TouchPhase.Canceled)
            || Input.GetKey(KeyCode.Space);
    }

        

    void OnCollisionEnter(Collision collision)
    {
        if (isHostile && collision.gameObject.CompareTag(blockTag))
            Destroy(collision.gameObject);

        totalNofBlocks--;
        updateYpos -= 2f;
    }
    void OnDisable()
    {
        isShuttingDown = true;
        bounceTw?.Kill(false);
        fallTw?.Kill(false);
        snapTw?.Kill(false);
    }
}
