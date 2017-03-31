using UnityEngine;
using System.Collections.Generic;
using System;

public class DynamicContainer : FunctionalContainer {

    // Use this for initialization
    public override void Run() {
        abilities = new List<Ability>();
        for (int i = 0; i < abilitycount; i++)
        {
            abilities.Add(Ability.GetAbilityInstance(abilitytype));
            if (abilitytype == AbilityType.Jump)
                ((Jump)abilities[i]).number = 4;
        }
        api.ChangeSprite(this);
        if (abilitycount != 0 && abilitytype == AbilityType.Fuel && on)
            currentState = abilitycount;
        base.Run();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    public override CloneableUnit Clone()
    {
        return new CloneableDynamicContainer(this);
    }
}

public class CloneableDynamicContainer : CloneableUnit
{
    public List<Ability> abilities;
    public bool on;
    public bool firstmove;
    public int currentState, nextState;
    public CloneableDynamicContainer(DynamicContainer container) : base(container.position)
    {
        original = container;
        abilities = new List<Ability>();
        for (int i = 0; i < container.abilities.Count; i++)
            abilities.Add(container.abilities[i]);
        on = container.on;
        firstmove = container.firstmove;
        currentState = container.currentState;
        nextState = container.nextState;
    }

    public override void Undo()
    {
        base.Undo();
        DynamicContainer original = (DynamicContainer)base.original;
        original.gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
        original.abilities = new List<Ability>();
        for (int i = 0; i < abilities.Count; i++)
            original.abilities.Add(abilities[i]);
        original.on = on;
        original.firstmove = firstmove;
        original.currentState = currentState;
        original.nextState = nextState;
        original.api.engine.apigraphic.UnitChangeSprite(original);
    }
}
