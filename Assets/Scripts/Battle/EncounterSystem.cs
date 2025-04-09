using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterSystem : MonoBehaviour
{
    [SerializeField] private Encounter[] enemiesInScene;//场景中的敌人
    [SerializeField] private int maxNumEnemies;//生成的敌人数量
    // Start is called before the first frame update

    void Start()
    {
        //通过遭遇生成敌人
        EnemyManager.Instance.GenerateEnemiesByEncounter(enemiesInScene,maxNumEnemies);
    }
}

[System.Serializable]
public class Encounter
{
    public string enemyName;//敌人属性
    public int levelMin;//最低等级
    public int levelMax;//最高等级

}