using UnityEngine;
using UnityEngine.EventSystems;


//�����ø�UI���������¼�
public class UIMouseHoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{  //�ӿڣ��ֱ���������U���뿪UI����
    private bool isShow;
    public string text;
    private GUIStyle style;

    private void Start()
    {
        isShow = false;
        style = new GUIStyle("box");
        style.fontSize = 25;
        //style.normal.textColor = UnityEngine.Color.green;
        style.alignment = TextAnchor.MiddleLeft;

       
    }

    public void OnPointerEnter(PointerEventData eventData)
    {  //�����ͣ���ڸ���Ϸ������ʱ����
        Debug.Log(isShow);
        isShow = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {  //������뿪����Ϸ����ʱ����
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