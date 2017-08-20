using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerGraphicsV5 : PlayerGraphics
{
    public override void MoveToBranch(Direction dir)
    {
        StopAllCoroutines();
        StartCoroutine(Simple_Move(player.position, 0.3f, true));
    }

    public override void ChangeColor()
    {
            ChangeSprites();
    }
    private void ChangeOffSprites()
    {

    }
    private void ChangeSprites()
    {
        Color color = Ability_Color(player.abilities);
        color = new Color(color.r, color.g, color.b, 1);
        SpriteRenderer brain = Toolkit.GetObjectInChild(this.gameObject, "Brain").GetComponent<SpriteRenderer>();
        SpriteRenderer right_hand = Toolkit.GetObjectInChild(this.gameObject, "Arm 2 Right").GetComponent<SpriteRenderer>();
        SpriteRenderer left_hand = Toolkit.GetObjectInChild(this.gameObject, "Arm 2 Left").GetComponent<SpriteRenderer>();
        string base_path =  "Player\\Player Version 5\\";
        string on = "";
        if (player.abilities.Count != 0)
            on = " On";
        brain.sprite = (Sprite)Resources.Load(base_path+"Brain"+on,typeof (Sprite));
        brain.color = color;
        right_hand.sprite = (Sprite)Resources.Load(base_path + "Arm 2 Right" + on, typeof(Sprite));
        right_hand.color = color;
        left_hand.sprite = (Sprite)Resources.Load(base_path+ "Arm 2 Left"+ on,typeof(Sprite));
        left_hand.color = color;
    }
}
