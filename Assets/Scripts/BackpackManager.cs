using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum DropItemType
{
    BloodVial,
    GreenVase,
    Heart,
    Star
}

public class BackpackManager : MonoBehaviour
{
    public static BackpackManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BackpackManager>();
            }
            return _instance;
        }
    }
    private static BackpackManager _instance;
    //public string mainMenuSceneName = "OverworldScene";
    public bool isShowCanvas = false;
    public ItemInfo itemInfo;
    public List<Item> items = new List<Item>();
    public int maxItemCount = 16;

    [Header("UI")]
    [SerializeField]private Transform itemContent;
    [SerializeField] private GameObject pauseUI; // 拖入PauseUI父物体
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

    // Awake 方法，确保单例在场景加载时初始化
    private void Awake()
    {
        // 如果实例已存在且不是当前对象，销毁当前对象
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // 设置实例为当前对象
        _instance = this;
        Debug.Log(Instance);
        // 确保单例在场景切换时不被销毁
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        //Debug.Log("按键:"+Input.GetKeyDown(KeyCode.R));
        //若当前的活动场景为xxx并且按下ESC按钮
        if (SceneManager.GetActiveScene().name == "OverworldScene" && Input.GetKeyDown(KeyCode.R))
        {
            ToggleShowPannel();
        }
    }

    public void AddItem(Item item)
    {
        if (items.Count >= maxItemCount)
        {
            Debug.Log("背包已满");
            return;
        }
        items.Add(item);
    }

    public void UpdatePannel()
    {
        int childCount = itemContent.childCount;
        int itemCount = items.Count;
        for (int i = 0; i < childCount; i++)
        {
            Image image = itemContent.GetChild(i).GetChild(0).GetComponent<Image>();
            if (i < itemCount)
            {
                image.sprite = items[i].sprite;
            }
            else
            {
                image.sprite = null;
            }
        }
    }

    public void ToggleShowPannel()
    {
        isShowCanvas = !isShowCanvas;
        Time.timeScale = isShowCanvas ? 0 : 1;
        pauseUI.SetActive(isShowCanvas); // 根据状态显示/隐藏
        if (isShowCanvas)
        {
            Debug.Log(1);
            UpdatePannel();
        }
        // Debug.Log("名字:"+CurrentParty[0].MemberName+"等级:"+CurrentParty[0].Level+"当前体力:"+ CurrentParty[0].CurrHealth+ "最大体力:" + CurrentParty[0].MaxHealth);
        // CurrentParty[0].Level += 1;

        DataUpdate(PartyManager.Instance.GetAliveParty());
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
}