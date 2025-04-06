using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager
    {
        private static UIManager _instance;
        //����������·����
        private Dictionary<string, string> pathDict;
        private Dictionary<string, GameObject>prefabDict;//Ԥ�Ƽ������ֵ�
        public Dictionary<string, BasePanel> panelDict;//�Ѵ򿪽��滺���ֵ�

        private  Transform _uiRoot;
        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UIManager();
                }
                return _instance;
            }
        }

        //UI���ĸ��ڵ�
        public Transform UIRoot
        {
            get
            {
                if (_uiRoot == null)
                {
                    //����canvas����
                    _uiRoot = GameObject.Find("Canvas").transform;
                }
                return _uiRoot;
            }

        }
        private UIManager()//���ֵ���г�ʼ��
        {
            prefabDict = new Dictionary<string, GameObject>();
            panelDict = new Dictionary<string, BasePanel>();

            pathDict = new Dictionary<string, string>()
            {
                //{UIConst.BackpackCanvas,"OverworldScene/BackpackCanvas" }//�ѽ���·�����õ��ֵ���
                { UIConst.BackpackPanel,"Package/BackpackPanel"}
            };
        }


        //�򿪽�����߼�
        public   BasePanel OpenPanel(string name)
        {
            BasePanel panel = null;
            //�������Ƿ��
            if (panelDict.TryGetValue(name,out panel))
            {
                return null;//�������Ѿ���,��ֱ����������
            }

            //���·���Ƿ�������
            string path = " ";
            if (!pathDict.TryGetValue(name,out  path))
            {
                return null;//��·�����������ñ���,Ҳ���򿪸ý���
            }

            //���ؽ���Ԥ�Ƽ�
            //ʹ�û���Ԥ�Ƽ�
            GameObject panelPrefab = null;

            //����Ԥ�Ƽ�֮ǰ���ڻ����ֵ��в鿴һ��
            if(!prefabDict.TryGetValue(name, out panelPrefab))
            {
                //�������Ѿ������ع�,��ֱ���ó����ã����Ѿ����ع�,�ͷ��ڻ����ֵ���
                 string realPath = "Prefab/Panel/" + path;///������Ҫ����·��
                //
                panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
                prefabDict.Add(name,panelPrefab);

            }
            //����Ԥ�Ƽ�������uiroot��
            GameObject panelObject = GameObject.Instantiate(panelPrefab,UIRoot,false);
            panel = panelObject.GetComponent<BasePanel>();
            panelDict.Add(name, panel);//��ʾ�ý����Ѿ���

            return panel;
        }

        //�رս�����߼�
        public bool ClosePanel(string name)
        {
            //���ý����Ƿ���panelDict��
            BasePanel panel = null;
            //���汻�򿪲���Ҫ�ر���
            if (!panelDict.TryGetValue(name, out panel))
            {
                return false;
            }
            panel.ClosePanel();
            return true;

        }



    }

    public  class UIConst//�洢�˵����Ƶĳ�����
    {
        //public const string BackpackCanvas = "BackpackCanvas";//�������������
        public const string BackpackPanel = "BackpackPanel";
    }


