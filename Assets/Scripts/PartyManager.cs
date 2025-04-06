using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public  PartyMemberInfo[] allMembers;//�������г�Ա������
    public  List<PartyMember> currentParty;
    [SerializeField] private PartyMemberInfo defaultPartMember;

    private Vector3 playerPosition;//��ҵ���ά����
    private static GameObject instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this.gameObject;
            AddMemberToPartyByName(defaultPartMember.MemberName);//��ӽ�ɫ��Ϣ
        }
        DontDestroyOnLoad(gameObject);
    }
    public void AddMemberToPartyByName(string menberName)
    {
        for (int i=0;i< allMembers.Length; i++)
        {
            if (allMembers[i].MemberName == menberName)
            {
                PartyMember newPartyMember = new PartyMember();
                newPartyMember.MemberName = allMembers[i].MemberName;
                newPartyMember.Level = allMembers[i].StartingLevel;
                newPartyMember.CurrHealth = allMembers[i].BaseHealth;
                newPartyMember.MaxHealth = newPartyMember.CurrHealth;
                newPartyMember.Strength = allMembers[i].BaseStr;
                newPartyMember.Initiative = allMembers[i].BaseInitiative;
                newPartyMember.MemberBattleVisualPrefab = allMembers[i].MemberBattleVisualPrefab;
                newPartyMember.MemberOverworVisualPrefab = allMembers[i].MemberOverworldVisualPrefab;

                currentParty.Add(newPartyMember);//����³�Ա



            }
        }
    }
    public List<PartyMember> GetAliveParty()
    {
        //�洢������ҽ�ɫ���б�
        List<PartyMember> aliveParty = new List<PartyMember>();
        aliveParty = currentParty;
        for(int i = 0; i < aliveParty.Count; i++)
        {
            //����ǰ��ɫѪ��Ϊ0
            if (aliveParty[i].CurrHealth <= 0)
            {
                aliveParty.RemoveAt(i);//����ǰ��ɫ���б����Ƴ�
            }
        }
        return aliveParty;
    }

    public List<PartyMember> GetCurrentParty()
    {
        return currentParty;
    }

    //ͬ����ɫ�ĵ�ǰ״̬
    public void SaveHealth(int partyMember,int health)
    {
        currentParty[partyMember].CurrHealth = health; 
    }
    public void SetPosition(Vector3 posistion)//��ȡ��ɫ��ǰλ��
    {
        playerPosition = posistion;
    }

    public Vector3 GetPosition()
    {
        return playerPosition;//�����������
    }
}
[System.Serializable]
public class PartyMember
{
    public string MemberName;
    public int Level;
    public int CurrHealth;//��ǰѪ��
    public int MaxHealth;//���Ѫ��
    public int Strength;
    public int Initiative;
    public int CurrExp;//��ǰ����
    public int MaxExp;//�����
    public GameObject MemberBattleVisualPrefab;//������Ϸ����
    public GameObject MemberOverworVisualPrefab;//�Ӿ�Ԥ�Ƽ�
    

}