using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LED : MonoBehaviour
{
    public float blinkInterval = 1;
    float strobeInterval;
    float timer;

    public Material blinkColor;
    public Material strobeColor;

    public bool blink;
    public bool strobe;
    public Material Off;
    public Material Green;
    public Material Red;
    public Material White;
    public Material Yellow;
    public Material Amber;
    public Material DefaultColor;

    [SerializeField] string previousState;
    [SerializeField] string currentState;   

    public void setOff()
    {
        previousState = currentState;
        if (currentState != "setOff")
        {
            previousState = currentState;
            currentState = "setOff";
            blink = false;
            strobe = false;
            setColor(Off);
        }
    }

    public void setGreen()
    {
        if (currentState != "setGreen")
        {
            previousState = currentState;
            currentState = "setGreen";
            blink = false;
            strobe = false;
            setColor(Green);
        }
    }

    public void setDefault()
    {
        previousState = currentState;
        if (currentState != "setDefault")
        {
            previousState = currentState;
            currentState = "setDefault";
            blink = false;
            strobe = false;
            setColor(DefaultColor);
        }
    }

    public void setYellow()
    {
        if (currentState != "setYellow")
        {
            previousState = currentState;
            currentState = "setYellow";
            blink = false;
            strobe = false;
            setColor(Yellow);
        }
    }

    public void setAmber(){
        if (currentState != "setAmber")
        {
            previousState = currentState;
            currentState = "setAmber";
            blink = false;
            strobe = false;
            setColor(Amber);
        }
    }

    public void returnToPrevious()
    {
        SendMessage(previousState);
    }
    public void setRed()
    {
        if (currentState != "setRed")
        {
            previousState = currentState;
            currentState = "setRed";
            blink = false;
            strobe = false;
            setColor(Red);
        }
    }

    public void startAmberBlink(float interval=0.3f)
    {
        if (currentState != "startAmberBlink")
        {
            previousState = currentState;
            currentState = "startAmberBlink";
            startBlink(interval, Amber);
        }
    }

    public void startGreenBlink()
    {
        if (currentState != "startGreenBlink")
        {
            previousState = currentState;
            currentState = "startGreenBlink";
            startBlink(0.3f, Green);
        }
    }

    public void startGreenStrobe()
    {
        if (currentState != "startGreenStrobe")
        {
            previousState = currentState;
            currentState = "startGreenStrobe";
            startStrobe(Green);
        }
    }

    void setColor(Material material)
    {        
        this.GetComponent<MeshRenderer>().material = material;        
    }    

    void startBlink(float interval, Material material)
    {
        strobe = false;
        blinkColor = material;
        blinkInterval = interval;
        blink = true;
    }    

    void startStrobe(Material material)
    {
        blink = false;
        strobeColor = material;
        strobe = true;
    }

    void Start()
    {
        setOff();
    }
    // Update is called once per frame
    void Update()
    {        
        if (blink==true)
        {
            timer += Time.deltaTime;
            if (timer > blinkInterval)
            {
                if (this.GetComponent<MeshRenderer>().sharedMaterial == Off)
                {                
                    this.GetComponent<MeshRenderer>().material = blinkColor;
                }
                else
                {                
                    this.GetComponent<MeshRenderer>().material = Off;
                }            
                timer -= blinkInterval;
            }
        }

        if (strobe==true)
        {
            strobeInterval = Random.Range(0f, 1.0f);
            timer += Time.deltaTime;
            if (timer > strobeInterval)
            {
                if (this.GetComponent<MeshRenderer>().sharedMaterial == Off)
                {                
                    this.GetComponent<MeshRenderer>().material = strobeColor;
                }
                else
                {                
                    this.GetComponent<MeshRenderer>().material = Off;
                }            
                timer -= strobeInterval;
            }
        }

    }
}
