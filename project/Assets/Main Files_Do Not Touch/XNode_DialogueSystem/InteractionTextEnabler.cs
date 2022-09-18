using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionTextEnabler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interact_Text;
    [SerializeField] private InteractionInstigator m_WatchedInteractionInstigator;

    void Update(){
        
        interact_Text.enabled = m_WatchedInteractionInstigator.enabled && m_WatchedInteractionInstigator.talk_once();
    }
}
