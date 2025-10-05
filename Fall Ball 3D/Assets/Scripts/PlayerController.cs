using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("Visual bounce")]
    public float bounceStepDuration = 0.5f;
    public float basePos = 1f;
    public float bouncePos = 5f;

    [Header("Falling (parent)")]
    public float fallSpeed = 10f;
    public float ballSnapDuration = 0.3f;

    [Header("Tags")]
    public string blockTag = "block";
    public string enemyTag = "enemy";
    public string stageTag = "stage";

    GameObject ball;

    Tween bounceTw;
    Tween fallTw;
    Tween snapTw;

    bool falling;
    [SerializeField] public bool isHostile;
    bool correctingTween = false;
    bool isShuttingDown = false;
    bool groundedLock = false;

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
        var lp = ball.transform.localPosition;
        ball.transform.localPosition = new Vector3(lp.x, basePos, lp.z);
        StartIdleLoop();
        bounceStepDuration = 0.5f;
        fallSpeed = 10f;
        ballSnapDuration = 0.3f;

        GameManager.instance.isPlaying = true;

}

    public void PlayerSetting(float _totalNofBlocks)
    {
        totalNofBlocks = _totalNofBlocks;
        curYpos = totalNofBlocks * 2f + 1f;
        updateYpos = curYpos;
    }

    void Update()
    {
        if (!GameManager.instance.islevelPassed || !GameManager.instance.isDead)
        {
            if (CameraFlow.instance) CameraFlow.instance.restructureCamPos("move", false, transform.position.y);
            if (correctingTween) return;

            bool rawHeld = IsHeld();
            if (!rawHeld) groundedLock = false;
            bool inputHeld = rawHeld && !groundedLock;

            if (inputHeld == falling) goto GroundClamp;

            falling = inputHeld;

            bounceTw?.Kill(false);
            fallTw?.Kill(false);
            snapTw?.Kill(false);

            if (falling && transform.position.y >= basePos)
            {
                isHostile = true;

                snapTw = ball.transform
                    .DOLocalMoveY(basePos, Mathf.Max(0.01f, ballSnapDuration))
                    .SetEase(Ease.Linear);

                float bigDistance = 1000f;
                float duration = bigDistance / Mathf.Max(0.01f, fallSpeed);

                fallTw = transform
                    .DOMoveY(transform.position.y - bigDistance, duration)
                    .SetEase(Ease.Linear)
                    .OnKill(() =>
                    {
                        if (!falling && !isShuttingDown) checkYpos();
                    });
            }
            else
            {
                isHostile = false;

                var pos = ball.transform.position;
                float targetWorldY = transform.position.y + basePos;
                ball.transform.position = new Vector3(pos.x, targetWorldY, pos.z);

                StartIdleLoop();
            }

        GroundClamp:
            {
                if (transform.position.y <= basePos)
                {
                    transform.position = new Vector3(transform.position.x, basePos, transform.position.z);

                    fallTw?.Kill(false);
                    snapTw?.Kill(false);
                    isHostile = false;
                    falling = false;

                    groundedLock = true;

                    var bpos = ball.transform.position;
                    ball.transform.position = new Vector3(bpos.x, basePos + transform.position.y - basePos, bpos.z);
                    ball.transform.localPosition = new Vector3(ball.transform.localPosition.x, basePos, ball.transform.localPosition.z);

                    bounceTw?.Kill(false);
                    StartIdleLoop();
                }
            }
            if (transform.position.y == basePos + 0.5f)
            {
                GameManager.instance.GameOver("cleared");
            }
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
