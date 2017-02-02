using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Gate : Container {

    public string sceneName;

    public void Start()
    {
        capacity = 1;
    }

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    public override void PlayerRelease(Player player)
    {
        if (capacity == abilities.Count)
            return;
        try {
            if (player.abilities[0] != AbilityType.Key)
                return;
        }
        catch
        {
            return;
        }
        PlayerReleaseAbilities(player);
    }

    public override void PlayerReleaseAbilities(Player player)
    {
        if (abilities.Count < capacity)
        {
            abilities.Add(player.abilities[0]);
            player.abilities.RemoveAt(0);
        }
        if(abilities.Count == capacity)
        {
            Change_Scene();
        }
    }

    private void Change_Scene()
    {
       SceneManager.LoadScene(sceneName);
    }
    public override void PlayerAbsorb(Player player)
    {
        return;
    }
}
