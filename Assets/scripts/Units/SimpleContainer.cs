using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleContainer : Container {

    public override void Run()
    {
        abilities = new List<Ability>();
        for(int i=0; i<abilitycount; i++)
        {
            abilities.Add(Ability.GetAbilityInstance(abilitytype));
        }
        api.ChangeSprite(this);
        base.Run();
    }

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    public override CloneableUnit Clone()
    {
        return new CloneableSimpleContainer(this);
    }
}

public class CloneableSimpleContainer : CloneableUnit
{
    public List<Ability> abilities;
    public CloneableSimpleContainer(SimpleContainer container) : base(container.position)
    {
        original = container;
        abilities = new List<Ability>();
        for (int i = 0; i < container.abilities.Count; i++)
            abilities.Add(container.abilities[i]);
    }

    public override void Undo()
    {
        base.Undo();
        SimpleContainer original = (SimpleContainer)base.original;
        original.abilities = new List<Ability>();
        for (int i = 0; i < abilities.Count; i++)
            original.abilities.Add(abilities[i]);

        original.api.engine.apigraphic.UnitChangeSprite(original);
    }
}
