using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingLight : MonoBehaviour
{
    public Vector3 startAngle;
    public Vector3 endAngle;
    private Quaternion startAngleQ;
    private Quaternion endAngleQ;
    public Vector2 animationTime;
    private float timing;
    private float lastTime;
    private bool direction;

    // Start is called before the first frame update
    void Start()
    {
        timing = Random.Range(animationTime.x, animationTime.y);
        startAngleQ = Quaternion.Euler(startAngle);
        endAngleQ = Quaternion.Euler(endAngle);
        transform.localRotation = startAngleQ;
        lastTime = Time.time + Random.Range(0, timing);
    }

    // Update is called once per frame
    void Update()
    {
        float lerpTime = Mathf.Clamp((Time.time - lastTime) / timing, 0, 1);
        if (!direction)
        {
            transform.localRotation = QuaternionHelpers.slerp(startAngleQ, endAngleQ, lerpTime);
        }
        else
        {
            transform.localRotation = QuaternionHelpers.slerp(endAngleQ, startAngleQ, lerpTime);
        }
        if (Time.time - lastTime >= timing)
        {
            lastTime = Time.time;
            direction = !direction;
            timing = Random.Range(animationTime.x, animationTime.y);
        }
    }
}