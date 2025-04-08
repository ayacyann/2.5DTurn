using System;
using UnityEngine;

[Serializable]
public class Character
{
    public string memberName;
    public int level;
    public int currHealth;//当前血量
    public int maxHealth;//最大血量
    public int strength;
    public int speed;

    public Character()
    {
        
    }

    public Character(Character character)
    {
        memberName = character.memberName;
        level = character.level;
        currHealth = character.currHealth;
        maxHealth = character.maxHealth;
        strength = character.strength;
        speed = character.speed;
    }
}

[Serializable]
public class PlayerData:Character
{
    public int currExp;//当前经验
    public int maxExp;//最大经验

    public PlayerData()
    {
        
    }
    
    public PlayerData(PlayerData playerData):base(playerData)
    {
        currExp = playerData.currExp;
        maxExp = playerData.maxExp;
    }
}

[Serializable]
public class EnemyData:Character
{
    public int expNum; //可获取的经验

    public EnemyData()
    {
        
    }

    public EnemyData(EnemyData enemyData):base(enemyData)
    {
        expNum = enemyData.expNum;
    }
}

[Serializable]
public class PlayerSpawnInfo : PlayerData
{
    public Sprite sprite;
    public GameObject memberBattleVisualPrefab;//战斗场景下的预制体
    public GameObject memberOverworldVisualPrefab;//主世界场景中的预制体

    public PlayerSpawnInfo(PlayerData playerData) : base(playerData)
    {
        
    }
}

[Serializable]
public class EnemySpawnInfo : EnemyData
{
    public GameObject memberBattleVisualPrefab;//战斗场景下的预制体

    public EnemySpawnInfo(EnemyData enemyData) : base(enemyData)
    {
        
    }
}


public class BattleEntities : Character
{
    public enum Action { Attack,Run }
    public Action battleAction;
    public bool isPlayer;
    public BattleVisual battleVisual;//创建一个公共战斗视觉效果
    public int target;//选择目标

    //将角色信息赋值到战斗系统中
    public BattleEntities(Character characterData,BattleVisual battleVisual,bool isPlayer):base(characterData)
    {
        this.battleVisual = battleVisual;
        this.isPlayer = isPlayer;
    }

    public void SetTarget(int target)
    {
        this.target = target;
    }

    public void UpdateUI()//UI更新
    {
        battleVisual.ChangeHealth(currHealth);
    }
}
