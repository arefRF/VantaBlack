using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {
    
    public void NewGame()
    {
        SceneManager.LoadScene("Level-1");    
    }

    public void Quit()
    {
        Application.Quit();
    }
}
