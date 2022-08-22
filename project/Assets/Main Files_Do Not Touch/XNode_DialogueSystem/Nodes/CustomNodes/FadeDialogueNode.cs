using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

// If you want to create a custom node, duplicate this and rename the class and the string. 
public class FadeDialogueNode : BaseNode{ //rename CustomNode here

    [Input] public Connection input;
    [Output] public Connection exit;
    public string speakerName;
	[TextArea] public string dialogueLine;
    public override string GetString()
    {
        return "FadeDialogueNode/" + speakerName + "/" + dialogueLine; 
    }
    public override object GetValue(NodePort port){
		return null;
	}

    
}
