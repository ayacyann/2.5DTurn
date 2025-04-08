using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PartyManager : MonoBehaviour
{
    public PlayerInfo playerInfos;//存有所有成员的数组
    public List<PlayerData> currentParty;
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
        SaveLoadManager.SaveBinaryFile($"{ConfigString.PLAYERDATA}_currentParty",currentParty);
    }

    public void LoadPlayerParty()
    {
        object data = SaveLoadManager.LoadBinaryFile($"{ConfigString.PLAYERDATA}_currentParty");
        if (data != null)
        {
            currentParty = (List<PlayerData>)data;
        }
        else
        {
            currentParty = new List<PlayerData>();
            AddMemberToPartyByName(ConfigString.DEFAULTPLAYERNAME);//添加角色信息
        }
    }
}