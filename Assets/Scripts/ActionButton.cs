using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    public ActionEnum actionEnum;
    public Text actionName;
    public Text actionType;
    public Text actionDes;
    public Image actionIcon;
    public int actionValue_01;
    public int actionValue_02;
    public event System.Action<ActionEnum> OnUseAction;

    public void UseAction()
    {
        OnUseAction?.Invoke(actionEnum);
        Debug.Log("Use Action: " + actionEnum);
    }

}
