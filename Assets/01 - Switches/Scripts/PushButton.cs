using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour
{
    [SerializeField] Vector3 offPosition;
    [SerializeField] Vector3 onPosition;    
    public bool statusOn = false;
    // Start is called before the first frame update

    public void FlipSwitch()
    {
        if (statusOn == true) TurnOff();
        else TurnOn();        
    }

    public void TurnOff(){
        statusOn = false;
        this.transform.localPosition = offPosition;
    }

    public void TurnOn(){
        statusOn = true;
        this.transform.localPosition = onPosition;
    }

  
}



