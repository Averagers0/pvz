using UnityEngine;
using UnityEngine.SceneManagement;  // 切换场景需要引入

public class MainMenu : MonoBehaviour
{
    // 点击开始游戏按钮
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // 加载游戏场景
    }

    // 点击退出按钮
    public void ExitGame()
    {
        Application.Quit(); // 退出游戏（仅打包后有效）
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
