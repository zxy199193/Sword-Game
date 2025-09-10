using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoleEnum
{
    Player_01,
    Player_02,
    Enemy_01,
    Enemy_02
}

[CreateAssetMenu(fileName = "RoleData", menuName = "Game/Role")]

public class RoleData : ScriptableObject
{
    public RoleEnum roleEnum;
    public string roleName;
    public float roleHP;
}
