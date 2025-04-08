using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public EnemyInfo enemyInfos;//存有所有成员的数组
    public List<EnemyData> currentEnemies;//敌人列表
    
    private const float LEVEL_MODIFIER = 0.5f;
    public static EnemyManager Instance{get; private set;}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //用于随机生成敌人
    public void GenerateEnemiesByEncounter(Encounter[] encounters,int maxNumEnemies)
    {
        currentEnemies.Clear();//清除敌人列表
        int numEnemies=Random.Range(1,maxNumEnemies);//随机敌人的数量,在1~3之间

        for(int i = 0; i < numEnemies; i++)
        {
            //用临时变量存储随机遭遇
            Encounter tempEncounter = encounters[Random.Range(0, encounters.Length)];//随机怪物数量
            //随机怪物等级
            int level = Random.Range(tempEncounter.levelMin, tempEncounter.levelMax+1);
            //生成怪物
            GenerateEnemyByName(tempEncounter.enemyName, level);
        }
    }
    private void GenerateEnemyByName(string enemyName,int level)
    {
        EnemySpawnInfo esi = enemyInfos.GetInfoFromName(enemyName);
        EnemyData newEnemy = new EnemyData(esi)
        {
            level = level
        };
        //怪物等级的成长曲线,怪物每加一级属性上升值为:属性*levelModifier
        float levelModifier = (LEVEL_MODIFIER * newEnemy.level);
        //新敌人的最大生命=可编写脚本的对象基本生命值
        newEnemy.maxHealth = Mathf.RoundToInt(esi.maxHealth+(esi.maxHealth*levelModifier));
        newEnemy.currHealth = newEnemy.maxHealth;
        newEnemy.strength = Mathf.RoundToInt(esi.strength+(esi.strength*levelModifier));
        newEnemy.speed = Mathf.RoundToInt(esi.speed + (esi.speed * levelModifier));
        //敌人视觉预制件
        currentEnemies.Add(newEnemy);
    }

    public List<EnemyData> GetCurrentEnemies()
    {
        return currentEnemies;
    }
}