using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterSystem : MonoBehaviour
{
    [SerializeField] private Encounter[] enemiesInScene;//�����еĵ���
    [SerializeField] private int maxNumEnemies;//���ɵĵ�������
    // Start is called before the first frame update

    private EnemyManager enemyManager;

    void Start()
    {
        //��ȡ���˹��������
        enemyManager = GameObject.FindAnyObjectByType<EnemyManager>();
        //ͨ���������ɵ���
        enemyManager.GenerateEnemiesByEncounter(enemiesInScene,maxNumEnemies);


    }


}

[System.Serializable]
public class Encounter
{
    public EnemyInfo Enemy;//��������
    public int LevelMin;//��͵ȼ�
    public int LevelMax;//��ߵȼ�

}
