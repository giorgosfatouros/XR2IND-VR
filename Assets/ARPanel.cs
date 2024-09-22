using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Hide()
    {
        this.GetComponent<Canvas>().enabled = false;
    }

    public void Show()
    {
        this.GetComponent<Canvas>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
