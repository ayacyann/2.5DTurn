using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadManager 
{

    private static readonly string root = Application.persistentDataPath + "/savedata";

    private static void CheckRoot()
    {
        if (!IsExist())
            Directory.CreateDirectory(root);
    }

    public static bool IsExist()
    {
        return Directory.Exists(root);
    }

    public static void SaveBinaryFile<T>(string fileName, T savedObject)
    {
        CheckRoot();
        string filePath = root + "/" + fileName;
        FileStream fs;
        if (File.Exists(filePath))//若路径存在则读取,否则创建这个目录
            fs = File.Open(root + "/" + fileName, FileMode.Open);
        else
            fs = File.Create(root + "/" + fileName);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs,savedObject);
        fs.Close();
    }

    public static object LoadBinaryFile(string fileName)
    {
        if (!File.Exists(root + "/" + fileName))
            return null;
        FileStream sw = File.Open(root + "/" + fileName,FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        object obj = bf.Deserialize(sw);
        sw.Close();
        sw.Dispose();
        return obj;
    }

    public static void ClearSaveFile()
    {
        if (Directory.Exists(root))
        {
            Directory.Delete(root, true);
        }
    }
}
