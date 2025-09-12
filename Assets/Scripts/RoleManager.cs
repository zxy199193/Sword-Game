using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleManager : MonoBehaviour
{
    public Role playerRole;
    public Role enemyRole;
    public GameObject rolePrefab;
    public GameObject playerRoleObject;
    public GameObject enemyRoleObject;
    public Transform playerPos;
    public Transform enemyPos;
    public RoleData playerRoleData;
    public RoleData enemyRoleData;

    public float playerDefend = 0;
    public float enemyDefend = 0;

    public RoleInfoPanel playerInfoPanel;
    public RoleInfoPanel enemyInfoPanel;

    public event System.Action OnBattleWon;
    public event System.Action OnBattleLost;


    public GameObject CreatePlayer()
    {
        GameObject playerGO = Instantiate(rolePrefab, playerPos);
        playerRole = playerGO.GetComponent<Role>();
        playerRole.roleData = playerRoleData;
        playerRole.AddRoleData();
        playerInfoPanel.role = playerRole;
        playerInfoPanel.SetRoleInfo();
        playerRoleObject = playerGO;
        return playerRoleObject;
    }


    public GameObject CreateEnemy()
    {
        GameObject enemyGO = Instantiate(rolePrefab, enemyPos);
        enemyRole = enemyGO.GetComponent<Role>();
        enemyRole.roleData = enemyRoleData;
        enemyRole.AddRoleData();
        enemyInfoPanel.role = enemyRole;
        enemyInfoPanel.SetRoleInfo();
        enemyRoleObject = enemyGO;
        return enemyRoleObject;
    }

    public void EnemyTakeDamage(float damage)
    {
        if (damage > enemyDefend)
        {
            enemyRole.roleHPCurrent -= damage - enemyDefend;
        }
        else { enemyRole.roleHPCurrent -= 0; }
        if (enemyRole.roleHPCurrent <= 0)
        {
            enemyRole.roleHPCurrent = 0;
            OnBattleWon?.Invoke();
        }
        enemyInfoPanel.RoleInfoUpdate(enemyRole.roleHPCurrent);
        enemyDefend = 0;
    }
    public void PlayerTakeDamage(float damage)
    {
        if (damage > playerDefend)
        {
           playerRole.roleHPCurrent -= damage - playerDefend;
        }
        else { playerRole.roleHPCurrent -= 0; }

        if (playerRole.roleHPCurrent <= 0)
        {
            playerRole.roleHPCurrent = 0;
            OnBattleLost?.Invoke();
        }
        playerInfoPanel.RoleInfoUpdate(playerRole.roleHPCurrent);
        playerDefend = 0;
    }
    public void PlayerDefend(float defendValue)
    {
        playerDefend = defendValue;
    }
    public void EnemyDefend(float defendValue)
    {
        enemyDefend = defendValue;
    }
}
