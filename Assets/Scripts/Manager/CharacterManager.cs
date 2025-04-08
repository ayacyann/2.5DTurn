using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//处理角色加入的逻辑
public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject joinPopup;//弹窗对象
    [SerializeField] private TextMeshProUGUI joinPopupText;//弹窗文本
    private bool infrontOfPartyMember;
    private GameObject joinableMember;
    private PlayerControls playerControls;
    private List<GameObject> overworldCharacter = new List<GameObject>();
    public Vector3 playerInitialPosition;
    private Vector3 playerPosition;    //玩家的三维坐标
    private const string PARTY_JOINED_MESSAGE = "joined The Party!";
    private const string NPC_JOINABLE_TAG = "NPC Joinable";
    public static CharacterManager Instance{get; private set;}

    private void Awake()
    {
        playerControls = new PlayerControls();//初始化
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        LoadPlayerPosition();
        Debug.Log(playerControls);
    }

    void Start()
    {
        playerControls.Player.Interact.performed += _ => Interct();
        SpawnOverworldMembers();
    }

    private void OnEnable()//启用
    {
        playerControls.Enable();//启用播放器
    }
    
    private void OnDisable()//禁用
    {
        playerControls.Disable();//禁用播放器
    }
    
    private void Interct()
    {
        //若可加入角色不为空且可以交互
        if(infrontOfPartyMember == true && joinableMember != null)
        {
            //将该队员加入队伍
            string playerName = joinableMember.GetComponent<JoinableCharacterScript>().memberToJoin;
            Debug.Log("加入:"+playerName);
            MemberJoined(playerName);
            infrontOfPartyMember = false;
            joinableMember = null;

        }
    }

    private void MemberJoined(string playerName)//添加成员的函数
    {
        //添加成员,输入成员姓名将派对成员添加到派对中
        GameObject.FindFirstObjectByType<PartyManager>().AddMemberToPartyByName(playerName);
        joinableMember.GetComponent<JoinableCharacterScript>().CheckIfJoined();

        joinPopup.SetActive(true);//开启弹窗
        joinPopupText.text = playerName + PARTY_JOINED_MESSAGE;//弹窗文本
        SpawnOverworldMembers();//添加成员时再次刷新主场景
    }

    private void SpawnOverworldMembers()//生成主世界成员
    {
        for(int i = 0; i < overworldCharacter.Count; i++)
        {
            Destroy(overworldCharacter[i]);//摧毁主世界的角色
        }
        overworldCharacter.Clear();//清空列表

        //获取场景中的第一个成员
        PartyManager pm = PartyManager.Instance;
        List<PlayerData> currentParty = pm.GetCurrentParty();

        for(int i = 0; i < currentParty.Count; i++)
        {
            PlayerData playerData = currentParty[i];
            PlayerSpawnInfo playerSpawnInfo = pm.playerInfos.GetInfoFromName(playerData.memberName);
            if (i == 0)//第一个成员为玩家
            {
                GameObject player = gameObject;//获取这个预制体
                //玩家视觉对象
                GameObject playerVisual = Instantiate(playerSpawnInfo.memberOverworldVisualPrefab,playerPosition, Quaternion.identity);
                Debug.Log("坐标:" + transform.position);
                playerVisual.transform.SetParent(player.transform);//
                player.GetComponent<PControl>().SetOverworldVisuals(playerVisual.GetComponent<Animator>(),playerVisual.GetComponent<SpriteRenderer>(),playerVisual.transform.localScale);//设置视觉效果
                //禁用跟随AI
                playerVisual.GetComponent<MemberFollowAI>().enabled = false;
                overworldCharacter.Add(playerVisual);//添加主世界角色
            }
            else//否则其他成员为跟随者
            {
                Vector3 positionToSpawn = transform.position;//角色坐标
                positionToSpawn.x = -i;
                //临时跟随者
                GameObject tempFollower= Instantiate(playerSpawnInfo.memberOverworldVisualPrefab, positionToSpawn, Quaternion.identity);
                //获取跟随者组件跟随
                tempFollower.GetComponent<MemberFollowAI>().SetFollowDistance(i);
                overworldCharacter.Add(tempFollower);
            }
        }
    }
    
    public void SetPosition(Vector3 position)//获取角色当前位置
    {
        playerPosition = position;
    }

    public Vector3 GetPosition()
    {
        return playerPosition;//返回玩家坐标
    }

    //碰撞检测开启
    private void OnTriggerEnter(Collider other)
    {
        //若游戏对象标签为NPC可连接标签
        if (other.gameObject.tag == NPC_JOINABLE_TAG)
        {
            infrontOfPartyMember = true;
            joinableMember = other.gameObject;//将当前碰到的角色设置为可加入队员
            //获取UI界面的组件并激活互动面板
            joinableMember.GetComponent<JoinableCharacterScript>().ShowInteractPrompt(true);
        }
    }

    //碰撞检测退出
    private void OnTriggerExit(Collider other)
    {
        //若游戏对象标签为NPC可连接标签
        if (other.gameObject.tag == NPC_JOINABLE_TAG)
        {
            infrontOfPartyMember = false;

            //获取UI界面的组件并激活互动面板
            joinableMember.GetComponent<JoinableCharacterScript>().ShowInteractPrompt(false);
            joinableMember = null;
        }
    }
    
    public void SavePlayerPosition()
    {
        playerPosition = transform.position;
        float[] position = { playerPosition.x, playerPosition.y, playerPosition.z };
        SaveLoadManager.SaveBinaryFile($"{ConfigString.PLAYERDATA}_playerPosition",position);
    }

    public void LoadPlayerPosition()
    {
        object data = SaveLoadManager.LoadBinaryFile($"{ConfigString.PLAYERDATA}_playerPosition");
        if (data != null)
        {
            float[] position = (float[])data;
            playerPosition = new Vector3(position[0], position[1], position[2]);
        }
        else
        {
            playerPosition = playerInitialPosition;
        }
    }
}