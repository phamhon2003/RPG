using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EvenInputCategory
{
    MouseDownLeft, MouseDownRight, MouseUpLeft, MouseHoldLeft
}
public class EventAction
{
    public event Action eventAction;
    public void RunAction()
    {
        eventAction?.Invoke();
    }
}
public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public bool doNotInteractwithUI;
    public Vector2 MousePosition;
    private new Camera camera;
    private Dictionary<EvenInputCategory, EventAction> EventInputDic = new Dictionary<EvenInputCategory, EventAction>();
    private bool holdingMouseLeft;
    private void Awake()
    {
        camera = Camera.main;
        AddDictionaryEvent();
        instance = this;
    }
    private void AddDictionaryEvent()
    {
        EventInputDic.Add(EvenInputCategory.MouseDownLeft, new EventAction());
        EventInputDic.Add(EvenInputCategory.MouseDownRight, new EventAction());
        EventInputDic.Add(EvenInputCategory.MouseUpLeft, new EventAction());      
        EventInputDic.Add(EvenInputCategory.MouseHoldLeft, new EventAction());    
    }
    private Vector3 ConvertScreenToWorldPoint(Vector3 position)
    {
        return camera.ScreenToWorldPoint(position);
    }
    public void subEventInput(EvenInputCategory EventInput, Action action)
    {
        EventInputDic[EventInput].eventAction += action;
    }
    public void RemoveEventInput(EvenInputCategory EventInput, Action action)
    {
        EventInputDic[EventInput].eventAction -= action;
    }
    private void Update()
    {
        if (doNotInteractwithUI) if (EventSystem.current.IsPointerOverGameObject()) return;
        MousePosition = ConvertScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)) 
        {
            EventInputDic[EvenInputCategory.MouseDownLeft].RunAction(); 
            holdingMouseLeft = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            EventInputDic[EvenInputCategory.MouseDownRight].RunAction();
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            EventInputDic[EvenInputCategory.MouseUpLeft].RunAction();
            holdingMouseLeft = false;
        }       
    }
    private void LateUpdate()
    {
        if (holdingMouseLeft) EventInputDic[EvenInputCategory.MouseHoldLeft].RunAction();
        //if (holdingMouseRight) EventInputDic[EvenInputCategory.MouseHoldRight].RunAction();
    }
}
