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
        enemyRole.roleHPCurrent -= damage;
        enemyInfoPanel.RoleInfoUpdate(enemyRole.roleHPCurrent);
        if(enemyRole.roleHPCurrent<=0)
        {
            OnBattleWon?.Invoke();
        }
    }
    public void PlayerTakeDamage(float damage)
    {
        playerRole.roleHPCurrent -= damage;
        playerInfoPanel.RoleInfoUpdate(playerRole.roleHPCurrent);
        if (playerRole.roleHPCurrent <= 0)
        {
            OnBattleLost?.Invoke();
        }
    }
}
