using UnityEngine;
using UnityEngine.UI;

public class MenuButtonEvent : MonoBehaviour
{
    private void Start()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(()=>LoadSceneManager.Instance.StartGame());
        buttons[1].onClick.AddListener(()=>LoadSceneManager.Instance.ContinueGame());
        buttons[2].onClick.AddListener(()=>LoadSceneManager.Instance.QuitGame());
    }
}
