using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectIndex : MonoBehaviour
{
    public _NodeUpdater nodeUpdater;
    public int particleNumber;
    public int propNumber;
    public int npcNumber; 
    //public int fragmentedPropNumber;
    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Player"){
            nodeUpdater.p = propNumber; 
            nodeUpdater.f = particleNumber; 
            nodeUpdater.n = npcNumber;
        }
        
    }
}
