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
            if (abilitytype == AbilityType.Jump)
                ((Jump)abilities[i]).number = 4;
        }
        api.ChangeSprite(this);
        base.Run();
    }

    public override void SetCapacityLight()
    {
        if (capacity == 1)
        {
            Transform trans = transform.GetChild(1).GetChild(1).transform;
            Vector3 temppos = trans.localPosition;
            trans.localPosition = new Vector3(0.005f, -1.111f, temppos.z);
        }
        else if (capacity == 2)
        {
            Transform trans = transform.GetChild(1).GetChild(1).transform;
            Vector3 temppos = trans.localPosition;
            trans.localPosition = new Vector3(-0.689f, -0.928f, temppos.z);

            trans = transform.GetChild(1).GetChild(2).transform;
            temppos = trans.localPosition;
            trans.localPosition = new Vector3(0.698f, -0.933f, temppos.z);
        }
        else if (capacity == 3)
        {
            Transform trans = transform.GetChild(1).GetChild(1).transform;
            Vector3 temppos = trans.localPosition;
            trans.localPosition = new Vector3(-0.689f, -0.928f, temppos.z);

            trans = transform.GetChild(1).GetChild(2).transform;
            temppos = trans.localPosition;
            trans.localPosition = new Vector3(0.019f, -1.139f, temppos.z);

            trans = transform.GetChild(1).GetChild(3).transform;
            temppos = trans.localPosition;
            trans.localPosition = new Vector3(0.682f, -0.937f, temppos.z);
        }
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
