using UnityEngine;
using System.Collections;
public class Get : MonoBehaviour {

    public GameObject hud { get; set; }
    public GameObject inMenu;
    public GameObject drainUI;
    private bool menu_show;
	// Use this for initialization

    public void inMenu_Show()
    {
        menu_show = !menu_show;
        inMenu.SetActive(menu_show);
    }

    public void DrainShow()
    {
        try {
            drainUI.SetActive(true);
        }
        catch
        {
            Debug.Log("drain animation not set yet");
        }
    }
}
