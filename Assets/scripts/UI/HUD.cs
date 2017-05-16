using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HUD : MonoBehaviour {
    private UnityEngine.UI.Image icon, circle;
    private Image[] lights;
    private Animator animator;
    void Awake()
    {
        circle = transform.GetChild(1).GetComponent<Image>();
        icon = transform.GetChild(4).GetComponent<Image>();
        lights = transform.GetChild(3).GetComponentsInChildren<Image>();
        animator = GetComponent<Animator>();
    }

    // main function of HUD
    public void AbilityChanged(Player player)
    {
        
        IconChange(player);
        ColorChange(player);
        CountChange(player);
    }

    private void ColorChange(Player player)
    {
        float[] color = new float[4];
        if (player.abilities.Count != 0)
        {
            if (player.abilities[0].abilitytype == AbilityType.Fuel)
            {
                color = new float[] { 1, 0.674f, 0.211f, 1 };
                animator.SetBool("Fuel", true);
            }
            else if (player.abilities[0].abilitytype == AbilityType.Jump)
                color = new float[] { 0, 0.941f, 0.654f, 1 };
            else if (player.abilities[0].abilitytype == AbilityType.Key)
            {
                animator.SetBool("Fuel", false);
                color = new float[] { 1, 1, 1, 1 };
            }
            
        }
        else
        {
            color[0] = 0;
            color[1] = 0;
            color[2] = 0;
            color[3] = 0;
            animator.SetBool("Fuel", false);
        }
        icon.color = new Color(color[0], color[1], color[2], color[3]);
        for (int i = 0; i < 4; i++)
        {
            lights[i].color = new Color(color[0], color[1], color[2], color[3]);
        }
    }

    private void IconChange(Player player)
    {
        string path = "Containers\\Icons\\New\\";
        if (player.abilities.Count != 0)
        {
            if (player.abilities[0].abilitytype == AbilityType.Fuel)
                path += "Fuel Off";
            else if (player.abilities[0].abilitytype == AbilityType.Jump)
                path += "Jump";
            else if (player.abilities[0].abilitytype == AbilityType.Key)
                path += "Key";
        }
        else
            path += "";
        icon.sprite = (Sprite)Resources.Load(path, typeof(Sprite));
        icon.SetNativeSize();

    }

    // Change Number
    private void CountChange(Player player)
    {

        for (int i = 0; i < 4; i++)
        {
            lights[i].enabled = false;
        }
        for (int i = 0; i < player.abilities.Count; i++)
        {
            lights[i].enabled = true;
        }
    }
}
