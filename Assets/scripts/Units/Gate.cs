using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class Gate : Container {

    public string sceneName;
    public GateType gatetype;
    public SceneLoader sceneloader;
    public override void Run()
    {
        abilities = new List<Ability>();
        capacity = 1;
        sceneloader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
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
            api.engine.apigraphic.Absorb(player, null);
            Action();
        }
    }

    public void Action()
    {
        if (gatetype == GateType.Internal)
        {
            api.AddToSnapshot(this);
            api.MergeSnapshot();
            Debug.Log("Internal OPen");
            GetComponent<Animator>().SetBool("Open", true);
            api.RemoveFromDatabase(this);
        }
        else if(gatetype == GateType.InternalChangeScene)
        {
            SceneManager.LoadScene(sceneName);
        }
        else if(gatetype == GateType.External)
        {
            Change_Scene();
        }
    }

    public override void Action(Player player, Direction dir)
    {   
    }
    private void Change_Scene()
    {
        Debug.Log("changing scene");
        //SceneManager.LoadScene(sceneName);
        sceneloader.Load(sceneName);
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
        original = gate;
        abilities = new List<Ability>();
        for (int i = 0; i < gate.abilities.Count; i++)
            abilities.Add(gate.abilities[i]);
    }

    public override void Undo()
    {
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
