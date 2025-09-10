using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{
    public string roleName;
    public float roleHP;
    //public int roleEnergy;
    //public int roleStrength;
    //public float roleAgility;
    //public float roleArmour;

    public float roleHPCurrent;

    public RoleData roleData;

    public void AddRoleData()
    {
        roleName = roleData.roleName;
        roleHP = roleData.roleHP;
        roleHPCurrent = roleHP;
    }

}
