using UnityEngine;

public class Login : MonoBehaviour
{
    public Animator Animator;
    public void StartGame(string Scenes)
    {
        LoaderManager.LoadScene(Scenes); // 调用LoaderManager加载场景
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        Animator.SetTrigger("Active");
    }
}
