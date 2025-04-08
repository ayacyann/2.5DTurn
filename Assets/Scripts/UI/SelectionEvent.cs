using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionEvent : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public int idx;
    private BattleEntities target;
    private BattleSystem bs;

    public void InitBattleSystem(BattleSystem battleSystem)
    {
        bs = battleSystem;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        target = bs.GetEnemyEntities()[idx];
        BattleEntities current = bs.CurrentPlayer;
        bs.ShowLine(current.battleVisual.transform, target.battleVisual.transform);
        Debug.Log(bs.CurrentPlayer.battleVisual.gameObject.name);
        Debug.Log(target.battleVisual.gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        target = null;
        bs.ShowLine(null,null);
    }
}
