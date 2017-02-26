using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class Gate : Container {

    public string sceneName;
    public bool Internal;

    public override void Run()
    {
        abilities = new List<Ability>();
        capacity = 1;
        base.Run();
    }

    public override void SetInitialSprite()
    {

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
            if (player.abilities[0].abilitytype != AbilityType.Key)
                return;
        }
        catch
        {
            return;
        }
        api.AddToSnapshot(this);
        api.AddToSnapshot(player);
        Debug.Log(player.abilitycount);
        api.TakeSnapshot();
        PlayerReleaseAbilities(player);
    }

    public override void PlayerReleaseAbilities(Player player)
    {
        if (abilities.Count < capacity)
        {
            abilities.Add(player.abilities[0]);
            player.abilities.RemoveAt(0);
            _setability(player);    
            api.engine.apigraphic.UnitChangeSprite(this);
        }
    }
    public override void Action(Player player, Direction dir)
    {
        if (abilities == null)
            return;
        if (abilities.Count != capacity)
            return;
        if (Internal)
        {
            api.AddToSnapshot(this);
            api.MergeSnapshot();
            Debug.Log("Internal OPen");
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
    public List<Ability> abilities;
    public CloneableGate(Gate gate) : base(gate.position)
    {
        Debug.Log("wtfwtf");
        original = gate;
        abilities = new List<Ability>();
        for (int i = 0; i < gate.abilities.Count; i++)
            abilities.Add(gate.abilities[i]);
    }

    public override void Undo()
    {
        Debug.Log("here");
        base.Undo();
        Gate original = (Gate)base.original;
        original.abilities = new List<Ability>();
        for (int i = 0; i < abilities.Count; i++)
            original.abilities.Add(abilities[i]);
        original.api.AddToDatabase(original);
        original.api.engine.apigraphic.Undo_Unit(original);
        original.api.engine.apigraphic.UnitChangeSprite(original);
    }
}
