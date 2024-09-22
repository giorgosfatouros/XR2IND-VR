using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objectives : MonoBehaviour
{
    // Start is called before the first frame update
    // [SerializeField] GameObject Router;
    [SerializeField] Sprite CheckMark;
    [SerializeField] Sprite XMark;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Router.GetComponent<Router>().turnedOn == true)
        // {
        //     this.transform.Find("Canvas/CheckMark_01").GetComponent<Image>().sprite = CheckMark;
        // } 
        // else
        // {
        //     this.transform.Find("Canvas/CheckMark_01").GetComponent<Image>().sprite = XMark;
        // }

        // if (Router.GetComponent<Router>().connectedToNetwork == true)
        // {
        //     this.transform.Find("Canvas/CheckMark_02").GetComponent<Image>().sprite = CheckMark;
        // } 
        // else
        // {
        //     this.transform.Find("Canvas/CheckMark_02").GetComponent<Image>().sprite = XMark;
        // }

        // if (Router.GetComponent<Router>().completeConnection == true)
        // {
        //     this.transform.Find("Canvas/CheckMark_03").GetComponent<Image>().sprite = CheckMark;
        // } 
        // else
        // {
        //     this.transform.Find("Canvas/CheckMark_03").GetComponent<Image>().sprite = XMark;
        // }
        
    }
}
