using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetXRRigHeight : MonoBehaviour
{
    Vector3 newpos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        newpos = this.transform.position;
        newpos.y = 0.0f;
        this.transform.position = newpos;
    }
}
