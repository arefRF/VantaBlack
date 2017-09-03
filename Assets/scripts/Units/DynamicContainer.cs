using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;

public class DynamicContainer : FunctionalContainer {
    public LineRenderer linerenderer { get; set; }
    public bool LaserBeamHitting { get; set; }
    private bool templaserhit = false;
    private List<LaserBranchUnlocker> Branches, tempbranchlist;
    private List<ContainerLaser> Containers, tempcontainerlist;
    private List<Container> HittingContainers;
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
        Branches = new List<LaserBranchUnlocker>();
        tempbranchlist = new List<LaserBranchUnlocker>();
        Containers = new List<ContainerLaser>();
        tempcontainerlist = new List<ContainerLaser>();
        HittingContainers = new List<Container>();
        base.Run();
	}
	
	// Update is called once per frame
	void Update () {
        if (abilities.Count != 0 && abilities[0].abilitytype == AbilityType.Laser)
            SetLaser();
        if (!LaserBeamHitting && templaserhit)
        {
            templaserhit = false;
            if (linerenderer != null)
                Destroy(linerenderer.gameObject);
            linerenderer = null;
            api.engine.apigraphic.LaserUnHitDynamic(this);
        }
        else if (LaserBeamHitting && !templaserhit)
        {
            templaserhit = true;
        }
        if(!LaserBeamHitting && (abilities.Count == 0 || abilities[0].abilitytype != AbilityType.Laser))
        {
            if(linerenderer != null)
            {
                linerenderer = null;
                /*for(int i=0; i<Containers.Count; i++)
                {
                    Containers[i].laser = false;
                }*/
            }
        }
	}

    public override void SetCapacityLight()
    {
        if (capacity == 1)
        {
            Transform trans = transform.GetChild(1).GetChild(1).transform;
            Vector3 temppos = trans.localPosition;
            trans.localPosition = new Vector3(0.024f, temppos.y, temppos.z);
        }
        else if (capacity == 2)
        {
            Transform trans = transform.GetChild(1).GetChild(1).transform;
            Vector3 temppos = trans.localPosition;
            trans.localPosition = new Vector3(-0.368f, temppos.y, temppos.z);

            trans = transform.GetChild(1).GetChild(2).transform;
            temppos = trans.localPosition;
            trans.localPosition = new Vector3(0.363f, temppos.y, temppos.z);
        }
        else if (capacity == 3)
        {
            Transform trans = transform.GetChild(1).GetChild(1).transform;
            Vector3 temppos = trans.localPosition;
            trans.localPosition = new Vector3(-0.719f, temppos.y, temppos.z);

            trans = transform.GetChild(1).GetChild(2).transform;
            temppos = trans.localPosition;
            trans.localPosition = new Vector3(-0.017f, temppos.y, temppos.z);

            trans = transform.GetChild(1).GetChild(3).transform;
            temppos = trans.localPosition;
            trans.localPosition = new Vector3(0.702f, temppos.y, temppos.z);
        }
    }

    public override bool PlayerMoveInto(Direction dir)
    {
        return false;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void ChangeLaserHitState(bool HitState)
    {
        LaserBeamHitting = HitState;
    }

    public void SetLaser()
    {
        tempbranchlist.AddRange(Branches);
        tempcontainerlist.AddRange(Containers);
        SetLaserInDirection(direction, transform.position, linerenderer, this);
        for (int i = 0; i < tempbranchlist.Count; i++)
        {
            Branches.Remove(tempbranchlist[i]);
            StopCoroutine(tempbranchlist[i].LaserUnlockWaitCoroutine);
        }
        for (int i = 0; i < tempcontainerlist.Count; i++)
        {
            Containers.Remove(tempcontainerlist[i]);
            HittingContainers.Remove(tempcontainerlist[i].container);
            tempcontainerlist[i].container.ChangeLaserHitState(false);
        }
        tempbranchlist.Clear();
        tempcontainerlist.Clear();
    }

    private void SetLaserInDirection(Direction direction, Vector2 startingpos, LineRenderer linerenderer, Unit LaserSource)
    {

        Vector2 pos = Toolkit.VectorSum(startingpos, Toolkit.DirectiontoVector(direction) / 1.95f);
        RaycastHit2D hit = Physics2D.Raycast(pos, Toolkit.DirectiontoVector(direction), Mathf.Max(api.engine.sizeX, api.engine.sizeY));
        Vector2 finalpos = hit.point;
        if (LaserSource is DynamicContainer)
            pos -= Toolkit.DirectiontoVector((LaserSource as DynamicContainer).direction) / 5;
        if (hit.collider == null)
        {
            finalpos = pos + Toolkit.DirectiontoVector(direction) * Mathf.Max(api.engine.sizeX, api.engine.sizeY);
        }
        else
        {
            if (hit.collider.transform.gameObject.GetComponent<Unit>().isLeanable())
                finalpos += Toolkit.DirectiontoVector(direction) / 8;
            if (hit.collider.tag == "Dynamic Container")
            {
                DynamicContainer tempcontainer = hit.collider.transform.gameObject.GetComponent<DynamicContainer>();
                if (!(tempcontainer.ConnectedUnits.Contains(this) && Toolkit.AreNeighbours(tempcontainer, this)))
                {
                    bool flag = false;
                    for (int i = 0; i < Containers.Count; i++)
                    {
                        if (tempcontainer == Containers[i].container)
                        {
                            tempcontainer.ChangeLaserHitState(true);
                            flag = true;
                            tempcontainerlist.Remove(Containers[i]);
                            if (Containers[i].ContainerTimeFinished)
                            {
                                SetLaserInDirection(tempcontainer.direction, tempcontainer.transform.position, tempcontainer.linerenderer, tempcontainer);
                                if (!HittingContainers.Contains(tempcontainer))
                                    HittingContainers.Add(tempcontainer);
                            }
                            break;
                        }
                    }
                    if (!flag && !HittingContainers.Contains(tempcontainer))
                    {
                        ContainerLaser temp = new ContainerLaser();
                        temp.container = tempcontainer;
                        temp.ContainerTimeFinished = false;
                        Containers.Add(temp);
                        api.engine.apigraphic.LaserHitDynamic(tempcontainer);
                        temp.ContainerLaserBeginCoroutine = StartCoroutine(ContainerLaserBegin(0.95f, Containers[Containers.Count - 1]));
                    }
                }
            }
            else if (hit.collider.tag == "Player")
            {
                Player tempplayer = hit.collider.transform.gameObject.GetComponent<Player>();
                api.engine.apigraphic.Laser_Player_Died(tempplayer);
                tempplayer.transform.position = new Vector3(-1, -1, 0);
            }
            else if (hit.collider.tag == "Branch")
            {
                Branch tempbranch = hit.collider.transform.gameObject.GetComponent<Branch>();
                if (tempbranch.islocked)
                {
                    bool flag = false;
                    for (int i = 0; i < Branches.Count; i++)
                    {
                        if (Branches[i].branch == tempbranch)
                        {
                            flag = true;
                            tempbranchlist.Remove(Branches[i]);
                            if (Branches[i].LaserUnlockTimeFinished)
                            {
                                api.engine.apigraphic.UnlockBranchLaser(tempbranch);
                                Branches.RemoveAt(i);
                            }
                            break;
                        }
                    }
                    if (!flag)
                    {
                        LaserBranchUnlocker temp = new LaserBranchUnlocker();
                        temp.branch = tempbranch;
                        temp.LaserUnlockTimeFinished = false;
                        Branches.Add(temp);
                        temp.LaserUnlockWaitCoroutine = StartCoroutine(LaserUnlockWait(1, Branches[Branches.Count - 1]));
                    }
                }
            }
        }
        linerenderer = api.engine.apigraphic.AddLaserLine(pos, finalpos, transform.parent.gameObject, linerenderer);
        if (LaserSource is DynamicContainer)
            (LaserSource as DynamicContainer).linerenderer = linerenderer;
        else
        {
            this.linerenderer = linerenderer;
        }
    }

    public IEnumerator LaserUnlockWait(float f, LaserBranchUnlocker branchunlocker)
    {
        yield return new WaitForSeconds(f);
        branchunlocker.LaserUnlockTimeFinished = true;
    }

    public IEnumerator ContainerLaserBegin(float f, ContainerLaser containerlaser)
    {
        yield return new WaitForSeconds(f);
        containerlaser.ContainerTimeFinished = true;
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
