using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ActionEnum
{
    Attack_01,
    Attack_02,
    Defend_01,
    Defend_02,
    Dodge_01,
    Dodge_02,
    Item_01,
    Item_02
}
public enum ActionType
{
    Attack,
    Defend,
    Dodge,
    Item
}

[CreateAssetMenu(fileName = "ActionData", menuName = "Game/Action")]

public class ActionData : ScriptableObject
{
    public ActionEnum actionEnum;
    public string actionName;
    public ActionType actionType;
    public Sprite actionIcon;
    [TextArea] public string actionDes;
    public int actionValue_01;
    public int actionValue_02;
}
