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
    public GameObject battleInfo;
    public Text showAttackDamage;
    public Animator playerAnimator;
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
        attackPopUp.OnAttackDamage += ExecuteAttack;
    }

    public void ExecuteAttack(float actualDamage, int ani)
    {
        StartCoroutine(AttackAnimation(actualDamage,ani));
    }
    IEnumerator AttackAnimation(float dmg, int ani)
    {
        playerAnimator.SetInteger("AttackAction", ani);
        yield return new WaitForSeconds(1f);
        playerAnimator.SetInteger("AttackAction", 0);
        if (dmg == 0)
        {
            battleInfo.SetActive(true);
            showAttackDamage.text = "Miss";
        }
        else
        {
            battleInfo.SetActive(true);
            showAttackDamage.text = "Deal Damage: " + dmg.ToString("F0"); 
        }
        yield return new WaitForSeconds(1f);
        battleInfo.SetActive(false);
        yield return null;
        Debug.Log(ani);
    }

}
