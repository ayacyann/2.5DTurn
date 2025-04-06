using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class Pickup : MonoBehaviour
{
    private bool isShow;//�����ͣʱ�Ĵ���չʾ
    private GUIStyle style;//GUI��ʽ�ı���
    public string text;//��Ҫչʾ����Ϣ

    public enum PickupType
    {
        BloodVial,
        heart
    }
    
  
 
    private PartyManager partyManager;
    void Awake()
    {
        partyManager = GameObject.FindAnyObjectByType<PartyManager>();
        BackpackTable backpacktable = Resources.Load<BackpackTable>("DataTable/packageTable");
    }

    private void Start()
    {
        isShow = false;
        style = new GUIStyle("box");
        style.fontSize = 15;
        style.alignment = TextAnchor.MiddleLeft;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 1 ��ʾ�����Ҽ�
        {
            Debug.Log("�Ҽ�");
        }
        }


    //��������¼�����
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(isShow);
        isShow = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isShow = false;
    }

    void OnGUI()
    {
        if (isShow)
        {
            var vt = style.CalcSize(new GUIContent(text));
            GUI.backgroundColor = new Color32(255, 255, 255, 100);
            GUI.Label(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, vt.x, vt.y), text, style);
        }
    }
}
