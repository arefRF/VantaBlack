using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HUD : MonoBehaviour {
    private UnityEngine.UI.Image icon;

    void Start()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
    }
    public void AbilityChanged(Player player)
    {
        iconChange(player);
        ColorChange(player);
    }

    private void ColorChange(Player player)
    {
        float[] color = new float[4];
        if (player.abilities.Count != 0)
        {
            if(player.abilities[0].abilitytype == AbilityType.Fuel)
            {
                color = new float[] { 0, 0.941f , 0.654f ,1};
            }
            else if(player.abilities[0].abilitytype == AbilityType.Jump)
                color = new float[] { 0, 0.941f, 0.654f, 1 };
            
        }
        else
        {
            color[0] = 0;
            color[1] = 0;
            color[2] = 0;
            color[3] = 0;
        }
        icon.color = new Color(color[0], color[1], color[2], color[3]);
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
        }
        else
            path += "";

        icon.sprite = (Sprite)Resources.Load(path, typeof(Sprite));
        
    }
}
