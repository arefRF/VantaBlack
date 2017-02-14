using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleContainer : Container {

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
    public List<AbilityType> abilities;
    public CloneableSimpleContainer(SimpleContainer container) : base(container.position)
    {
        original = container;
        abilities = new List<AbilityType>();
        for (int i = 0; i < container.abilities.Count; i++)
            abilities.Add(container.abilities[i]);
    }

    public override void Undo()
    {
        base.Undo();
        SimpleContainer original = (SimpleContainer)base.original;
        original.abilities = new List<AbilityType>();
        for (int i = 0; i < abilities.Count; i++)
            original.abilities.Add(abilities[i]);

        original.api.engine.apigraphic.UnitChangeSprite(original);
    }
}
