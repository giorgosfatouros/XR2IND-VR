using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeChildrenKinematic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (var sibling in transform.GetComponentsInChildren<Transform>())
        {
            if (sibling.TryGetComponent(out Rigidbody rb)) rb.isKinematic = true;
        }
    }


}
