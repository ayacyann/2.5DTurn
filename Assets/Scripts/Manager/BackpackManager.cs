using System;
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

    public ItemUse currentItem;
    public Transform showItemInfo;
    public Transform buttons;
    public Image playerImage;
    private static BackpackManager _instance;
    //public string mainMenuSceneName = "OverworldScene";
    public bool isShowCanvas = false;
    public ItemInfo itemInfo;
    public List<DropItemType> dropItems = new List<DropItemType>();
    public List<int> itemCounts = new List<int>();
    public int maxItemCount = 16;
    public int currentIndex = 0;

    [Header("UI")]
    [SerializeField]private Transform itemContent;
    private ItemUse[] itemUses;
    [SerializeField] private GameObject pauseUI; // 拖入PauseUI父物体
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Level;
    [SerializeField] private TextMeshProUGUI Health;
    [SerializeField] private TextMeshProUGUI Attack;
    [SerializeField] private TextMeshProUGUI Speed;

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
        // 确保单例在场景切换时不被销毁
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Button[] changePlayerButtons = buttons.GetComponentsInChildren<Button>();
        changePlayerButtons[0].onClick.AddListener(() =>
        {
            //Last
            currentIndex--;
            int maxPlayer = PartyManager.Instance.GetAliveParty().Count;
            currentIndex = (currentIndex+maxPlayer)%maxPlayer;
            DataUpdate();
        });
        changePlayerButtons[1].onClick.AddListener(() =>
        {
            //Next
            currentIndex++;
            int maxPlayer = PartyManager.Instance.GetAliveParty().Count;
            currentIndex = (currentIndex+maxPlayer)%maxPlayer;
            DataUpdate();
        });

        itemUses = itemContent.GetComponentsInChildren<ItemUse>();
        for (int i = 0; i < itemUses.Length; i++)
        {
            itemUses[i].idx = i;
        }
    }

    public PartyMember GetCurrentPartyMember()
    {
        return PartyManager.Instance.GetAliveParty()[currentIndex];
    }
    
    void Update()
    {
        //Debug.Log("按键:"+Input.GetKeyDown(KeyCode.R));
        //若当前的活动场景为xxx并且按下ESC按钮
        if (SceneManager.GetActiveScene().name == "OverworldScene" && Input.GetKeyDown(KeyCode.R))
        {
            ToggleShowPanel();
        }

        if (isShowCanvas)
        {
            UpdateItemInfo();
        }
    }

    public void UpdateItemInfo()
    {
        if (currentItem == null)
        {
            showItemInfo.gameObject.SetActive(false);
            return;
        }
        showItemInfo.gameObject.SetActive(true);
        TMP_Text[] texts = showItemInfo.GetComponentsInChildren<TMP_Text>();
        texts[0].text = currentItem.item.name;
        texts[1].text = currentItem.item.description;
        showItemInfo.gameObject.SetActive(false);
        showItemInfo.gameObject.SetActive(true);
    }

    public void UseItem(int idx,ItemUse iu)
    {
        RemoveItem(idx);
        UpdatePanel();
        currentItem = iu.item == null ? null : iu;
    }

    private void RemoveItem(int idx)
    {
        if (itemCounts[idx] > 1)
        {
            itemCounts[idx]--;
            return;
        }
        dropItems.RemoveAt(idx);
        itemCounts.RemoveAt(idx);
    }

    public void AddItem(DropItemType itemType)
    {
        int idx;
        if (CheckIsFull(itemType,out idx))
        {
            Debug.Log("背包已满");
            return;
        }
        if (idx < dropItems.Count)
        {
            itemCounts[idx]++;
        }
        else
        {
            dropItems.Add(itemType);
            itemCounts.Add(1);
        }
    }

    //idx返回背包未满时可存放的位置索引
    private bool CheckIsFull(DropItemType itemType,out int idx)
    {
        for (int i = 0; i < dropItems.Count; i++)
        {
            DropItemType t = dropItems[i];
            if(t!= itemType)
                continue;
            if (itemCounts[i] < itemInfo.GetItemMaxCount(itemType))
            {
                idx = i;
                return false;
            }
        }
        if (dropItems.Count < maxItemCount)
        {
            idx = dropItems.Count;
            return false;
        }
        idx = -1;
        return true;
    }
    
    public void UpdatePanel()
    {
        int childCount = itemContent.childCount;
        int itemCount = dropItems.Count;
        for (int i = 0; i < childCount; i++)
        {
            ItemUse iu = itemUses[i];
            Image image = iu.transform.GetChild(0).GetComponent<Image>();
            if (i < itemCount)
            {
                Item newItem = new Item(dropItems[i]);
                image.sprite = newItem.sprite;
                iu.SetItem(newItem);
                if(itemCounts[i]>1)
                    iu.transform.GetChild(1).GetComponent<TMP_Text>().text = itemCounts[i].ToString();
                else
                    iu.transform.GetChild(1).GetComponent<TMP_Text>().text = "";
            }
            else
            {
                image.sprite = null;
                iu.SetItem(null);
                iu.transform.GetChild(1).GetComponent<TMP_Text>().text = "";
            }
        }
        //显示角色按钮
        buttons.gameObject.SetActive(PartyManager.Instance.GetAliveParty().Count > 1);
        DataUpdate();
    }

    public void ToggleShowPanel()
    {
        isShowCanvas = !isShowCanvas;
        pauseUI.SetActive(isShowCanvas); // 根据状态显示/隐藏
        Time.timeScale = isShowCanvas ? 0 : 1;
        if (isShowCanvas)
        {
            currentIndex = 0;
            UpdatePanel();
        }
    }
    public void DataUpdate()
    {
        PartyMember currentPlayer = PartyManager.Instance.GetAliveParty()[currentIndex];
        playerImage.sprite = currentPlayer.sprite;
        Name.text = currentPlayer.MemberName;
        Level.text = currentPlayer.Level.ToString();
        Health.text = $"{currentPlayer.CurrHealth}/{currentPlayer.MaxHealth}";
        Speed.text = currentPlayer.Speed.ToString();
        Attack.text = currentPlayer.Strength.ToString();
    }
}