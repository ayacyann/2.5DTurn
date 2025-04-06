using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BasePanel : MonoBehaviour
    {
            protected bool isRemove = false;//�����Ƿ��Ѿ��ر�
            protected new string name;
            public virtual  void OpenPanel(string name)//�򿪽���
        {
            this.name = name;
            gameObject.SetActive(true);
        }

        public virtual void ClosePanel()//�رս���
        {
            isRemove = true;
            gameObject.SetActive(false);//�ر�
            Destroy(gameObject);

            //�Ƴ�����,��ʾ����û�д�
            if (UIManager.Instance.panelDict.ContainsKey(name))
            {
                UIManager.Instance.panelDict.Remove(name);

            }

        }
    }
