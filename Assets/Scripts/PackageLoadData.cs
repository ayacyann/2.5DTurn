using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//������������json����ʽ�洢�ڱ���
public class PackageLoadData
{
    private static PackageLoadData _instance;

    public static PackageLoadData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PackageLoadData();
            }
            return _instance;
        }
    }

    public List<packageLoadItem> items;//���浱ǰ��Ʒ���еĶ�̬��Ϣ


    //����û�д洢�ڱ��أ�
    public void SavePackage()//�洢����
    {
        //�ѱ����Ϣ���л�Ϊ�ַ���
        string inventoryJson = JsonUtility.ToJson(this);
        //���ı����ݴ洢�����ص��ļ���
        PlayerPrefs.SetString(" PackageLoadData",inventoryJson);
        PlayerPrefs.Save();

    }

    public  List<packageLoadItem> LoadPackage()//��ȡ����
    {
        //���жϻ��������Ƿ񻹴���,������ھ�ֱ�ӷ��ش洢��Ϣ
        if (items != null)//����֮ǰ��ȡ���ı���Ϣ
        {
            return items;
        }
        //�����ڱ����ļ��ж�ȡ
        if (PlayerPrefs.HasKey("PackageLoadData"))
        {
            //�ѱ����ļ���ȡ���ڴ���ʹ���Ϊ�ַ���
            string inventoryJson = PlayerPrefs.GetString("PackageLoadData");
            //ʹ��JsonUtility�����л�
            PackageLoadData packageLoadData = JsonUtility.FromJson<PackageLoadData>(inventoryJson);
            items = packageLoadData.items;
            //��ʵ���ǰ��ļ�����ַ���Ȼ���ٱ�����������
            return items;
        
        }
        else
        {
            items = new List<packageLoadItem>();
            return items;
        }
    }



}


[System.Serializable]
public class packageLoadItem
{
    public string uid;//Ψһ��ʶ��
    public int id;//��ʾ��������
    public int num;//��������
    public int level;//���ߵȼ�
    public bool isNew;//�Ƿ�Ϊ�»�õ���

    public override string ToString()
    {
        return string.Format("id:{0}, num:{1}",id,num);
    }
}


