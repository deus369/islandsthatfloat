using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

// If you want to create a custom node, duplicate this and rename the class and the string. 
public class PropInteraction : BaseNode{ //rename CustomNode here

    [Input] public Connection entry;
    [Output] public Connection exit;
    public override string GetString()
    {
        return "PropInteraction"; //rename CustomNode here
    }
    public override object GetValue(NodePort port){
		return null;
	}
    //public GameObject Model;
    /* public override GameObject GetOBJ(){
        return Model;
    } */

    
}
