using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleInfoPanel : MonoBehaviour
{

    public Role role;
    public string roleName;
    public float roleHP;
    public float roleHPCurrent;

    public Text roleNameText;
    public Text roleHPText;
    public Slider roleHPSlider;

    public void SetRoleInfo()
    {
        roleName = role.roleName;
        roleHP = role.roleHP;
        roleHPCurrent = roleHP;
        roleNameText.text = roleName;
        HPInfo();
    }
    
    public void RoleInfoUpdate(float hpCurrent)
    {
        roleHPCurrent = hpCurrent;
        roleNameText.text = roleName;
        HPInfo();
    }
    public void HPInfo()
    {
        roleHPText.text = roleHPCurrent.ToString("F0") + "/" + roleHP.ToString("F0");
        roleHPSlider.value = roleHPCurrent / roleHP;
    }


}
