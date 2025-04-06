using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��Դ�˵�
[CreateAssetMenu(menuName="New Party Member")]
public class PartyMemberInfo : ScriptableObject
{
    public string MemberName;
    public int StartingLevel;
    public int BaseHealth;
    public int BaseStr;
    public int BaseInitiative;
    public GameObject MemberBattleVisualPrefab;//ս����ʾ����
    public GameObject MemberOverworldVisualPrefab;//�����糡����Ҫչʾ������
}
