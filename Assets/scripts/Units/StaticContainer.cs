using UnityEngine;
using System.Collections.Generic;

public class StaticContainer : FunctionalContainer {

    public override void Run()
    {
        abilities = new List<Ability>();
        for (int i = 0; i < abilitycount; i++)
        {
            abilities.Add(Ability.GetAbilityInstance(abilitytype));
            if (abilitytype == AbilityType.Jump)
                ((Jump)abilities[i]).number = 4;
        }
        api.ChangeSprite(this);
        base.Run();
    }

    public override CloneableUnit Clone()
    {
        return new CloneableStaticContainer(this);
    }
}

public class CloneableStaticContainer : CloneableUnit
{
    public bool on;
    public int moved;
    public int shouldmove;
    public bool movedone;
    public int stucklevel;
    public List<int> reservedmoveint;
    public List<bool> reservedmovebool;
    public bool resetstucked;
    public bool laston;
    public Direction stuckdirection;
    public int stuckstatus;
    public CloneableStaticContainer(StaticContainer container) : base(container.position)
    {
        original = container;
        reservedmovebool = new List<bool>();
        reservedmoveint = new List<int>();
        on = container.on;
    }

    public override void Undo()
    {
        base.Undo();
        StaticContainer original = (StaticContainer)base.original;
        original.gameObject.transform.parent.gameObject.GetComponent<ParentScript>().movelock = false;
        original.on = on;


        original.api.engine.apigraphic.UnitChangeSprite(original);
    }
}
