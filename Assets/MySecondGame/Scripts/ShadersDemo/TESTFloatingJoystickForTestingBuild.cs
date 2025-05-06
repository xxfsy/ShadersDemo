using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TESTFloatingJoystickForTestingBuild : Joystick //TEST
{
    protected override void Start()//TEST
    {
        base.Start();//TEST
        background.gameObject.SetActive(false);//TEST
    }

    public override void OnPointerDown(PointerEventData eventData)//TEST
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);//TEST
        background.gameObject.SetActive(true);//TEST
        base.OnPointerDown(eventData);//TEST
    }

    public override void OnPointerUp(PointerEventData eventData)//TEST
    {
        background.gameObject.SetActive(false);//TEST
        base.OnPointerUp(eventData);//TEST
    }
}
