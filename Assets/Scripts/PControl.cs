using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//��������

public class PControl : MonoBehaviour
{
    //���л�,��˽�б���Ҳ�����������ϵ���
    [SerializeField] private int speed;
    [SerializeField] private Animator anim;
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] private LayerMask GressLayer;//�ݵ�ͼ��
    [SerializeField] private int stepsInGrass;//�����ڲݵ������˶��ٲ�
    [SerializeField] private int minStepsToEncounter;//��С�����
    [SerializeField] private int maxStepsToEncounter;//��������


    //˽�˲������ؼ�
    private PlayerControls playerControls;
    private Rigidbody rb;
    private Vector3 movement;
    private Vector3 scale;
    private bool movingInGrass;//�Ƿ��ڲݵ����ƶ�
    private float stepTimer;//��ֵ
    private int stepToEncounter;//�����
    private PartyManager partyManager;

    private const string IS_WALK_PARAM="IsWalk";//����isWalk����
    private const string BATTLE_SCENE = "BattleScene";//ս������
    private const float TIME_PER_STEP = 0.5f;//�ڲݴ�������ʱ��Ҫ����ʱ��
    private const string AI_NAME = "NPC";

    
    private void Awake()//���Ѻ���
    {
        playerControls = new PlayerControls();
        CalculateStepsToNextEncounter();
    }
    private void OnEnable()
    {
        playerControls.Enable();//������ҿ�����
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();//��ȡ�������
        partyManager = GameObject.FindFirstObjectByType<PartyManager>();

        //���������һ��λ��,��ȥ�ƶ����
        if(partyManager.GetPosition() != Vector3.zero)//��ս����ɫ���겻Ϊ��
        {
            //��ս��ʱ��ɫ�����긳����ʵ�н�ɫ������
            transform.position = partyManager.GetPosition();
        }
       // GameObject obj = GameObject.FindWithTag(AI_NAME);
        //Debug.Log("AI����:"+obj.transform.position);


    }
    // Update is called once per frame
    void Update()//ÿִ֡��һ��
    {
        //�����Ҹ�
        float x = playerControls.Player.Move.ReadValue<Vector2>().x;
        float z = playerControls.Player.Move.ReadValue<Vector2>().y;

        movement = new Vector3(x, 0, z).normalized;//��һ��
        //���ö��� ���������⣬����������
        anim.SetBool(IS_WALK_PARAM, movement != Vector3.zero);//����ά���겻Ϊ0ʱ,�������߶���

        //��ɫ��ת
        if (x!=0 && x < 0)
        {
            playerSprite.transform.localScale=new Vector3(-scale.x,scale.y,scale.z);//flipX:x�ᷭת
        }
        if(x!=0 && x >= 0)
        {
            playerSprite.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        }
    }

//FixeUpdate:���ڴ���������ƶ�,ʱ����ص��߼�,�̶�����ʱ��Ϊ0.02s
    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);
        //�������������е���ײ��
        Collider[] colliders = Physics.OverlapSphere(transform.position,1, GressLayer);
        //����ƶ��������������������������ײ
        movingInGrass = colliders.Length!=0 && movement!=Vector3.zero;

        if (movingInGrass)//�����ɫ�ڲݵ����ƶ�,���Ӳ�����ʱ��
        {
            stepTimer+=Time.fixedDeltaTime;//������ʱ��
            //���������ʱ������ָ������ֵ,��ӽ�ɫ���������ò�����ʱ��
            if (stepTimer>TIME_PER_STEP)
            {
                stepsInGrass++;//��ɫ�������
                stepTimer = 0;

                if (stepsInGrass>= stepToEncounter)//����ڲݴ������ߵĲ������������Ĳ���
                {
                    partyManager.SetPosition(transform.position);//�̶���ɫ��ǰ��ս������
                    SceneManager.LoadScene(BATTLE_SCENE);//����ս������
                }

            }
        }
    }

    //����㷶Χ����,���ڷ��䲽���Լ�������С����֮���ֵ�������������
    private void CalculateStepsToNextEncounter()
    {
        //�����,��ʾ����ս�Ĳ�����������������С������ķ�Χ�����һ��ֵ
        stepToEncounter = Random.Range(minStepsToEncounter,maxStepsToEncounter);
        Debug.Log(stepToEncounter);
    }

    public void SetOverwordVisuals(Animator animator,SpriteRenderer spriteRenderer,Vector3 playerScale)
    {
        anim = animator;
        playerSprite = spriteRenderer;
        scale = playerScale;
    }
}
