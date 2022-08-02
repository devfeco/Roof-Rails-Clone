using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Touch touch;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float swipeSpeed;
    [SerializeField]
    private float xRange;
    private void FixedUpdate()
    {
        if (transform.position.x > xRange)
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        else if (transform.position.x < -xRange)
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
    }
    void Update()
    {                       
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(
                transform.position.x + touch.deltaPosition.x * swipeSpeed,
                transform.position.y,
                transform.position.z);
            }
        }
    }
}
