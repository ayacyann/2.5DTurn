using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance{get; private set;}
    public Image fadePanel;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        fadePanel = transform.GetComponentInChildren<Image>();
    }

    public void StartGame()
    {
        //清空存档
        SaveLoadManager.ClearSaveFile();
        LoadScene(ConfigString.OVERWORLD_SCENE);
        Debug.Log("开始游戏");
    }

    public void BackMenu(Action callback = null)
    {
        //TODO 保存数据
        LoadScene(ConfigString.MENU_SCENE,callback);
        Debug.Log("返回菜单");
    }

    public void QuitGame()
    {
        // EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void ContinueGame()
    {
        //TODO 加载数据
        LoadScene(ConfigString.OVERWORLD_SCENE);
    }

    public void LoadBattleScene()
    {
        LoadScene(ConfigString.BATTLE_SCENE);
    }

    public void LoadScene(string sceneName, Action callback = null,bool isFade = true)
    {
        if (!isFade)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            return;
        }
        StartCoroutine(TransitionRoutine(sceneName,callback));
    }

    private IEnumerator TransitionRoutine(string sceneName,Action callback = null)
    {
        // 淡出效果
        fadePanel.raycastTarget = true; // 阻止点击穿透
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadePanel.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        fadePanel.color = new Color(0f, 0f, 0f, 1f);

        // 异步加载场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // 等待加载进度达到90%（Unity的异步加载特性）
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;

        // 等待场景加载完成
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        // 淡入效果
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadePanel.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        fadePanel.color = new Color(0f, 0f, 0f, 0f);
        fadePanel.raycastTarget = false; // 恢复点击
        callback?.Invoke();
    }
}