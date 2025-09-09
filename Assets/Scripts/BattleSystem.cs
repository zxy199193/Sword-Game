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
    public AnimationEvent animationEvent;
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
        animationEvent.OnAnimationEnd += ReceiveAnimationEvent;
        playerAnimator.SetInteger("AttackAction", ani);
        actualAttackDamage = actualDamage;
    }

    public void ReceiveAnimationEvent()
    {
        playerAnimator.SetInteger("AttackAction", 0);
        StartCoroutine(AttackAnimation());
        animationEvent.OnAnimationEnd -= ReceiveAnimationEvent;
    }

    IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        if (actualAttackDamage == 0)
        {
            battleInfo.SetActive(true);
            showAttackDamage.text = "Miss";
        }
        else
        {
            battleInfo.SetActive(true);
            showAttackDamage.text = "Deal Damage: " + actualAttackDamage.ToString("F0"); 
        }
        yield return new WaitForSeconds(1.5f);
        battleInfo.SetActive(false);
        yield return null;
    }

}
