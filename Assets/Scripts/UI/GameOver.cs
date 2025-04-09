using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public BattleSystem battleSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        battleSystem = FindObjectOfType<BattleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (battleSystem.State == BattleSystem.BattleState.Lost)
        {
            if (Input.anyKeyDown)
            {
                //TODO 返回主菜单
                BackToMainMenu();
            }
        }
    }

    private void BackToMainMenu()
    {
        SaveLoadManager.ClearSaveFile();
        // 清除无用的单例
        LoadSceneManager.Instance.BackMenu(() =>
        {
            Destroy(BackpackManager.Instance.gameObject);
            Destroy(PartyManager.Instance.gameObject);
            Destroy(EnemyManager.Instance.gameObject);
        });
    }
}
