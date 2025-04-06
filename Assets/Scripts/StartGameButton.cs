using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public string NextSceneName = "OverworldScene";
    
    public void GameStart()//开始游戏按钮
    {
        SceneManager.LoadScene(NextSceneName,LoadSceneMode.Single);
    }

    public void AppExit()//退出游戏按钮
    {
        //  Application.Quit();
        Application.Quit();
        Debug.Log("ok");
    }
}