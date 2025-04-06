using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//�����ɫ������߼�
public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject joinPopup;//��������
    [SerializeField] private TextMeshProUGUI joinPopupText;//�����ı�
    private bool infrontOfPartyMember;
    private GameObject joinableMember;
    private PlayerControls playerControls;
    private List<GameObject> overworldCharacter = new List<GameObject>();
    // Start is called before the first frame update

    private const string PARTY_JOINED_MESSAGE = "joined The Party!";
    private const string NPC_JOINABLE_TAG = "NPC Joinable";
    private void Awake()
    {
        playerControls = new PlayerControls();//��ʼ��
    }
    void Start()
    {
        playerControls.Player.Interact.performed += _ => Interct();
        SpawnOberworldMembers();
    }

    private void OnEnable()//����
    {
        playerControls.Enable();//���ò�����
    }
    private void OnDisable()//����
    {
        playerControls.Disable();//���ò�����
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void Interct()
    {
        //���ɼ����ɫ��Ϊ���ҿ��Խ���
        if(infrontOfPartyMember == true && joinableMember != null)
        {
            //���ö�Ա�������
            Debug.Log("����:"+joinableMember.GetComponent<JoinableCharacterScript>().MemberToJoin);
            MemberJoined(joinableMember.GetComponent<JoinableCharacterScript>().MemberToJoin);
            infrontOfPartyMember = false;
            joinableMember = null;

        }
    }

    private void MemberJoined(PartyMemberInfo partyMember)//��ӳ�Ա�ĺ���
    {
        //��ӳ�Ա,�����Ա�������ɶԳ�Ա��ӵ��ɶ���
        GameObject.FindFirstObjectByType<PartyManager>().AddMemberToPartyByName(partyMember.MemberName);
        joinableMember.GetComponent<JoinableCharacterScript>().CheckIfJoined();

        joinPopup.SetActive(true);//��������
        joinPopupText.text = partyMember.MemberName + PARTY_JOINED_MESSAGE;//�����ı�
        SpawnOberworldMembers();//��ӳ�Աʱ�ٴ�ˢ��������
    }

    private void SpawnOberworldMembers()//�����������Ա
    {
        for(int i = 0; i < overworldCharacter.Count; i++)
        {
            Destroy(overworldCharacter[i]);//�ݻ�������Ľ�ɫ
        }
        overworldCharacter.Clear();//����б�

        //��ȡ�����еĵ�һ����Ա
        List<PartyMember> currentParty = GameObject.FindFirstObjectByType<PartyManager>().GetCurrentParty();

        for(int i = 0; i < currentParty.Count; i++)
        {
            if (i == 0)//��һ����ԱΪ���
            {
                GameObject player = gameObject;//��ȡ���Ԥ����
                //����Ӿ�����
                GameObject playerVisual = Instantiate(currentParty[i].MemberOverworVisualPrefab, player.transform.position, Quaternion.identity);
                //Debug.Log("����:" + transform.position);

                playerVisual.transform.SetParent(player.transform);//

                player.GetComponent<PControl>().SetOverwordVisuals(playerVisual.GetComponent<Animator>(),playerVisual.GetComponent<SpriteRenderer>(),playerVisual.transform.localScale);//�����Ӿ�Ч��
                //���ø���AI
                playerVisual.GetComponent<MemberFollowAI>().enabled = false;
                overworldCharacter.Add(playerVisual);//����������ɫ
      
            }
            else//����������ԱΪ������
            {
                Vector3 posistionToSpawn = transform.position;//��ɫ����
                posistionToSpawn.x = -i;
                //��ʱ������
                GameObject tempFollower= Instantiate(currentParty[i].MemberOverworVisualPrefab, posistionToSpawn, Quaternion.identity);

                //��ȡ�������������
                tempFollower.GetComponent<MemberFollowAI>().SetFollowDistance(i);
                overworldCharacter.Add(tempFollower);

            }
        }
    }

    //��ײ��⿪��
    private void OnTriggerEnter(Collider other)
    {
        //����Ϸ�����ǩΪNPC�����ӱ�ǩ
        if (other.gameObject.tag == NPC_JOINABLE_TAG)
        {
            infrontOfPartyMember = true;
            joinableMember = other.gameObject;//����ǰ�����Ľ�ɫ����Ϊ�ɼ����Ա
            //��ȡUI������������������
            joinableMember.GetComponent<JoinableCharacterScript>().ShowInteractPrompt(true);
        }
    }

    //��ײ����˳�
    private void OnTriggerExit(Collider other)
    {
        //����Ϸ�����ǩΪNPC�����ӱ�ǩ
        if (other.gameObject.tag == NPC_JOINABLE_TAG)
        {
            infrontOfPartyMember = false;

            //��ȡUI������������������
            joinableMember.GetComponent<JoinableCharacterScript>().ShowInteractPrompt(false);
            joinableMember = null;
        }
    }
}
