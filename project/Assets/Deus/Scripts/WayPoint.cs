using UnityEngine;

public class WayPoint : MonoBehaviour
{
    //! Time to arrive since last node
    public Vector2 timeToArrive;

    public float GetTiming()
    {
        return Random.Range(timeToArrive.x, timeToArrive.y);
    }
}
