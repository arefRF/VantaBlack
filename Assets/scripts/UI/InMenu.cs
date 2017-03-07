using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class InMenu : MonoBehaviour {
    private int button;
    private APIInput api;

    void Start()
    {
        api = Starter.GetEngine().apiinput;
    }
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void Resume()
    {
        GameObject.Find("UI").GetComponent<Get>().inMenu_Show();
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Undo()
    {
        api.UndoPressed();
        GameObject.Find("UI").GetComponent<Get>().inMenu_Show();
    }
	
}
