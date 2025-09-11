using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    public BattleState battleState;

    public RoleManager roleManager;
    public GameObject playerObject;
    public GameObject enemyObject;
    public Role playerRole;
    public Role enemyRole;

    public GameObject attackPopUpPanel;
    public AttackPopUp attackPopUp;
    public ActionButton actionButton;
    public float actualAttackDamage;
    public UIManager uiManager;
    
    public Animator playerAnimator;
    public AnimationEvent animationEvent;


    void Start()
    {
        battleState = BattleState.START;
        actionButton.OnUseAction += ReceiveActionEnum;
        roleManager.OnBattleWon += BattleWon;
        Debug.Log("Have subscribed OnUseAction:");
        playerObject = roleManager.CreatePlayer();
        playerRole = playerObject.GetComponent<Role>();
        enemyObject = roleManager.CreateEnemy();
        enemyRole = playerObject.GetComponent<Role>();
        StartPlayerTurn();
    }
    public void StartPlayerTurn()
    {
        battleState = BattleState.PLAYERTURN;
        StartCoroutine(uiManager.PlayerTurnUI());
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
        animationEvent = playerObject.GetComponentInChildren<AnimationEvent>();
        playerAnimator = playerObject.GetComponentInChildren<Animator>();
        animationEvent.OnAnimationEnd += ReceiveAnimationEvent;
        playerAnimator.SetInteger("AttackAction", ani);
        actualAttackDamage = actualDamage;
        roleManager.EnemyTakeDamage(actualAttackDamage);
    }

    public void ReceiveAnimationEvent()
    {
        playerAnimator.SetInteger("AttackAction", 0);
        uiManager.actualAttackDamage = actualAttackDamage;
        StartCoroutine(uiManager.BattleInfo());
        animationEvent.OnAnimationEnd -= ReceiveAnimationEvent;
        uiManager.OnUIEnd += StartEnemyTurn;
    }

    public void StartEnemyTurn()
    {
        if(battleState != BattleState.WON)
        {
            battleState = BattleState.ENEMYTURN;
            StartCoroutine(uiManager.EnemyTurnUI());
        }
        attackPopUpPanel.SetActive(true);
        attackPopUp.currentActionEnum = ActionEnum.Attack_02;
        attackPopUp.UseAction();
        attackPopUp.OnAttackDamage += ExecuteAttack;
    }

    public IEnumerator EnemyAction()
    {
        yield return new WaitForSeconds(2f);

    }

    public void BattleWon()
    {
        battleState = BattleState.WON;
        uiManager.Won();
    }

}
