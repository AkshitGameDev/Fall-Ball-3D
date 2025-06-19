using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float bounceHeight = 2f;
    public float bounceSpeed = 2f;
    public AnimationCurve easeOutCurve;  // Create this in Inspector
    public float fallSpeed = 10f;

    private Vector3 startPosition;
    private float timer;
    private bool isFalling = false;

    void Start()
    {
        startPosition = transform.position;

        // If not set manually, use a default ease out curve
        if (easeOutCurve == null || easeOutCurve.length == 0)
        {
           if (isFalling) easeOutCurve = AnimationCurve.EaseInOut(0, 0, 0, 0);

           easeOutCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))  // Mobile tap/hold
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }

        if (isFalling)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
        else
        {
            timer += Time.deltaTime * bounceSpeed;
            float curveValue = easeOutCurve.Evaluate(Mathf.PingPong(timer, 1f));
            Vector3 newPosition = startPosition + Vector3.up * bounceHeight * curveValue;
            transform.position = newPosition;
        }
    }

}
