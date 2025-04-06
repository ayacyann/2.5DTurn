using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinableCharacterScript : MonoBehaviour
{
    public PartyMemberInfo MemberToJoin;//����ӵĳ�Ա
    [SerializeField] private GameObject interactPrompt;//������ʾ

    // Start is called before the first frame update
    void Start()
    {
        CheckIfJoined();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowInteractPrompt(bool showPrompt)//��ʾ������ʾ
    {
        if (showPrompt == true)//����ʾ��ʾΪtrue
        {
            interactPrompt.SetActive(true);//��ʾUI����
        }
        else
        {
            interactPrompt.SetActive(false);
        }
    }
    public void CheckIfJoined()//��鵱ǰ��ɫ�Ƿ��Ѿ��������
    {
        //��ȡ��ǰ���г�Ա������
        List<PartyMember> currparty = GameObject.FindFirstObjectByType<PartyManager>().GetCurrentParty();
        
        for(int i = 0; i < currparty.Count; i++)
        {
            //���������ĳ�Ա�������Ա�б��еĳ�Աһ��,˵���ý�ɫ�Ѿ����
            if (currparty[i].MemberName == MemberToJoin.MemberName)
            {
                gameObject.SetActive(false);//�رոý�ɫ
            }
        }
    
    }
}
