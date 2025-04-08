using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//资源菜单
[CreateAssetMenu(menuName="SOInfo/Party Member Info")]
public class PlayerInfo : ScriptableObject
{
    public List<PlayerSpawnInfo> playerSpawnInfos = new List<PlayerSpawnInfo>();
    
    public PlayerSpawnInfo GetInfoFromName(string enemyName)
    {
        return playerSpawnInfos.Find(x=>x.memberName==enemyName);
    }
}