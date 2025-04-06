using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyInfo[] allEnemies;//�������г�Ա������
    [SerializeField] private List<Enemy> currentEnemies;//�����б�

    //���������������ʲô?
    private static GameObject instance;//�þ�̬����ȷ��һ��������һ��ֻ����һ������
    private const float LEVEL_MODIFIER = 0.5f;

    private void Awake()
    {
        
        if (instance != null)//���ʵ����Ϊ��
        {
            Destroy(this.gameObject);//�����������
        }
        else
        {
            instance = this.gameObject;//���򽫸ñ�������ʵ��
        }
//������Ϊ������ȫû�б�Ҫ������״,Ӧ��ÿ��ս���������������һ�ε�������

        DontDestroyOnLoad(gameObject);//��֤�����ڳ����л�ʱ��������
        /**/

    }
    //����������ɵ���
    public void GenerateEnemiesByEncounter(Encounter[] encounters,int maxNumEnemies)
    {
        currentEnemies.Clear();//��������б�
        int numEnemies=Random.Range(1,maxNumEnemies+1);//������˵�����,��1~3֮��

        for(int i = 0; i < numEnemies; i++)
        {
            //����ʱ�����洢�������
            Encounter tempEncounter = encounters[Random.Range(0, encounters.Length)];//�����������
            //�������ȼ�
            int level = Random.Range(tempEncounter.LevelMin, tempEncounter.LevelMax+1);
            //���ɹ���
            GeneratEnemyByName(tempEncounter.Enemy.EnemyName, level);
        }
        

    }
    private void GeneratEnemyByName(string enemyName,int level)
    {
        for(int i = 0; i < allEnemies.Length; i++)
        {
            if (enemyName == allEnemies[i].EnemyName)//���Կ���ȥ��,���岻�����ж�����
            {
                Enemy newEnemy = new Enemy();
                //������Ϣ
                newEnemy.EnemyName = allEnemies[i].EnemyName;
                newEnemy.Level = level;
//����ȼ��ĳɳ�����,����ÿ��һ����������ֵΪ:����*levelModifier
                float levelModifier = (LEVEL_MODIFIER * newEnemy.Level);
                //�µ��˵��������=�ɱ�д�ű��Ķ����������ֵ
                newEnemy.MaxHealth = Mathf.RoundToInt(allEnemies[i].BaseHealth+(allEnemies[i].BaseHealth*levelModifier));
                newEnemy.CurrHealth = newEnemy.MaxHealth;
                newEnemy.Strength = Mathf.RoundToInt(allEnemies[i].BaseStr+(allEnemies[i].BaseStr*levelModifier));
                newEnemy.Initiative = Mathf.RoundToInt(allEnemies[i].BaseInitiative + (allEnemies[i].BaseInitiative * levelModifier));
                //�����Ӿ�Ԥ�Ƽ�
                newEnemy.EnemyVisualPrefab = allEnemies[i].EnemyVisualPrefab;

                currentEnemies.Add(newEnemy);

            }
        }
    }

    public List<Enemy> GetCurrentEnemies()
    {
        return currentEnemies;
    }
}

[System.Serializable]
public class Enemy
{
    public string EnemyName;
    public int Level;
    public int CurrHealth;
    public int MaxHealth;
    public int Strength;
    public int Initiative;
    public GameObject EnemyVisualPrefab;

}