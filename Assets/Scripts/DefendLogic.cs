using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendLogic : MonoBehaviour
{
    public float defendValue;
    public ActionEnum currentActionEnum;
    public int actionAni;
    public ActionSelection actionSelection;
    public ActionData currentActionData;
    public event System.Action<float, int> OnDefend;

    public void UseAction()
    {
        foreach (ActionData action in actionSelection.actionList)
        {
            if (action.actionEnum == currentActionEnum)
            {
                currentActionData = action;
                break;
            }
        }
        defendValue = currentActionData.defendValue;
        actionAni = currentActionData.actionAni;
        OnDefend?.Invoke(defendValue, actionAni);
    }

}
