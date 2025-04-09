using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

//没看懂
public class BattleSystem : MonoBehaviour
{
    public enum BattleState//战斗状态
    {
        //选择状态,战斗状态,胜利,失败,逃跑
        Start,Selection,Battle,Won,Lost,Run
    }

    [FormerlySerializedAs("State")]
    [Header("战斗状态")]
    [SerializeField] private BattleState state;
    public BattleState State => state;

    [Header("重生点")]
    [SerializeField] private Transform[] partySpawnPoints;//角色的重生点 
    [SerializeField] private Transform[] enemySpawnPoints;//敌人的重生点 

    //战斗实体类型
    [Header("Battlers")]
    [SerializeField] private List<BattleEntities> allBattlers = new List<BattleEntities>();//所有角色战斗实体列表
    [SerializeField] private List<BattleEntities> enemyBattlers = new List<BattleEntities>();//敌人战斗实体列表;
    [SerializeField] private List<BattleEntities> playerBattlers = new List<BattleEntities>();//角色战斗实体列表

    [Header("UI")]
    [SerializeField] private GameObject[] enemySelectionButtons;//敌人选择菜单
    [SerializeField] private GameObject battleMenu;
    [SerializeField] private GameObject enemySelectionMenu;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private GameObject bottomTextPopUp;
    [SerializeField] private TextMeshProUGUI bottomText;
    private AttackLine showAttackLine;


    private PartyManager partyManager;
    private EnemyManager enemyManager;
    private int currentPlayer;//已选择操作的玩家
    public BattleEntities CurrentPlayer => playerBattlers[currentPlayer];

    private const string ACTION_MESSAGE = " s Action:";
    private const string WIN_MESSAGE = "Win !";
    private const string LOST_MESSAGE = "Lost";
    private const string SUCCESFULLY_RAN_MESSAGE = "Run!";
    private const string UNSUCCESFULLY_RAN_MESSAGE = "No Run";
    private const int TURN_DURATION = 2;
    private const int RUN_CHANCE = 50;
    private const string OVERWORLD_SCENE = "OverworldScene";
    // Start is called before the first frame update
    
    void Start()
    {
        partyManager = GameObject.FindAnyObjectByType<PartyManager>();
        enemyManager = GameObject.FindAnyObjectByType<EnemyManager>();

        CreatePartyEnitities();
        CreateEnemyEntities();
        ShowBattleMenu();
        //AttackAction(allBattlers[0], allBattlers[1]);
        DetermineBattleOrder();
        InitSelectionRef();
    }

    private void InitSelectionRef()
    {
        for (var i = 0; i < enemySelectionButtons.Length; i++)
        {
            var button = enemySelectionButtons[i];
            SelectionEvent se = button.GetComponent<SelectionEvent>();
            se.InitBattleSystem(this);
            se.idx = i;
        }
        showAttackLine = GetComponentInChildren<AttackLine>();
    }

    public List<BattleEntities> GetEnemyEntities()
    {
        return enemyBattlers;
    }

    public void ShowLine(Transform start, Transform end)
    {
        showAttackLine.SetTransform(start, end);
    }
    
    private IEnumerator BattleRoutine()//战斗例程
    {
        //先禁用敌人的战斗状态;接着循环遍历所有的角色
        enemySelectionMenu.SetActive(false);
        state = BattleState.Battle;//将当前状态设为战斗状态
        bottomTextPopUp.SetActive(true);//打开底部的文本面板

        //遍历所有的角色
        for(int i = 0; i < allBattlers.Count; i++)
        {
            //若当前属于战斗状态并且角色当前血量大于0
            if (state == BattleState.Battle && allBattlers[i].currHealth>0)
            {
                //用简单的switch检查每个战斗者要执行的动作
                switch (allBattlers[i].battleAction)
                {
                    case BattleEntities.Action.Attack://若该角色选择战斗,则
                                                      //执行攻击操作
                        yield return StartCoroutine(AttackRoutine(i));//启动AttackRoutine协程
                        break;
                    case BattleEntities.Action.Run://若该角色选择逃跑,则
                        yield return StartCoroutine(RunRoutine());                           //退出战斗
                        break;

                    default:
                        Debug.Log("其他");
                        break;
                }
            }
        }
        RemoveDeadBattlers();
        //如果循环结束后依旧处于战斗状态,就继续执行上面的循环,返回战斗菜单
        if (state == BattleState.Battle)
        {
            bottomTextPopUp.SetActive(false);
            currentPlayer = 0;
            ShowBattleMenu();//返回战斗菜单
        }
        yield return null;
        
    }

    private IEnumerator AttackRoutine(int i)//战斗逻辑
    {
        if (allBattlers[i].isPlayer)//如果该角色为玩家
        {
            BattleEntities currAttacker = allBattlers[i]; //当前攻击者
            //如果攻击目标是玩家，或者目标序列超出所有角色的数量
            if (currAttacker.target>=allBattlers.Count || allBattlers[currAttacker.target].currHealth<=0)//若当前攻击目标的血量小于0
            {
                currAttacker.SetTarget(GetRandomEnemy());//将攻击目标设置为随机敌人
            }
            BattleEntities currTarget = allBattlers[currAttacker.target];//当前目标
            AttackAction(currAttacker, currTarget);//执行攻击行为
            yield return new WaitForSeconds(TURN_DURATION);//2秒后再执行下面的逻辑

            if (currTarget.currHealth <= 0)//若目标的当前血量为0,播放死亡动画
            {
                //导入击败文本
                bottomText.text = string.Format("{0} defeated {1}\n", currAttacker.memberName, currTarget.memberName);
                DropItem(currTarget.memberName,bottomText);
                yield return new WaitForSeconds(TURN_DURATION);
                enemyBattlers.Remove(currTarget);//将目标敌人从敌人列表中移除
                allBattlers.Remove(currTarget);//将敌人从所有成员的列表中移除

                if (enemyBattlers.Count <= 0)//如果敌人数量为0,结束战斗
                {
                    state = BattleState.Won;//将状态改为胜利
                    bottomText.text = WIN_MESSAGE;
                    yield return new WaitForSeconds(TURN_DURATION);
                    //返回主场景，不使用渐变
                    LoadSceneManager.Instance.LoadScene(OVERWORLD_SCENE,isFade:false);
                }
            }
        }
        //若攻击对象是敌人
        if (i<allBattlers.Count && allBattlers[i].isPlayer == false)//这里主要确保我们的索引依旧在敌人的回合范围内。
        {
            //确定发动攻击的对象
            BattleEntities currAttacker = allBattlers[i];
            currAttacker.SetTarget(GetRandomPartyMember());//初始化目标变量
            //让敌人随机选择进攻的目标玩家
            BattleEntities currTarget = allBattlers[currAttacker.target];

            AttackAction(currAttacker, currTarget);//敌人进攻
            yield return new WaitForSeconds(TURN_DURATION);

            if (currTarget.currHealth <= 0)
            {
                //导入击败文本
                bottomText.text = string.Format("{0} defeated {1}", currAttacker.memberName, currTarget.memberName); ;
                yield return new WaitForSeconds(TURN_DURATION);
                playerBattlers.Remove(currTarget);//将目标玩家从玩家列表中移除
                allBattlers.Remove(currTarget);//将该玩家从所有成员的列表中移除
                PartyManager.Instance.RemoveParty(currTarget.memberName);

                if (playerBattlers.Count <= 0 || currTarget.memberName==ConfigString.DEFAULT_PLAYER_NAME)//如果玩家数量为0,结束战斗
                {
                    bottomText.text = LOST_MESSAGE;
                    battleMenu.gameObject.SetActive(false);
                    enemySelectionMenu.gameObject.SetActive(false);
                    yield return new WaitForSeconds(TURN_DURATION);
                    bottomText.text = "Press any key to back menu!";
                    state = BattleState.Lost;//将状态改为失败
                }
            }
        }
    }

    public void DropItem(string target,TMP_Text text)
    {
        ItemInfo itemInfo = BackpackManager.Instance.itemInfo;
        List<string> names = new List<string>();
        foreach (var item in itemInfo.items)
        {
            float rand = Random.value;
            if (rand < item.probability)
            {
                BackpackManager.Instance.AddItem(item.dropType);
                names.Add(item.name);
            }
        }
        if (names.Count>0)
        {
            text.text += $"{target} Drop: ";
            for (var i = 0; i < names.Count; i++)
            {
                var n = names[i];
                text.text += n;
                text.text += i!=names.Count-1 ? ", " : ".";
            }
        }
    }
    
    
    private void RemoveDeadBattlers()//删除死去的玩家角色
    {
        for(int i = 0; i < allBattlers.Count; i++)
        {
            if (allBattlers[i].currHealth <= 0)
            {
                allBattlers.RemoveAt(i);
            }
        }
    }

    private void CreatePartyEnitities()//创建角色战斗实体
    {
        List<PlayerData> currentParty = partyManager.GetAliveParty();
        //循环遍历所有成员,并开始创建空的战斗实体
        for(int i = 0; i < currentParty.Count; i++)
        {
            PlayerData pData = currentParty[i];
            PlayerSpawnInfo psi = PartyManager.Instance.playerInfos.GetInfoFromName(pData.memberName);
            BattleVisual tempBattleVisual=
            Instantiate(psi.memberBattleVisualPrefab,partySpawnPoints[i].position,
                Quaternion.identity).GetComponent<BattleVisual>();       //生成战斗生成视觉效果
            //将角色的信息赋值给临时变量
            tempBattleVisual.SetStartingValues(pData.currHealth,pData.maxHealth,pData.level);
            BattleEntities tempEntity = new BattleEntities(pData,tempBattleVisual,true);
            //将战斗实体存放到所有实体和角色实体的队列中
            allBattlers.Add(tempEntity);
            playerBattlers.Add(tempEntity);
        }
        
    }
//On Click事件:可以将某个函数绑定给某个按钮,当按下按钮时自动调用该函数。
    private void CreateEnemyEntities()//创建敌人战斗实体
    {
        List<EnemyData> currentEnemies = enemyManager.GetCurrentEnemies();
        for (int i = 0; i < currentEnemies.Count; i++)
        {
            EnemyData eData = currentEnemies[i];
            EnemySpawnInfo esi = EnemyManager.Instance.enemyInfos.GetInfoFromName(eData.memberName);
            BattleVisual tempBattleVisual =
            Instantiate(esi.memberBattleVisualPrefab,
            enemySpawnPoints[i].position, Quaternion.identity).GetComponent<BattleVisual>();//生成战斗生成视觉效果
            BattleEntities tempEntity = new BattleEntities(eData,tempBattleVisual,false);
            //将角色的信息赋值给临时变量
            tempBattleVisual.SetStartingValues(eData.maxHealth, eData.maxHealth, eData.level);
            //将敌人的实体添加到敌人和所有角色的队列中
            allBattlers.Add(tempEntity);
            enemyBattlers.Add(tempEntity);

        }


    }
    public void ShowBattleMenu()//显示角色的战斗菜单
    {
        actionText.text = playerBattlers[currentPlayer].memberName + ACTION_MESSAGE;
        battleMenu.SetActive(true);
    }

    public void ShowEnemySelectionMenu()//敌人选择菜单
    {
        battleMenu.SetActive(false);
        SetEnemySelectionButtons();
        enemySelectionMenu.SetActive(true);//将菜单点设为活动状态
    }

    private void SetEnemySelectionButtons()
    {
        //若此时只有一个野怪时,禁掉所有的按钮
        for(int i = 0; i < enemySelectionButtons.Length; i++)
        {
            enemySelectionButtons[i].SetActive(false);//关闭按钮的活动状态
        }

        for(int j = 0; j < enemyBattlers.Count; j++)
        {
            enemySelectionButtons[j].SetActive(true);//将当前存在的敌人对应的选项按钮激活
            //获取敌人选项按钮的文本,将文本改为怪物的名字
            enemySelectionButtons[j].GetComponentInChildren<TextMeshProUGUI>().text = enemyBattlers[j].memberName;
        }
    }
    
    public void SelectEnemy(int currentEnemy)
    {
        //先确定成员的攻击目标
        BattleEntities currentPlayerEntity = playerBattlers[currentPlayer];//获取当前玩家实体
        currentPlayerEntity.SetTarget(allBattlers.IndexOf(enemyBattlers[currentEnemy]));//确定玩家要攻击的目标敌人
        currentPlayerEntity.battleAction = BattleEntities.Action.Attack;
        ShowLine(null,null);
        currentPlayer++;//选择操作玩家的数量+1
        if (currentPlayer >= playerBattlers.Count)//当所有玩家都选择完操作后,开始战斗
        {
            //开始战斗
            StartCoroutine(BattleRoutine());//开启BattleRoutine协程
        }
        else
        {
            enemySelectionMenu.SetActive(false);
            ShowBattleMenu();//显示战斗菜单
        }
    }
    
    private void AttackAction(BattleEntities currAttacker,BattleEntities currTarget)
    {
        //获取攻击,然后播放攻击动画,接着造成伤害,更新UI
        int damage = currAttacker.strength;
        //int damage = 20;
        currAttacker.battleVisual.PlayAttackAnimation();//播放攻击动画
        //更新当前目标的血量
        currTarget.currHealth -= damage;
         currTarget.battleVisual.PlayHitAnimation();//播放目标受击动画
        currTarget.UpdateUI();//更新血条UI
        bottomText.text = string.Format("{0} attacks {1} for {2} damage",currAttacker.memberName,currTarget.memberName,damage);
        SaveHealth();//保存当前生命值
    }

    private int GetRandomPartyMember()//敌人随机选择目标
    {
        List<int> partyMembers = new List<int>();//存储玩家序号的列表
        for(int i = 0; i < allBattlers.Count; i++)
        {
            if (allBattlers[i].isPlayer == true && allBattlers[i].currHealth>0)//若当前角色属于玩家
            {
                partyMembers.Add(i);//将玩家序号存储到列表中
            }
        }
        return partyMembers[Random.Range(0,partyMembers.Count)];//从列表中随机返回一个敌人的选择目标
    }

    private int GetRandomEnemy()//随机获取一个敌人序列
    {
        List<int> enemies = new List<int>();//获取随机怪物序列的列表
        for (int i = 0; i < allBattlers.Count; i++)
        {
            if (allBattlers[i].isPlayer == false && allBattlers[i].currHealth > 0)//若当前角色不属于玩家
            {
                enemies.Add(i);//添加到敌人序列列表中
            }
        }
        return enemies[Random.Range(0, enemies.Count)];

    }

    private void SaveHealth()//保存每位角色的当前生命值
    {
        for(int i = 0; i < playerBattlers.Count; i++)
        {
            partyManager.SaveHealth(i, playerBattlers[i].currHealth);
        }
    }
 
    private void DetermineBattleOrder()//决定所有角色的出手顺序
    {
        //对所有角色的主动权进行排序
        allBattlers.Sort((bi1,bi2)=>-bi1.speed.CompareTo(bi2.speed));
    }

    public void SelectRunAction()//选择逃跑操作
    {
        state = BattleState.Selection;
        BattleEntities currentPlayerEntity = playerBattlers[currentPlayer];//获取当前玩家实体
    
        currentPlayerEntity.battleAction = BattleEntities.Action.Run;
        battleMenu.SetActive(false);
        currentPlayer++;//选择操作玩家的数量+1
        if (currentPlayer >= playerBattlers.Count)//当所有玩家都选择完操作后,开始战斗
        {
            //开始战斗
            StartCoroutine(BattleRoutine());//开启BattleRoutine协程
        }
        else
        {
            enemySelectionMenu.SetActive(false);
            ShowBattleMenu();//显示战斗菜单
        }
    }

    private IEnumerator RunRoutine()//逃跑例程
    {
        if (state == BattleState.Battle)
        {
            if (Random.Range(1,101)>=RUN_CHANCE)
            {
                //先显示逃跑成功文本
                bottomText.text = SUCCESFULLY_RAN_MESSAGE;
                //将状态改为逃跑
                state = BattleState.Run;
                allBattlers.Clear();//清空所有的角色列表

                yield return new WaitForSeconds(TURN_DURATION);
                LoadSceneManager.Instance.LoadScene(OVERWORLD_SCENE,isFade:false);//切换为主场景
                yield break;
            }
            else
            {
                //逃跑失败文本
                bottomText.text = UNSUCCESFULLY_RAN_MESSAGE;
                yield return new WaitForSeconds(TURN_DURATION);
            }
        }
    }
}