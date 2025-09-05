using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelection : MonoBehaviour
{
    public GameObject actionButtonPrefab;
    public Transform actionButtionArea;
    public BattleSystem battleSystem;

    public List<ActionData> actionList;
    public List<ActionData> actionListAttack;
    public List<ActionData> actionListDefend;
    public List<ActionData> actionListDodge;
    public List<ActionData> actionListItem;
    
    void Start()
    {
        ActionCategory();
    }
    public void ActionCategory()
    {
        foreach (ActionData action in actionList)
        {
            switch(action.actionType)
            {
                case ActionType.Attack:
                    actionListAttack.Add(action);
                    break;
                case ActionType.Defend:
                    actionListDefend.Add(action);
                    break;
                case ActionType.Dodge:
                    actionListDodge.Add(action);
                    break;
                case ActionType.Item:
                    actionListItem.Add(action);
                    break;
                default:
                    break;
                }
        }
    }

    public void InstantiateActionButtons(List<ActionData> actionList)
    {
        foreach (Transform child in actionButtionArea)
        {
            Destroy(child.gameObject);
        }
        foreach (ActionData action in actionList)
        {
            GameObject actionButton = Instantiate(actionButtonPrefab, actionButtionArea);
            ActionButton buttonInfo = actionButton.GetComponent<ActionButton>();
            buttonInfo.actionEnum = action.actionEnum;
            buttonInfo.actionName.text = action.actionName;
            buttonInfo.actionType.text = action.actionType.ToString();
            buttonInfo.actionDes.text = action.actionDes;
            buttonInfo.actionIcon.sprite = action.actionIcon;
            buttonInfo.OnUseAction += battleSystem.ReceiveActionEnum;
        }
    }

    public void ShowActionListAttack()
    {
        InstantiateActionButtons(actionListAttack); 
    }
    public void ShowActionListDefend()
    {
        InstantiateActionButtons(actionListDefend);
    }
    public void ShowActionListDodge()
    {
        InstantiateActionButtons(actionListDodge);
    }
    public void ShowActionListItem()
    {
        InstantiateActionButtons(actionListItem);
    }

}
