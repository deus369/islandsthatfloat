using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> wayPoints;
    public Vector3 offset;
    private GameObject targetWaypoint;
    private Vector3 startPosition;
    private float lastTime;
    private float timeTaking;
    private int wayPointIndex;

    void Start()
    {
        lastTime = Time.time;
        startPosition = wayPoints[0].transform.position;
        transform.position = startPosition + offset;
        wayPointIndex = 1;
        targetWaypoint = wayPoints[wayPointIndex];
        timeTaking = targetWaypoint.GetComponent<WayPoint>().GetTiming();
    }

    // Update is called once per frame
    void Update()
    {
        var timePassed = Time.time - lastTime;
        if (timePassed >= timeTaking)
        {
            lastTime = Time.time;
            wayPointIndex++;
            if (wayPointIndex >= wayPoints.Count)
            {
                Destroy(gameObject);
                return;
            }
            startPosition = transform.position;
            targetWaypoint = wayPoints[wayPointIndex];
            timeTaking = targetWaypoint.GetComponent<WayPoint>().GetTiming();
            return;
        }
        transform.position = Vector3.Lerp(
            startPosition,
            targetWaypoint.transform.position,
            timePassed / timeTaking) + offset;
    }
}
