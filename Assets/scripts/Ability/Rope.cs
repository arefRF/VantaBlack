using UnityEngine;
using System.Collections;

public class Rope : Ability{
    private Player player;
    Vector2 endPos;
    private APIGraphic api_graphic;
    private APIUnit api_unit;
    public Rope()
    {
        abilitytype = AbilityType.Rope;
    }

    private void SetVariables()
    {
        api_graphic = Starter.GetEngine().apigraphic;
        api_unit = Starter.GetEngine().apiunit;
    }

    public void Action(Player player)
    {
        if (api_graphic == null)
            SetVariables();
        this.player = player;
        endPos = player.position;
        Debug.Log(RopeMono.instance);
        RopeMono.instance.StartCoroutine(RopeActivate(5));
    }

    private IEnumerator RopeActivate(float time)
    {
        yield return new WaitForSeconds(time);

        // after it's time take player back
        api_unit.RemoveFromDatabase(player);
        player.position = endPos;
        api_unit.AddToDatabase(player);
        api_graphic.Teleport(player, player.position);
    }
}
