using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PartyManager : MonoBehaviour
{
    public PlayerInfo playerInfos;//存有所有成员的数组
    public List<PlayerData> currentParty;
    public Vector3 playerInitialPosition;
    private Vector3 playerPosition;    //玩家的三维坐标
    public static PartyManager Instance{ get; private set; }

    private void Awake()
    {
        LoadPlayerParty();
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    
    public void AddMemberToPartyByName(string memberName)
    {
        PlayerSpawnInfo psi = playerInfos.GetInfoFromName(memberName);
        PlayerData pd = new PlayerData(psi);
        currentParty.Add(pd);
    }
    
    public List<PlayerData> GetAliveParty()
    {
        for(int i = 0; i < currentParty.Count; i++)
        {
            //若当前角色血量为0
            if (currentParty[i].currHealth <= 0)
            {
                currentParty.RemoveAt(i);//将当前角色从列表中移除
            }
        }
        return currentParty;
    }

    public List<PlayerData> GetCurrentParty()
    {
        return currentParty;
    }

    //同步角色的当前状态
    public void SaveHealth(int partyMember,int health)
    {
        currentParty[partyMember].currHealth = health; 
    }

    public void SavePlayerParty()
    {
        SaveLoadManager.SaveBinaryFile($"{ConfigString.PLAYER_DATA}_currentParty",currentParty);
        if (CharacterManager.Instance != null)
        {
            playerPosition = CharacterManager.Instance.transform.position;
        }
        float[] position = { playerPosition.x, playerPosition.y, playerPosition.z };
        SaveLoadManager.SaveBinaryFile($"{ConfigString.PLAYER_DATA}_playerPosition",position);
    }

    public void LoadPlayerParty()
    {
        object data = SaveLoadManager.LoadBinaryFile($"{ConfigString.PLAYER_DATA}_currentParty");
        if (data != null)
        {
            currentParty = (List<PlayerData>)data;
            float[] position = (float[])SaveLoadManager.LoadBinaryFile($"{ConfigString.PLAYER_DATA}_playerPosition");
            playerPosition = new Vector3(position[0], position[1], position[2]);
        }
        else
        {
            currentParty = new List<PlayerData>();
            playerPosition = playerInitialPosition;
            AddMemberToPartyByName(ConfigString.DEFAULT_PLAYER_NAME);//添加角色信息
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
}