using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class Gate : Container {

    public string sceneName;
    public bool Internal;

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
            Open();
        }
    }


    private void Open()
    {
        if (Internal)
        {
            GetComponent<Animator>().SetBool("Open", true);
            api.RemoveFromDatabase(this);
        }
        else
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

    public override CloneableUnit Clone()
    {
        return new CloneableGate(this);
    }
}

public class CloneableGate : CloneableUnit
{
    public List<AbilityType> abilities;
    public CloneableGate(Gate gate) : base(gate.position)
    {
        original = gate;
        abilities = new List<AbilityType>();
        for (int i = 0; i < gate.abilities.Count; i++)
            abilities.Add(gate.abilities[i]);
    }

    public override void Undo()
    {
        base.Undo();
        Gate original = (Gate)base.original;
        original.abilities = new List<AbilityType>();
        for (int i = 0; i < abilities.Count; i++)
            original.abilities.Add(abilities[i]);

        original.api.engine.apigraphic.UnitChangeSprite(original);
    }
}
