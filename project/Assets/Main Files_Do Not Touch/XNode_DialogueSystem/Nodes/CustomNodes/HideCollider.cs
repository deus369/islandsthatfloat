using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

// If you want to create a custom node, duplicate this and rename the class and the string. 
public class HideCollider : BaseNode{ //rename CustomNode here

    [Input] public Connection entry;
    [Output] public Connection exit;
    public override string GetString()
    {
        return "HideCollider"; //rename CustomNode here
    }
    public override object GetValue(NodePort port){
		return null;
	}

    
}
