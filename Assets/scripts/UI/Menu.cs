using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {
    
    public void NewGame()
    {
        SceneManager.LoadScene("Level-1");    
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void Levels()
    {
         transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void Back()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
