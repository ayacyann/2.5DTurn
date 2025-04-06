using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


//û����
public class BattleSystem : MonoBehaviour
{
    [SerializeField] private enum BattleState//ս��״̬
    {
        //ѡ��״̬,ս��״̬,ʤ��,ʧ��,����
        Start,Selection,Battle,Won,Lost,Run
    }

    [Header("ս��״̬")]
    [SerializeField] private BattleState State;

    [Header("������")]
    [SerializeField] private Transform[] partySpawnPoints;//��ɫ�������� 
    [SerializeField] private Transform[] enemySpawnPoints;//���˵������� 

    //ս��ʵ������
    [Header("Battlers")]
    [SerializeField] private List<BattleEntities> allBattlers = new List<BattleEntities>();//���н�ɫս��ʵ���б�
    [SerializeField] private List<BattleEntities> EnemyBattlers = new List<BattleEntities>();//����ս��ʵ���б�;
    [SerializeField] private List<BattleEntities> PlayerBattlers = new List<BattleEntities>();//��ɫս��ʵ���б�

    [Header("UI")]
    [SerializeField] private GameObject[] enemySelectionButtons;//����ѡ��˵�
    [SerializeField] private GameObject battleMenu;
    [SerializeField] private GameObject enemySelectionMenu;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private GameObject bottomTextPopUp;
    [SerializeField] private TextMeshProUGUI bottomText;


    private PartyManager partyManager;
    private EnemyManager enemyManager;
    private int currentPlayer;//��ѡ����������

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
        DetermineBattleOrde();
    }

    private IEnumerator BattleRoutine()//ս������
    {
        //�Ƚ��õ��˵�ս��״̬;����ѭ���������еĽ�ɫ
        enemySelectionMenu.SetActive(false);
        State = BattleState.Battle;//����ǰ״̬��Ϊս��״̬
        bottomTextPopUp.SetActive(true);//�򿪵ײ����ı����

        //�������еĽ�ɫ
        for(int i = 0; i < allBattlers.Count; i++)
        {
            //����ǰ����ս��״̬���ҽ�ɫ��ǰѪ������0
            if (State == BattleState.Battle && allBattlers[i].CurrHealth>0)
            {
                //�ü򵥵�switch���ÿ��ս����Ҫִ�еĶ���
                switch (allBattlers[i].BattleAction)
                {
                    case BattleEntities.Action.Attack://���ý�ɫѡ��ս��,��
                                                      //ִ�й�������
                        yield return StartCoroutine(AttackRoutine(i));//����AttackRoutineЭ��
                        break;
                    case BattleEntities.Action.Run://���ý�ɫѡ������,��
                        yield return StartCoroutine(RunRoutine());                           //�˳�ս��
                        break;

                    default:
                        Debug.Log("����");
                        break;
                }
            }
        }
        RemoveDeadBattlers();
        //���ѭ�����������ɴ���ս��״̬,�ͼ���ִ�������ѭ��,����ս���˵�
        if (State == BattleState.Battle)
        {
            bottomTextPopUp.SetActive(false);
            currentPlayer = 0;
            ShowBattleMenu();//����ս���˵�

        }
        yield return null;
        
    }

    private IEnumerator AttackRoutine(int i)//ս���߼�
    {
        if (allBattlers[i].IsPlayer == true)//����ý�ɫΪ���
        {
            BattleEntities currAttacker = allBattlers[i]; //��ǰ������
            //�������Ŀ������ң�����Ŀ�����г������н�ɫ������
            if (allBattlers[currAttacker.Target].CurrHealth<=0)//����ǰ����Ŀ���Ѫ��С��0
            {
                currAttacker.SetTarget(GetRandomEnemy());//������Ŀ������Ϊ�������
            }
            BattleEntities currTarget = allBattlers[currAttacker.Target];//��ǰĿ��
            AttackAction(currAttacker, currTarget);//ִ�й�����Ϊ
            yield return new WaitForSeconds(TURN_DURATION);//2�����ִ��������߼�

            if (currTarget.CurrHealth <= 0)//��Ŀ��ĵ�ǰѪ��Ϊ0,������������
            {
                //��������ı�
                bottomText.text = string.Format("{0} defeated {1}", currAttacker.Name, currTarget.Name); ;
                yield return new WaitForSeconds(TURN_DURATION);
                EnemyBattlers.Remove(currTarget);//��Ŀ����˴ӵ����б����Ƴ�
                allBattlers.Remove(currTarget);//�����˴����г�Ա���б����Ƴ�

                if (EnemyBattlers.Count <= 0)//�����������Ϊ0,����ս��
                {
                    State = BattleState.Won;//��״̬��Ϊʤ��
                    bottomText.text = WIN_MESSAGE;
                    yield return new WaitForSeconds(TURN_DURATION);
                    //����������
                    SceneManager.LoadScene(OVERWORLD_SCENE);
                }
            }
        }
        //�����������ǵ���
        if (i<allBattlers.Count && allBattlers[i].IsPlayer == false)//������Ҫȷ�����ǵ����������ڵ��˵ĻغϷ�Χ�ڡ�
        {
            //ȷ�����������Ķ���
            BattleEntities currAttacker = allBattlers[i];
            currAttacker.SetTarget(GetRandomPartyMember());//��ʼ��Ŀ�����
            //�õ������ѡ�������Ŀ�����
            BattleEntities currTarget = allBattlers[currAttacker.Target];

            AttackAction(currAttacker, currTarget);//���˽���
            yield return new WaitForSeconds(TURN_DURATION);

            if (currTarget.CurrHealth <= 0)
            {
                //��������ı�
                bottomText.text = string.Format("{0} defeated {1}", currAttacker.Name, currTarget.Name); ;
                yield return new WaitForSeconds(TURN_DURATION);
                PlayerBattlers.Remove(currTarget);//��Ŀ����Ҵ�����б����Ƴ�
                allBattlers.Remove(currTarget);//������Ҵ����г�Ա���б����Ƴ�

                if (PlayerBattlers.Count <= 0)//����������Ϊ0,����ս��
                {
                    State = BattleState.Lost;//��״̬��Ϊʧ��
                    bottomText.text = LOST_MESSAGE;
                    yield return new WaitForSeconds(TURN_DURATION);
                    //���Է�����Ϸ������
                }
            }
        }
    }
    private void RemoveDeadBattlers()//ɾ����ȥ����ҽ�ɫ
    {
        for(int i = 0; i < allBattlers.Count; i++)
        {
            if (allBattlers[i].CurrHealth <= 0)
            {
                allBattlers.RemoveAt(i);
            }
        }
    }

    private void CreatePartyEnitities()//������ɫս��ʵ��
    {
        List<PartyMember> CurrentParty = new List<PartyMember>();
        CurrentParty = partyManager.GetAliveParty();

        //ѭ���������г�Ա,����ʼ�����յ�ս��ʵ��
        for(int i = 0; i < CurrentParty.Count; i++)
        {
            BattleEntities tempEntity = new BattleEntities();//ʵ����ս��ʵ��

            tempEntity.SetEntityValues
           (CurrentParty[i].MemberName,CurrentParty[i].CurrHealth,CurrentParty[i].MaxHealth,CurrentParty[i].Initiative,CurrentParty[i].Strength,CurrentParty[i].Level,true);
            //�Ȳ����Ӿ�Ч��,Ȼ�������Ӿ�Ч����ʼֵ,Ȼ��洢ս���Ӿ���Ϣ
            BattleVisual tempBattleVisual=
            Instantiate(CurrentParty[i].MemberBattleVisualPrefab,
            partySpawnPoints[i].position,Quaternion.identity).GetComponent<BattleVisual>();//����ս�������Ӿ�Ч��

            //����ɫ����Ϣ��ֵ����ʱ����
            tempBattleVisual.SetStartingValues(CurrentParty[i].CurrHealth,CurrentParty[i].MaxHealth,CurrentParty[i].Level);
            tempEntity.BattleVisual = tempBattleVisual;


            //��ս��ʵ���ŵ�����ʵ��ͽ�ɫʵ��Ķ�����
            allBattlers.Add(tempEntity);
            PlayerBattlers.Add(tempEntity);
        }
        
    }
//On Click�¼�:���Խ�ĳ�������󶨸�ĳ����ť,�����°�ťʱ�Զ����øú�����
    private void CreateEnemyEntities()//��������ս��ʵ��
    {
        List<Enemy> CurrentEnemies = new List<Enemy>();
        CurrentEnemies = enemyManager.GetCurrentEnemies();
        for (int i = 0; i < CurrentEnemies.Count; i++)
        {
            BattleEntities tempEntity = new BattleEntities();

            tempEntity.SetEntityValues
            (CurrentEnemies[i].EnemyName, CurrentEnemies[i].CurrHealth, CurrentEnemies[i].MaxHealth, CurrentEnemies[i].Initiative, CurrentEnemies[i].Strength, CurrentEnemies[i].Level, false);

            BattleVisual tempBattleVisual =
            Instantiate(CurrentEnemies[i].EnemyVisualPrefab,
            enemySpawnPoints[i].position, Quaternion.identity).GetComponent<BattleVisual>();//����ս�������Ӿ�Ч��

            //����ɫ����Ϣ��ֵ����ʱ����
            tempBattleVisual.SetStartingValues(CurrentEnemies[i].MaxHealth, CurrentEnemies[i].MaxHealth, CurrentEnemies[i].Level);
            tempEntity.BattleVisual = tempBattleVisual;


            //�����˵�ʵ����ӵ����˺����н�ɫ�Ķ�����
            allBattlers.Add(tempEntity);
            EnemyBattlers.Add(tempEntity);

        }


    }
    public void ShowBattleMenu()//��ʾ��ɫ��ս���˵�
    {
        actionText.text = PlayerBattlers[currentPlayer].Name + ACTION_MESSAGE;
        battleMenu.SetActive(true);
    }

    public void ShowEnemySelectionMenu()//����ѡ��˵�
    {
        battleMenu.SetActive(false);
        SetEnemySelectionButtons();
        enemySelectionMenu.SetActive(true);//���˵�����Ϊ�״̬
    }

    private void SetEnemySelectionButtons()
    {
        //����ʱֻ��һ��Ұ��ʱ,�������еİ�ť
        for(int i = 0; i < enemySelectionButtons.Length; i++)
        {
//����ѡ��˵�������4������,����ǰ������û��4��,�������ڵĵ���ѡ�����ص�
            enemySelectionButtons[i].SetActive(false);//�رհ�ť�Ļ״̬

        }

        for(int j = 0; j < EnemyBattlers.Count; j++)
        {
            enemySelectionButtons[j].SetActive(true);//����ǰ���ڵĵ��˶�Ӧ��ѡ�ť����
            //��ȡ����ѡ�ť���ı�,���ı���Ϊ���������
            enemySelectionButtons[j].GetComponentInChildren<TextMeshProUGUI>().text = EnemyBattlers[j].Name;
        }
    }
    public void SelectEnemy(int currentEnemy)
    {
        //��ȷ����Ա�Ĺ���Ŀ��
        BattleEntities currentPlayerEntity = PlayerBattlers[currentPlayer];//��ȡ��ǰ���ʵ��
        currentPlayerEntity.SetTarget(allBattlers.IndexOf(EnemyBattlers[currentEnemy]));//ȷ�����Ҫ������Ŀ�����
        currentPlayerEntity.BattleAction = BattleEntities.Action.Attack;

        currentPlayer++;//ѡ�������ҵ�����+1
        if (currentPlayer >= PlayerBattlers.Count)//��������Ҷ�ѡ���������,��ʼս��
        {
            //��ʼս��
            StartCoroutine(BattleRoutine());//����BattleRoutineЭ��
            
        }
        else
        {
            enemySelectionMenu.SetActive(false);
            ShowBattleMenu();//��ʾս���˵�
        }

    }
    private void AttackAction(BattleEntities currAttacker,BattleEntities currTarget)
    {
        //��ȡ����,Ȼ�󲥷Ź�������,��������˺�,����UI
        int damage = currAttacker.Strength;
        //int damage = 20;
        currAttacker.BattleVisual.PlayAttackAnimation();//���Ź�������
        //���µ�ǰĿ���Ѫ��
        currTarget.CurrHealth -= damage;
         currTarget.BattleVisual.PlayHitAnimation();//����Ŀ���ܻ�����
        currTarget.UpdateUI();//����Ѫ��UI
        bottomText.text = string.Format("{0} attacks {1} for {2} damage",currAttacker.Name,currTarget.Name,damage);

        SaveHealth();//���浱ǰ����ֵ
    }

    private int GetRandomPartyMember()//�������ѡ��Ŀ��
    {
        List<int> partyMembers = new List<int>();//�洢�����ŵ��б�
        for(int i = 0; i < allBattlers.Count; i++)
        {
            if (allBattlers[i].IsPlayer == true && allBattlers[i].CurrHealth>0)//����ǰ��ɫ�������
            {
                partyMembers.Add(i);//�������Ŵ洢���б���
            }
        }
        return partyMembers[Random.Range(0,partyMembers.Count)];//���б����������һ�����˵�ѡ��Ŀ��
    }

    private int GetRandomEnemy()//�����ȡһ����������
    {
        List<int> enemies = new List<int>();//��ȡ����������е��б�
        for (int i = 0; i < allBattlers.Count; i++)
        {
            if (allBattlers[i].IsPlayer == false && allBattlers[i].CurrHealth > 0)//����ǰ��ɫ���������
            {
                enemies.Add(i);//��ӵ����������б���
            }
        }
        return enemies[Random.Range(0, enemies.Count)];

    }

    private void SaveHealth()//����ÿλ��ɫ�ĵ�ǰ����ֵ
    {
        for(int i = 0; i < PlayerBattlers.Count; i++)
        {
            partyManager.SaveHealth(i, PlayerBattlers[i].CurrHealth);
        }
    }
 
    private void DetermineBattleOrde()//�������н�ɫ�ĳ���˳��
    {
        //�����н�ɫ������Ȩ��������
        allBattlers.Sort((bi1,bi2)=>-bi1.Initiative.CompareTo(bi2.Initiative));
    }

    public void SelectRunAction()//ѡ�����ܲ���
    {
        State = BattleState.Selection;
        BattleEntities currentPlayerEntity = PlayerBattlers[currentPlayer];//��ȡ��ǰ���ʵ��
    
        currentPlayerEntity.BattleAction = BattleEntities.Action.Run;
        battleMenu.SetActive(false);
        currentPlayer++;//ѡ�������ҵ�����+1
        if (currentPlayer >= PlayerBattlers.Count)//��������Ҷ�ѡ���������,��ʼս��
        {
            //��ʼս��
            StartCoroutine(BattleRoutine());//����BattleRoutineЭ��

        }
        else
        {
            enemySelectionMenu.SetActive(false);
            ShowBattleMenu();//��ʾս���˵�
        }
    }

    private IEnumerator RunRoutine()//��������
    {
        if (State == BattleState.Battle)
        {
//���һ��1��100֮�����,�����������10,�������ܳɹ�,���˳�ս��
            if (Random.Range(1,101)>=RUN_CHANCE)
            {
                //����ʾ���ܳɹ��ı�
                bottomText.text = SUCCESFULLY_RAN_MESSAGE;
                //��״̬��Ϊ����
                State = BattleState.Run;
                allBattlers.Clear();//������еĽ�ɫ�б�

                yield return new WaitForSeconds(TURN_DURATION);
                SceneManager.LoadScene(OVERWORLD_SCENE);//�л�Ϊ������
                yield break;

            }
            else
            {
                //����ʧ���ı�
                bottomText.text = UNSUCCESFULLY_RAN_MESSAGE;
                yield return new WaitForSeconds(TURN_DURATION);
            }
        }
    }
}
[System.Serializable]
public class BattleEntities//ս��ʵ��
{
    public enum Action { Attack,Run }
    public Action BattleAction;

    public string Name;
    public int CurrHealth;
    public int MaxHealth;
    public int Level;
    public int Strength;
    public int Initiative;//����Ȩ
    public bool IsPlayer;//�Ƿ�Ϊ���
    public BattleVisual BattleVisual;//����һ������ս���Ӿ�Ч��
    public int Target;//ѡ��Ŀ��

    //����ɫ��Ϣ��ֵ��ս��ϵͳ��
    public void SetEntityValues(string name,int currHealth,int maxHealth,int initiative,int strength,int level,bool isPlayer)
    {
        Name = name;
        CurrHealth = currHealth;
        MaxHealth = maxHealth;
        Initiative = initiative;
        Strength = strength;
        Level = level;
        IsPlayer = isPlayer;
    }

    public void SetTarget(int target)
    {
        Target = target;
    }

    public void UpdateUI()//UI����
    {
        BattleVisual.ChangeHealth(CurrHealth);
    }
}