using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="CaiDan/PackageTable",fileName ="packageTable")]
public class BackpackTable : ScriptableObject
{
    public List<PackageTableItem> DataList = new List<PackageTableItem>();
    
}

[System.Serializable]
//�������ߵ������ļ�
public class PackageTableItem
{
    //���ߵľ�̬����
    public int id;//��Ʒid
    public int type;//��Ʒ����
   // public int star;

    public string name;//��������

    public string description;//������
    public string skillDescription;//��ϸ����

    public string imagePath;//ͼƬ·��
}
