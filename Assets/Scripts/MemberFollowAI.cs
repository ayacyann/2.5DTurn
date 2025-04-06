using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberFollowAI : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private int speed;

    private float followDist;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Vector3 scale;
    private const string IS_WALK_PARAM = "IsWalk";
    // Start is called before the first frame update
    private void Awake()
    {
        scale = transform.localScale;
    }

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        //���ø���Ŀ��
        followTarget = GameObject.FindFirstObjectByType<PControl>().transform;

    }

    // Update is called once per frame
    void FixedUpdate()//�̶�����
    {
        //�жϽ�ɫ֮��ľ����Ƿ񳬹��������
        if (Vector3.Distance(transform.position, followTarget.position) > followDist)
        {
            //��Χ����,�������ɫ���ɫ̫Զ,ֱ�ӽ������ɫˢ�µ��������
            if (Vector3.Distance(transform.position, followTarget.position) > 3*followDist)
            {
                Vector3 vector = new Vector3(1, 0, 0);
                transform.position = followTarget.position - vector;
            }
            else
            {
                //���������������
                anim.SetBool(IS_WALK_PARAM, true);//ִ�����߶���
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, followTarget.position, step);//����Ҫ��
            }
    

            if (followTarget.position.x - transform.position.x < 0)
            {
                spriteRenderer.transform.localScale=new Vector3(-scale.x,scale.y,scale.z);
            }
            else
            {
                spriteRenderer.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
            }
        }

        else
        {
            //����������״̬
            anim.SetBool(IS_WALK_PARAM, false);//�ر����߶���
        }
        
    }

    public void SetFollowDistance(float FollowDistance)//��ȡ�������
    {
        followDist = FollowDistance;
    }
}
