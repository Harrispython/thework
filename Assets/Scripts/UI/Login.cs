using UnityEngine;

public class Login : MonoBehaviour
{
    public Animator Animator;
    public void StartGame(string Scenes)
    {
        LoaderManager.LoadScene(Scenes); // ����LoaderManager���س���
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
