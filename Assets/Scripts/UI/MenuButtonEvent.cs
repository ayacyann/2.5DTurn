using UnityEngine;
using UnityEngine.UI;

public class MenuButtonEvent : MonoBehaviour
{
    private void Start()
    {
        //°´Å¥¶¯Ì¬°ó¶¨
        Button[] buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(()=>LoadSceneManager.Instance.StartGame());
        buttons[2].onClick.AddListener(()=>LoadSceneManager.Instance.QuitGame());
        if (SaveLoadManager.IsExist())
        {
            buttons[1].onClick.AddListener(()=>LoadSceneManager.Instance.ContinueGame());
        }
        else
        {
            buttons[1].interactable = false;
        }
    }
}
