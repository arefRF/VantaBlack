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
        textPanel.GetChild(0).gameObject.SetActive(false);
        textPanel.GetChild(1).gameObject.SetActive(true);
    }
    public void Back()
    {
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
