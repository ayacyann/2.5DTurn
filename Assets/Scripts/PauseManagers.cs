using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public string mainMenuSceneName = "OverworldScene";

    [SerializeField] private GameObject pauseUI; // ����PauseUI������
    private bool isPaused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            // ��ʼ��ʱǿ������
            if (pauseUI != null)
            {
                pauseUI.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }


    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ÿ�μ��س�����ǿ�ƹر���ͣ״̬
        //ForceResume();
        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
        }
    }

    void Update()
    {
        //Debug.Log((SceneManager.GetActiveScene().name == mainMenuSceneName));
       // Debug.Log("����:" + Input.GetKeyDown(KeyCode. R));
        //����ǰ�Ļ����Ϊxxx���Ұ���ESC��ť
        if (SceneManager.GetActiveScene().name != mainMenuSceneName && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pauseUI.SetActive(isPaused); // ����״̬��ʾ/����
    }

    // ������������ԭ��...

    public void ResumeGame()
    {
        TogglePause();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1; // �ָ�ʱ��
        TogglePause();
        SceneManager.LoadSceneAsync("MenuScene"); // �滻Ϊ������˵�������
        
    }
}
