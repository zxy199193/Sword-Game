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
    
    public Animator roleAnimator;
    public AnimationEvent animationEvent;


    void Start()
    {
        battleState = BattleState.START;
        actionButton.OnUseAction += ReceiveActionEnum;
        roleManager.OnBattleWon += BattleWon;
        playerObject = roleManager.CreatePlayer();
        playerRole = playerObject.GetComponent<Role>();
        enemyObject = roleManager.CreateEnemy();
        enemyRole = playerObject.GetComponent<Role>();
        StartPlayerTurn();
    }
    public void StartPlayerTurn()
    {
        battleState = BattleState.PLAYERTURN;
        uiManager.OnUIEnd -= StartPlayerTurn;
        attackPopUp.OnAttackDamage -= ExecuteAttack;
        StartCoroutine(uiManager.PlayerTurnUI());
    }

    public void ReceiveActionEnum(ActionEnum actionEnum)
    {
        attackPopUpPanel.SetActive(true);
        attackPopUp.currentActionEnum = actionEnum;
        attackPopUp.UseAction();
        attackPopUp.OnAttackDamage += ExecuteAttack;
    }

    public void ExecuteAttack(float actualDamage, int ani)
    {
        switch (battleState)
        {
            case BattleState.PLAYERTURN:
                animationEvent = playerObject.GetComponentInChildren<AnimationEvent>();
                roleAnimator = playerObject.GetComponentInChildren<Animator>();
                animationEvent.OnAnimationEnd += ReceiveAnimationEvent;
                roleAnimator.SetInteger("AttackAction", ani);
                actualAttackDamage = actualDamage;
                roleManager.EnemyTakeDamage(actualAttackDamage);
                break;
            case BattleState.ENEMYTURN:
                animationEvent = enemyObject.GetComponentInChildren<AnimationEvent>();
                roleAnimator = enemyObject.GetComponentInChildren<Animator>();
                animationEvent.OnAnimationEnd += ReceiveAnimationEvent;
                roleAnimator.SetInteger("AttackAction", ani);
                actualAttackDamage = actualDamage;
                roleManager.PlayerTakeDamage(actualAttackDamage);
                break;

        }
    }

    public void ReceiveAnimationEvent()
    {
        switch (battleState)
        {
            case BattleState.PLAYERTURN:
                roleAnimator.SetInteger("AttackAction", 0);
                uiManager.actualAttackDamage = actualAttackDamage;
                StartCoroutine(uiManager.BattleInfo());
                animationEvent.OnAnimationEnd -= ReceiveAnimationEvent;
                uiManager.OnUIEnd += StartEnemyTurn;
                break;
            case BattleState.ENEMYTURN:
                roleAnimator.SetInteger("AttackAction", 0);
                uiManager.actualAttackDamage = actualAttackDamage;
                StartCoroutine(uiManager.BattleInfo());
                animationEvent.OnAnimationEnd -= ReceiveAnimationEvent;
                uiManager.OnUIEnd += StartPlayerTurn;
                break;
        }
    }

    public void StartEnemyTurn()
    {
        uiManager.OnUIEnd -= StartEnemyTurn;
        attackPopUp.OnAttackDamage -= ExecuteAttack;
        if (battleState != BattleState.WON)
        {
            battleState = BattleState.ENEMYTURN;
            StartCoroutine(uiManager.EnemyTurnUI());
        }
        attackPopUpPanel.SetActive(true);
        attackPopUp.currentActionEnum = ActionEnum.Attack_02;
        attackPopUp.UseAction();
        attackPopUp.OnAttackDamage += ExecuteAttack;
        StartCoroutine(attackPopUp.OnAIButton());
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
    public void BattleLost()
    {
        battleState = BattleState.LOST;
        uiManager.Lost();
    }

}
