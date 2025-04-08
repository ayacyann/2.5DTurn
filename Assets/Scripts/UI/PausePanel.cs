using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI; // 拖入PauseUI父物体
    private bool isPaused = false;

    void Update()
    {
        //Debug.Log((SceneManager.GetActiveScene().name == mainMenuSceneName));
        // Debug.Log("按键:" + Input.GetKeyDown(KeyCode. R));
        //若当前的活动场景为xxx并且按下ESC按钮
        if (SceneManager.GetActiveScene().name != ConfigString.MENUSCENE && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pauseUI.SetActive(isPaused); // 根据状态显示/隐藏
    }

    // 其他方法保持原样...

    public void ResumeGame()
    {
        TogglePause();
    }

    public void BackMenu()
    {
        Time.timeScale = 1; // 恢复时间
        TogglePause();
        BackpackManager.Instance.SaveBackpackItem();
        PartyManager.Instance.SavePlayerParty();
        CharacterManager.Instance.SavePlayerPosition();
        // 清除无用的单例
        LoadSceneManager.Instance.BackMenu(() =>
        {
            Destroy(BackpackManager.Instance.gameObject);
            Destroy(PartyManager.Instance.gameObject);
            Destroy(EnemyManager.Instance.gameObject);
        });
    }
}