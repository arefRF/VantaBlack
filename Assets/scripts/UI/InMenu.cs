using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class InMenu : MonoBehaviour {
    private int button;
	// Use this for initialization
	void Start () {
        button = 0;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        ColorBlock cb = GameObject.Find("inMenuPanel").transform.GetChild(button).GetComponent<Button>().colors;
        cb.normalColor = Color.red;
        GameObject.Find("inMenuPanel").transform.GetChild(button).GetComponent<Button>().colors = cb;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void click()
    {
        if (gameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }


	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {     
           if (gameObject.transform.GetChild(0).gameObject.activeSelf)
           {
              if (button <3)
              {
                    
                    button++;
                    ColorBlock cb = GameObject.Find("inMenuPanel").transform.GetChild(button).GetComponent<Button>().colors;
                    cb.normalColor = Color.red;
                    GameObject.Find("inMenuPanel").transform.GetChild(button).GetComponent<Button>().colors = cb;
                    if (button > 0)
                    {
                        int temp = button - 1;
                        ColorBlock cb2 = GameObject.Find("inMenuPanel").transform.GetChild(temp).GetComponent<Button>().colors;
                        cb2.normalColor = Color.white;
                        GameObject.Find("inMenuPanel").transform.GetChild(temp).GetComponent<Button>().colors = cb2;
                    }
                }  
            }        
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gameObject.transform.GetChild(0).gameObject.activeSelf)
            {
                if (button >0)
                {
                    button--;
                    ColorBlock cb = GameObject.Find("inMenuPanel").transform.GetChild(button).GetComponent<Button>().colors;
                    cb.normalColor = Color.red;
                    GameObject.Find("inMenuPanel").transform.GetChild(button).GetComponent<Button>().colors = cb;
                    cb.normalColor = Color.white;
                    int temp = button+1;
                    GameObject.Find("inMenuPanel").transform.GetChild(temp).GetComponent<Button>().colors = cb;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (gameObject.transform.GetChild(0).gameObject.activeSelf)
            {
                if (button == 0)
                    Controls();
                else if (button == 1)
                    LoadCheckPoint();
                else if (button == 2)
                    Reset_Game();
                else if (button == 3)
                    Quit();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameObject.transform.GetChild(0).gameObject.activeSelf)
            {
                if (gameObject.transform.GetChild(1).gameObject.activeSelf)
                {
                    Back_to_Menu();
                }
                else
                    click();
            }
            else 
                click();
        }

	}

    public void Quit()
    {
        Application.Quit();
    }

    private void LoadCheckPoint()
    {

    }

    private void Reset_Game()
    {
        
    }

    public void Controls()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void Action_End()
    {

    }
    public void Back_to_Menu()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }
}
