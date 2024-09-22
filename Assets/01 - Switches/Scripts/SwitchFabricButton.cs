using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchFabricButton : MonoBehaviour
{
    [SerializeField] Vector3 MasterLEDPos;
    [SerializeField] Vector3 OnlineLEDPos;
    [SerializeField] Vector3 OfflineLEDPos;
     
    void Awake()
    {
        MasterLEDPos = this.transform.Find("MasterLED").transform.position;
        OnlineLEDPos = this.transform.Find("OnlineLED").transform.position;
        OfflineLEDPos = this.transform.Find("OfflineLED").transform.position;
    }

    void Start()
    {        
    }

    public void FlipSwitch()
    {
        this.transform.Find("MasterLED").transform.position = MasterLEDPos;
        this.transform.Find("OnlineLED").transform.position = OnlineLEDPos;
        this.transform.Find("OfflineLED").transform.position = OfflineLEDPos;
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
