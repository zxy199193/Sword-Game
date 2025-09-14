using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    public BattleState battleState;
    public BattleState tempState;

    public RoleManager roleManager;
    public GameObject playerObject;
    public GameObject enemyObject;
    public Role playerRole;
    public Role enemyRole;

    public GameObject attackPopUpPanel;
    public AttackLogic attackLogic;
    public ActionButton actionButton;
    public float actualAttackDamage;
    public UIManager uiManager;

    public DefendLogic defendLogic;
    public float defendValue;

    public Animator playerAnimator;
    public Animator enemyAnimator;
    public AnimationEvent playerAnimationEvent;
    public AnimationEvent enemyAnimationEvent;


    void Start()
    {
        battleState = BattleState.START;
        actionButton.OnUseAction += ReceiveActionButton;
        roleManager.OnBattleWon += BattleWon;
        roleManager.OnBattleLost += BattleLost;
        defendLogic.OnDefend -= ExecuteDefend;
        playerObject = roleManager.CreatePlayer();
        playerRole = playerObject.GetComponent<Role>();
        playerAnimationEvent = playerObject.GetComponentInChildren<AnimationEvent>();
        playerAnimator = playerObject.GetComponentInChildren<Animator>();
        enemyObject = roleManager.CreateEnemy();
        enemyRole = playerObject.GetComponent<Role>();
        enemyAnimationEvent = enemyObject.GetComponentInChildren<AnimationEvent>();
        enemyAnimator = enemyObject.GetComponentInChildren<Animator>();
        StartPlayerTurn();
    }
    public void StartPlayerTurn()
    {
        battleState = BattleState.PLAYERTURN;
        uiManager.OnUIEnd -= StartPlayerTurn;
        attackLogic.OnAttackDamage -= ExecuteAttack;
        uiManager.OnUIEnd -= CheckGameState;
        playerAnimator.SetInteger("ActionAniNum", 0);
        StartCoroutine(uiManager.PlayerTurnUI());
    }

    public void ReceiveActionButton(ActionEnum actionEnum, ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.Attack:
                attackPopUpPanel.SetActive(true);
                attackLogic.currentActionEnum = actionEnum;
                attackLogic.UseAction();
                attackLogic.OnAttackDamage += ExecuteAttack;
                break;
            case ActionType.Defend:
                defendLogic.currentActionEnum = actionEnum;
                defendLogic.OnDefend += ExecuteDefend;
                defendLogic.UseAction();
                break;
            case ActionType.Dodge:
                break;
            case ActionType.Item:
                break;
        }
    }

    public void ExecuteAttack(float actualDamage, int ani)
    {
        switch (battleState)
        {
            case BattleState.PLAYERTURN:
                playerAnimationEvent.OnAnimationEnd += ReceiveAnimationEvent;
                playerAnimator.SetInteger("ActionAniNum", ani);
                actualAttackDamage = actualDamage;
                roleManager.EnemyTakeDamage(actualAttackDamage);
                break;
            case BattleState.ENEMYTURN:
                enemyAnimationEvent.OnAnimationEnd += ReceiveAnimationEvent;
                enemyAnimator.SetInteger("ActionAniNum", ani);
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
                playerAnimator.SetInteger("ActionAniNum", 0);
                uiManager.actualAttackDamage = actualAttackDamage;
                StartCoroutine(uiManager.BattleInfoAttack());
                playerAnimationEvent.OnAnimationEnd -= ReceiveAnimationEvent;
                uiManager.OnUIEnd += CheckGameState;
                break;
            case BattleState.ENEMYTURN:
                enemyAnimator.SetInteger("ActionAniNum", 0);
                uiManager.actualAttackDamage = actualAttackDamage;
                StartCoroutine(uiManager.BattleInfoAttack());
                enemyAnimationEvent.OnAnimationEnd -= ReceiveAnimationEvent;
                uiManager.OnUIEnd += CheckGameState;
                break;
        }
    }
    public void ExecuteDefend(float getDefendValue, int ani)
    {
        switch (battleState)
        {
            case BattleState.PLAYERTURN:
                playerAnimator.SetInteger("ActionAniNum", ani);
                defendValue = getDefendValue;
                roleManager.PlayerDefend(defendValue);
                StartCoroutine(uiManager.BattleInfoDefend());
                StartEnemyTurn();
                break;
            case BattleState.ENEMYTURN:
                enemyAnimator.SetInteger("ActionAniNum", ani);
                defendValue = getDefendValue;
                roleManager.EnemyDefend(defendValue);
                StartCoroutine(uiManager.BattleInfoDefend());
                StartPlayerTurn();
                break;
        }
    }

    public void StartEnemyTurn()
    {
        uiManager.OnUIEnd -= StartEnemyTurn;
        attackLogic.OnAttackDamage -= ExecuteAttack;
        uiManager.OnUIEnd -= CheckGameState;
        defendLogic.OnDefend -= ExecuteDefend;
        enemyAnimator.SetInteger("ActionAniNum", 0);
        if (battleState != BattleState.WON)
        {
            battleState = BattleState.ENEMYTURN;
            StartCoroutine(uiManager.EnemyTurnUI());
        }
        attackPopUpPanel.SetActive(true);
        attackLogic.currentActionEnum = ActionEnum.Attack_02;
        attackLogic.UseAction();
        attackLogic.OnAttackDamage += ExecuteAttack;
        StartCoroutine(attackLogic.OnAIButton());
    }

    public IEnumerator EnemyAction()
    {
        yield return new WaitForSeconds(2f);

    }

    public void BattleWon()
    {
        tempState = BattleState.WON;
    }
    public void BattleLost()
    {
        tempState = BattleState.LOST;
    }
    public void CheckGameState()
    {
        if (tempState == BattleState.WON)
        {
            battleState = BattleState.WON;
            uiManager.Won();
        }
        else if (tempState == BattleState.LOST)
        {
            battleState = BattleState.LOST;
            uiManager.Lost();
        }
        else
        {
            switch (battleState)
            {
                case BattleState.PLAYERTURN:
                    StartEnemyTurn();
                    break;
                case BattleState.ENEMYTURN:
                    StartPlayerTurn();
                    break;
            }
        }
    }
}
