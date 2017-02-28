using UnityEngine;
using System.Collections;
public class Get : MonoBehaviour {

    public GameObject hud;
    public GameObject inMenu;
    private bool menu_show;
	// Use this for initialization

    public void inMenu_Show()
    {
        menu_show = !menu_show;
        inMenu.SetActive(menu_show);
    }
}
