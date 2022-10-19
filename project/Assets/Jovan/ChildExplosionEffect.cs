using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildExplosionEffect : MonoBehaviour
{

    private Vector3 parentPosition;
    private Vector3 originalPosition;
    private Vector3 parentOffset;
    private Vector3 randomOffset;
    private Vector3 randomRotation;

    public float explosionAmount = 0.2f;
    public float explosionAnimationAmount = 0.2f;

    public bool unexplode = false;
    
    void Start()
    {
        parentPosition = transform.parent.position;
        originalPosition = transform.position;
        parentOffset = originalPosition - parentPosition;
        randomOffset = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        randomRotation = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        randomRotation *= 20f;
        //randomRotation = randomRotation * 20;
        transform.Rotate(randomRotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (unexplode == true)
        {
            explosionAmount -= 0.25f * Time.deltaTime;
            if (explosionAmount < 0) {
                explosionAmount = 0;
                unexplode = false;
            }

        }
        transform.position = originalPosition + parentOffset * explosionAmount + randomOffset * (Mathf.Sin(Time.time) / 2 + 0.5f) * explosionAnimationAmount;
    }

    public void ReturnToNormal()
    {
        unexplode = true;
    }
}
