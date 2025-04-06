using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackPack : MonoBehaviour
{
    public static BackPack  Instance;
    //public string mainMenuSceneName = "OverworldScene";
    private bool isPaused = false;
    private PartyManager partyManager;

 

    [Header("UI")]
    [SerializeField] private GameObject pauseUI; // ����PauseUI������
    [SerializeField] private GameObject itemquantity;
    [SerializeField] private TextMeshProUGUI Propeffect;
    [SerializeField] private TextMeshProUGUI CurrentQuantity;
    [SerializeField] private TextMeshProUGUI MaxQuantity;

    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Level;
    [SerializeField] private TextMeshProUGUI CurrrentHealth;
    [SerializeField] private TextMeshProUGUI MaxHealth;
    [SerializeField] private TextMeshProUGUI Atk;
    [SerializeField] private TextMeshProUGUI Spe;




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
        partyManager = GameObject.FindAnyObjectByType<PartyManager>();

        BackpackTable backpacktable = Resources.Load<BackpackTable>("DataTable/packageTable");


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
        
        //Debug.Log("����:"+Input.GetKeyDown(KeyCode.R));
        //����ǰ�Ļ����Ϊxxx���Ұ���ESC��ť


        if (SceneManager.GetActiveScene().name == "OverworldScene" && Input.GetKeyDown(KeyCode.R))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pauseUI.SetActive(isPaused); // ����״̬��ʾ/����
        List<PartyMember> CurrentParty = new List<PartyMember>();
        CurrentParty = partyManager.GetAliveParty();


        // Debug.Log("����:"+CurrentParty[0].MemberName+"�ȼ�:"+CurrentParty[0].Level+"��ǰ����:"+ CurrentParty[0].CurrHealth+ "�������:" + CurrentParty[0].MaxHealth);
        // CurrentParty[0].Level += 1;

        DataUpdate(CurrentParty);
        CurrentQuantity.text = "2";
        
        /*
        BackpackTable backpacktable = Resources.Load<BackpackTable>("DataTable/packageTable");
        PackageTableItem pt = new PackageTableItem();
         pt.id = 3;
         backpacktable.DataList.Add(pt);
        
        foreach (PackageTableItem packageItem in backpacktable.DataList)
        {
            Debug.Log(string.Format("id:{0}, name:{1}", packageItem.id, packageItem.name));
        }
*/
       

        
    }
    public void DataUpdate(List<PartyMember> CurrentParty)
    {
          Name.text = CurrentParty[0].MemberName;
          Level.text = ""+CurrentParty[0].Level;
        CurrrentHealth.text = "" + CurrentParty[0].CurrHealth;
        MaxHealth.text = "" + CurrentParty[0].MaxHealth;
        Spe.text = "" + CurrentParty[0].Initiative;
        Atk.text = "" + CurrentParty[0].Strength;
    }

    // ������������ԭ��...

    public void ResumeGame()
    {
        TogglePause();
    }
}
