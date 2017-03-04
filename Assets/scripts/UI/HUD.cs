using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HUD : MonoBehaviour {
    private UnityEngine.UI.Image icon, count;
    private Sprite[] countsprite;
    void Awake()
    {
        count = transform.GetChild(1).GetComponent<Image>();
        countsprite = new Sprite[6];
        countsprite[0] = null;
        countsprite[1] = (Sprite)Resources.Load("HUD\\Count 1", typeof(Sprite));
        countsprite[2] = (Sprite)Resources.Load("HUD\\Count 2", typeof(Sprite));
        countsprite[3] = (Sprite)Resources.Load("HUD\\Count 3", typeof(Sprite));
        countsprite[4] = (Sprite)Resources.Load("HUD\\Count 4", typeof(Sprite));
        countsprite[5] = (Sprite)Resources.Load("HUD\\Full", typeof(Sprite));
        icon = transform.GetChild(0).GetComponent<Image>();
    }
    public void AbilityChanged(Player player)
    {
        iconChange(player);
        countChange(player);
        ColorChange(player);
    }

    private void ColorChange(Player player)
    {
        float[] color = new float[4];
        if (player.abilities.Count != 0)
        {
            if (player.abilities[0].abilitytype == AbilityType.Fuel)
                color = new float[] { 0, 0.941f, 0.654f, 1 };
            else if (player.abilities[0].abilitytype == AbilityType.Jump)
                color = new float[] { 0, 0.941f, 0.654f, 1 };
            else if (player.abilities[0].abilitytype == AbilityType.Key)
                color = new float[] { 1, 1, 1, 1 };
            
        }
        else
        {
            color[0] = 0;
            color[1] = 0;
            color[2] = 0;
            color[3] = 0;
        }
        icon.color = new Color(color[0], color[1], color[2], color[3]);
        count.color = new Color(color[0], color[1], color[2], color[3]);
    }

    private void iconChange(Player player)
    {
        string path = "HUD\\";
        if (player.abilities.Count != 0)
        {
            if (player.abilities[0].abilitytype == AbilityType.Fuel)
                path += "Fuel";
            else if (player.abilities[0].abilitytype == AbilityType.Jump)
                path += "Jump";
            else if (player.abilities[0].abilitytype == AbilityType.Key)
                path += "Door";
        }
        else
            path += "";
        icon.sprite = (Sprite)Resources.Load(path, typeof(Sprite));
        
    }

    private void countChange(Player player)
    {
        if(player.abilities.Count != 0 && player.abilities[0].abilitytype == AbilityType.Key)
        {
            count.sprite = countsprite[5];
        }
        else
            count.sprite = countsprite[player.abilitycount];
    }
}
