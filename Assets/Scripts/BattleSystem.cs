using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    public GameObject attackPopUpPanel;
    public AttackPopUp attackPopUp;
    public ActionButton actionButton;
    public float actualAttackDamage;
    public Text showAttackDamage;
    public void Start()
    {
        actionButton.OnUseAction += ReceiveActionEnum;
        Debug.Log("Have subscribed OnUseAction:");
    }

    public void ReceiveActionEnum(ActionEnum actionEnum)
    {
        Debug.Log("Receive Action: " + actionEnum);
        attackPopUpPanel.SetActive(true);
        attackPopUp.currentActionEnum = actionEnum;
        attackPopUp.UseAction();
        attackPopUp.OnAttackDamage += ShowDamage;
    }

    public void ShowDamage(float actualDamage)
    {
        showAttackDamage.text = "Deal Damage: " + actualDamage.ToString("F1");
    }

}
