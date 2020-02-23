using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class OnButtonHold : UnityEvent{}

public class HoldButton : MonoBehaviour
{
    public double holdTime = 0.5;
    private double time = 0;

    private bool holding = false;

    public OnButtonHold holdEvent;

    // Start is called before the first frame update

    // Update is called once per frame

    public void beginHold()
    {
        time = 0.0;
        holding = true;
    }

    public void endHold()
    {
        holding = false;
        time = 0.0;
    }

    void Update()
    {
        if (holding)
        {
            time += Time.deltaTime;
        }
        if (holding && time >= holdTime)
        {
            holdEvent.Invoke();
            time = 0.0;
            holding = false;
        }
    }
}
