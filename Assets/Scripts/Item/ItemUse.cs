using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUse : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Item item = null;
    private bool isEnter = false;
    private Canvas canvas;
    public int idx;
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        if (isEnter)
        {
            // 获取鼠标的屏幕坐标
            Vector2 mousePosition = Input.mousePosition;
            // 将屏幕坐标转换为 Canvas 的局部坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, // 父 Canvas 的 RectTransform
                mousePosition, // 鼠标的屏幕坐标
                canvas.worldCamera, // Canvas 的相机（如果 Canvas 是 Screen Space - Overlay，则传入 null）
                out Vector2 localPoint // 输出的局部坐标
            );
            // 设置 UI 的位置
            RectTransform showRect = BackpackManager.Instance.showItemInfo.GetComponent<RectTransform>();
            Vector2 offset = showRect.GetChild(0).GetComponent<RectTransform>().sizeDelta / 2;
            showRect.anchoredPosition = localPoint+offset;
        }
    }

    public void SetItem(Item it)
    {
        item = it;
    }

    public void UseItem()
    {
        PlayerData playerData = BackpackManager.Instance.GetCurrentPartyMember();
        playerData.currHealth += item.health;
        if (playerData.currHealth > playerData.maxHealth)
        {
            playerData.currHealth = playerData.maxHealth;
        }
        playerData.strength += item.attack;
        playerData.speed += item.speed;
        //更新UI
        BackpackManager.Instance.UseItem(idx,this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item==null) return;
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item==null)
        {
            isEnter = false; 
            return;
        }
        isEnter = true;
        BackpackManager.Instance.currentItem = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item==null)
        {
            isEnter = false; 
            return;
        }
        isEnter = false;
        BackpackManager.Instance.currentItem = null;
    }
}
