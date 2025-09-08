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
    public int attackDamage;
    public float attackSliderSpeed;
    public float attackSliderDecay;
    public int attackHitSection_Lv1;
    public int attackHitSection_Lv2;
    public int attackHitSection_Lv3;
    public int attackHitSection_Lv4;
    public int attackHitSection_Lv5;
    public int attackAni;
    
    public int defendValue;
    public int dodgeRate;
    public int itemValue;

}
