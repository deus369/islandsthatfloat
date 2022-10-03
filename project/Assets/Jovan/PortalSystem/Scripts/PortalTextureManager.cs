using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureManager : MonoBehaviour
{
	public Camera[] cameras;
	public Material[] cameraMat;
	//public Portal[] PortalTextures;
	void Start () {
		for(int i = 0; i < cameras.Length; i++){
	
			if (cameras[i].targetTexture != null){
				cameras[i].targetTexture.Release();
			}
			cameras[i].targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
			//PortalTextures[i] = 
			cameraMat[i].mainTexture = cameras[i].targetTexture;              
            }  
	}

}