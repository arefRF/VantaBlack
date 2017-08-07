using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {
    private Animator animator;
    public GameObject levels;
    public GameObject menutexts;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void NewGame()
    {
        GameManager.manager.NewGame();
    }

    public void Controller()
    {
        GameManager.manager.controller = true;
            transform.GetChild(1).GetChild(0).GetChild(3).gameObject.SetActive(false);
            transform.GetChild(1).GetChild(0).GetChild(5).gameObject.SetActive(true);

    }

    public void Keyboard()
    {
        GameManager.manager.controller = false;
        transform.GetChild(1).GetChild(0).GetChild(3).gameObject.SetActive(true);
        transform.GetChild(1).GetChild(0).GetChild(5).gameObject.SetActive(false);
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
        menutexts.SetActive(false);
        levels.gameObject.SetActive(true);
    }

    public void Disable(int child_number) {
        GameObject.Find("Level Sections").transform.GetChild(child_number).gameObject.SetActive(false);
      
    }

    public void LevelSections(int child_number)
    {
        levels.SetActive(false);
        GameObject.Find("Level Sections").transform.GetChild(child_number).gameObject.SetActive(true);
    }

    public void BacktoLevels(int child_number)
    {
        GameObject.Find("Level Sections").transform.GetChild(child_number).gameObject.SetActive(false);
        levels.SetActive(true);
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
