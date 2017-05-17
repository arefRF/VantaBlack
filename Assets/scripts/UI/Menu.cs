using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void NewGame()
    {
        GameManager.manager.NewGame();
    }

    public void Resume()
    {
        GameManager.manager.Continue();
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void Levels()
    {
        animator.SetBool("Levels", true);
    }
    public void Controls()
    {
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Show Continue not new game
    public void Continue()
    {
        Transform textPanel = transform.GetChild(1);
        textPanel.GetChild(0).GetChild(0).gameObject.SetActive(false);
        textPanel.GetChild(0).GetChild(1).gameObject.SetActive(true);
    }
    public void Back()
    {
        animator.SetBool("Levels", false);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
