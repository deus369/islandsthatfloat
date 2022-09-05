using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChange : MonoBehaviour
{

    public Material skyOne;
    public Material skyTwo;
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = skyOne;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
