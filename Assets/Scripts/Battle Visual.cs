using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleVisual : MonoBehaviour
{
    [SerializeField] private Slider healthBar;//Ѫ��
    [SerializeField] private TextMeshProUGUI levelText;//�ȼ��ı�

    private int currHealth;//��ǰѪ��
    private int maxHealth;//���Ѫ��
    private int level;//�ȼ�
    private Animator anim;

    //һЩ���õĳ���
    private const string LEVEL_ABB = "Lv ";
    private const string IS_ATTACK_PARAM = "IsAttack";
    private const string IS_HIT_PARAM = "IsHit";
    private const string IS_DEAD_PARAM = "IsDead";

    // Start is called before the first frame update
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        
    }

    //���½�ɫ��Ϣ
    public void SetStartingValues(int currHealth,int maxHealth,int level)
    {
        //�Խ�ɫ���ݽ��г�ʼ��
        this.currHealth = currHealth;
        this.maxHealth = maxHealth;
        this.level = level;

        levelText.text = LEVEL_ABB + this.level.ToString();//����ɫ��ǰ�ȼ�д���ı���

        UpdataHealthBar();

    }

    public void ChangeHealth(int currHealth)
    {
        this.currHealth = currHealth;//���µ�ǰѪ��
        //����ǰ����ֵΪ0,������������
        if (this.currHealth <= 0)
        {
            PlayDeadAnimation();
            Destroy(gameObject,1f);//���ٽ�ɫԤ����
        }
        //����״̬��
        UpdataHealthBar();
    }

    //���½�ɫ״̬
    public void UpdataHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currHealth;

        //���µ�ǰѪ��
        //ChangeHealth(currHealth);
    }

    public void PlayAttackAnimation()//��������������
    {
        anim.SetTrigger(IS_ATTACK_PARAM);
    }
    public void PlayHitAnimation()//�ܻ�����������
    {
        anim.SetTrigger(IS_HIT_PARAM);//Ī�����붪ʧ����
    }
    public void PlayDeadAnimation()//��������������
    {
        anim.SetTrigger(IS_DEAD_PARAM);
    }
}
