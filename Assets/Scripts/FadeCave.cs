using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeCave : MonoBehaviour
{
    public static FadeCave Instance;

    public Image fadePanel;
    public float fadeDuration = 1f;

    public  string sceneName = "OverworldScene";

    void Awake()
    {
        if (Instance != null)
        {
            //��ɾ��֮ǰ�ģ������¸�ֵ����Ҫɾ���Լ�
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //���ز˵�֮��Ϊ�գ����Լ�ɾ���ˣ����¿�����
    }

    public void StartTransition()
    {
        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator TransitionRoutine(string sceneName)
    {
        // ����Ч��
        fadePanel.raycastTarget = true; // ��ֹ�����͸
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadePanel.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        fadePanel.color = new Color(0f, 0f, 0f, 1f);

        // �첽���س���
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // �ȴ����ؽ��ȴﵽ90%��Unity���첽�������ԣ�
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;

        // �ȴ������������
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // ����Ч��
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadePanel.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        Debug.Log("����");
        fadePanel.color = new Color(0f, 0f, 0f, 0f);
        fadePanel.raycastTarget = false; // �ָ����
    }
}



