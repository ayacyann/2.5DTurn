using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CMCmd
{
    [MenuItem("CMCmd/��ȡ���")]
//����Unity���Ե�Menu��̬����,���Ա�֤�༭�������˵��������ɰ�ť,��������ִ����Ӧ�߼�
    public static void ReadTable()
    {
        //��ȡ���
            BackpackTable backpacktable = Resources.Load<BackpackTable>("DataTable/packageTable");
            foreach(PackageTableItem packageItem in backpacktable.DataList)
        {
            Debug.Log(string.Format("id:{0}, name:{1}",packageItem.id, packageItem.name));
        }
     
    }


    [MenuItem("CMCmd/����������������")]
    public static void CreateLoadPackageData()
    {
        //��������
        PackageLoadData.Instance.items = new List<packageLoadItem>();
        for(int i = 0; i < 9; i++)
        {
            packageLoadItem packageLoadItem = new()
            {
                uid = Guid.NewGuid().ToString(),
                id = i,
                num = i,
                level = i,
                isNew = i % 2 == 1
            };

            PackageLoadData.Instance.items.Add(packageLoadItem);
        }
        PackageLoadData.Instance.SavePackage();
    }

    [MenuItem("CMCmd/��ȡ������������")]
    public static  void ReadLoadPackageData()
    {
        //��ȡ����
        List<packageLoadItem> readItems = PackageLoadData.Instance.LoadPackage();
        foreach (packageLoadItem item in readItems)
        {
            Debug.Log(item);
        }
    }

    [MenuItem("CMCmd/�򿪱���������")]
    public static  void OpenPackagePanel()
    {
        UIManager.Instance.OpenPanel(UIConst.BackpackPanel);
    }
}
