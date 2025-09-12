using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    public ActionEnum actionEnum;
    public ActionType actionType;
    public Text actionNameText;
    public Text actionTypeText;
    public Text actionDesText;
    public Image actionIcon;
    public int actionValue_01;
    public int actionValue_02;
    public event System.Action<ActionEnum,ActionType> OnUseAction;

    public void UseAction()
    {
        OnUseAction?.Invoke(actionEnum,actionType);
        Debug.Log("Use Action: " + actionEnum);
    }

}
