using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! Spawns packages in factory.
public class PackageSpawner : MonoBehaviour
{
    public List<GameObject> packagePrefabs;
    public Vector2 spawnTiming;
    private List<GameObject> wayPoints;
    private float timeBetween = 3f;
    private float lastTime;

    void Start()
    {
        timeBetween = Random.Range(spawnTiming.x, spawnTiming.y);
        lastTime = Time.time;
        wayPoints = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.gameObject.GetComponent<WayPoint>())
            {
                wayPoints.Add(child.gameObject);
            }
        }
    }

    void Update()
    {
        if (Time.time - lastTime > timeBetween)
        {
            lastTime = Time.time;
            timeBetween = Random.Range(spawnTiming.x, spawnTiming.y);
            SpawnPackage();
        }
    }

    void SpawnPackage()
    {
        var package = GameObject.Instantiate(packagePrefabs[Random.Range(0, packagePrefabs.Count - 1)]);
        package.GetComponent<Package>().wayPoints = wayPoints;
        package.SetActive(true);
    }
}
