using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public Transform blockObject;
    private Vector2 touchStart;
    public float t_rotationSpeed = 0.3f;
    public float a_rotationSpeed = 30f;
    public bool isTouch = false;
    void Update()
    {
        if (isTouch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 delta = (Vector2)Input.mousePosition - touchStart;
                touchStart = Input.mousePosition;

                float rotationY = -delta.x * t_rotationSpeed;
                blockObject.Rotate(0, rotationY, 0);
            }
        }
        else blockObject.Rotate(0, a_rotationSpeed * Time.deltaTime, 0);
    }
    
}
