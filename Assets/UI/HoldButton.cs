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

    public UnityEngine.UI.Image holdProgressImage;

    // Start is called before the first frame update

    // Update is called once per frame

    public void beginHold()
    {
        AudioController.GetSource("/Audio/HoldButton").Play();

        time = 0.0;
        holding = true;

        holdProgressImage.gameObject.SetActive(true);
        holdProgressImage.fillAmount = 0.0f;
    }

    public void endHold()
    {
        holding = false;
        time = 0.0;

        holdProgressImage.fillAmount = 1.0f;
        holdProgressImage.gameObject.SetActive(false);

        StartCoroutine(AudioController.FadeOut(AudioController.GetSource("/Audio/HoldButton"), 0.01F));
    }

    void Update()
    {
        if (holding)
        {
            time += Time.deltaTime;
            // if (Input.touchCount > 0)
            // {
            //     Touch touch = Input.GetTouch(0);
            //     holdProgressImage.rectTransform.anchoredPosition = touch.position;
            // }
            // else if (Input.GetMouseButtonDown(0))
            // {
            //     holdProgressImage.rectTransform.anchoredPosition = Input.mousePosition;
            // }
                holdProgressImage.fillAmount = (float)(time / holdTime);
        }
        if (holding && time >= holdTime)
        {
            holdEvent.Invoke();
            time = 0.0;
            holding = false;
            holdProgressImage.gameObject.SetActive(false);
        }
    }
}
