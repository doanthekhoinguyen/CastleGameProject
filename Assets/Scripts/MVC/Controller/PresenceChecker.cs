using UnityEngine;
using MVC.View;

public class PresenceChecker : MonoBehaviour
{
    public BattleHUD battleHUD; 
    private int playerUnitsInArea = 0;
    private int enemyUnitsInArea = 0; 

    void OnTriggerEnter(Collider other)
    {
        // Khi một đơn vị bước vào khu vực
        if (other.CompareTag("Hero"))
        {
            Debug.Log("OnTriggerEnter called with: " + other.name);
            playerUnitsInArea++;
        }
        else if (other.CompareTag("Enemy"))
        {
            enemyUnitsInArea++;
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Hero"))
        {
            Debug.Log("OnTriggerExit called with: " + other.name);
            playerUnitsInArea--;
        }
        else if (other.CompareTag("Enemy"))
        {
            enemyUnitsInArea--;
        }
        
    }

    void Update()
    {
        
        CheckWinLoseCondition();
    }

    public void CheckWinLoseCondition()
    {
        
        if (enemyUnitsInArea == 0 && playerUnitsInArea > 0)
        {
            Debug.Log("Player Wins!");
            battleHUD.ShowGameWin();
            ResetUnitCounts();
        }
        
        else if (playerUnitsInArea == 0 && enemyUnitsInArea > 0)
        {
            Debug.Log("Player Loses!");
            battleHUD.ShowGameLose();
            ResetUnitCounts();
        }
    }

    private void ResetUnitCounts()
    {
        playerUnitsInArea = 0;
        enemyUnitsInArea = 0;
    }
}
