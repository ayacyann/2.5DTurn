using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public string NextSceneName = "OverworldScene";
    public void GameStart()//��ʼ��Ϸ��ť
    {
        SceneManager.LoadScene(NextSceneName,LoadSceneMode.Single);
    }

    public void AppExit()//�˳���Ϸ��ť
    {
      //  Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;//�ڲ������˳�
        Debug.Log("ok");
    }
}
