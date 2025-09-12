using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Text battleInfoText;
    public float actualAttackDamage;
    public GameObject battleInfo;
    public GameObject actionButtonPanel;
    public GameObject actionList;

    public event System.Action OnUIEnd;
    public IEnumerator BattleInfoAttack()
    {
        yield return new WaitForSeconds(0.5f);
        if (actualAttackDamage == 0)
        {
            battleInfo.SetActive(true);
            battleInfoText.text = "Miss";
        }
        else
        {
            battleInfo.SetActive(true);
            battleInfoText.text = "Deal Damage: " + actualAttackDamage.ToString("F0");
        }
        yield return new WaitForSeconds(1.5f);
        battleInfo.SetActive(false);
        yield return new WaitForSeconds(1f);
        OnUIEnd?.Invoke();
        yield return null;
    }
    public IEnumerator BattleInfoDefend()
    {
        battleInfo.SetActive(true);
        battleInfoText.text = "Defend!";
        yield return new WaitForSeconds(1f);
        battleInfo.SetActive(false);
    }

    public IEnumerator PlayerTurnUI()
    {
        battleInfoText.text = "Battle Start!";
        battleInfo.SetActive(true);
        yield return new WaitForSeconds(1f);
        actionButtonPanel.SetActive(true);
        actionList.SetActive(true);
        battleInfo.SetActive(false);
        yield return null;
    }
    public IEnumerator EnemyTurnUI()
    {
        battleInfoText.text = "Enemy Turn!";
        battleInfo.SetActive(true);
        actionButtonPanel.SetActive(false);
        actionList.SetActive(false);
        yield return new WaitForSeconds(1f);
        battleInfo.SetActive(false);
        yield return null;
    }

    public void Won()
    {
        battleInfoText.text = "You Win!!";
        battleInfo.SetActive(true);
    }
    public void Lost()
    {
        battleInfoText.text = "You Lose!!";
        battleInfo.SetActive(true);
    }
}
