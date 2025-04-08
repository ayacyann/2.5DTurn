using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOInfo/Enemy Info")]
public class EnemyInfo : ScriptableObject
{
    public List<EnemySpawnInfo> enemySpawnInfos = new List<EnemySpawnInfo>();

    public EnemySpawnInfo GetInfoFromName(string enemyName)
    {
        return enemySpawnInfos.Find(x=>x.memberName==enemyName);
    }
}