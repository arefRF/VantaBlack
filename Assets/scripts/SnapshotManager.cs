using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnapshotManager{
    Database database;
    LogicalEngine engine;
    Snapshot snapshot;
    public SnapshotManager(LogicalEngine engine)
    {
        this.engine = engine;
        database = engine.database;
        snapshot = new Snapshot();
    }

    public void takesnapshot()
    {
        if (snapshot.clonedunits.Count == 0)
            return;
        database.snapshots.Add(snapshot);
        snapshot = new Snapshot();
    }
    private void AddStucktoOtherList(List<Unit> copyto, List<Unit> copyfrom)
    {
        copyto.Clear();
        for (int i = 0; i < copyfrom.Count; i++)
        {
            if(!copyto.Contains(copyfrom[i]))
                copyto.Add(copyfrom[i]);
        }
    }
    public void CloneStuckList()
    {
        if(snapshot.stuckedunits.Count == 0)
            AddStucktoOtherList(snapshot.stuckedunits, engine.stuckedunits);
    }
    public void MergeSnapshot()
    {
        if (snapshot.clonedunits.Count == 0)
        {
            return;
        }
        if (database.snapshots.Count == 0)
            takesnapshot();
        else
        {
            for (int i = 0; i < snapshot.clonedunits.Count; i++)
            {
                bool flag = true;
                for(int j=0; j< database.snapshots[database.snapshots.Count - 1].clonedunits.Count; j++)
                {
                    if(snapshot.clonedunits[i].original == database.snapshots[database.snapshots.Count - 1].clonedunits[j].original)
                    {
                        flag = false;
                        break;
                    }
                }
               if(flag)
                    database.snapshots[database.snapshots.Count - 1].clonedunits.Add(snapshot.clonedunits[i]);
            }
            snapshot = new Snapshot();
        }
    }

    public void AddToSnapShot(Unit unit)
    {
        for (int i = 0; i < snapshot.clonedunits.Count; i++)
            if (snapshot.clonedunits[i].original == unit)
                return;
        snapshot.clonedunits.Add(unit.Clone());
    }
    public void AddToSnapShot(List<Unit> units)
    {
        for (int i = 0; i < units.Count; i++)
            AddToSnapShot(units[i]);
    }
    public void Undo()
    {
        if (database.snapshots.Count != 0)
        {
            Snapshot snp = database.snapshots[database.snapshots.Count - 1];
            database.snapshots.RemoveAt(database.snapshots.Count - 1);
            Undo(snp);
            /*for(int i=0; i<snapshot.clonedunits.Count; i++)
            {
                bool flag = true;
                for(int j=0; j<snp.clonedunits.Count; j++)
                {
                    if(snp.clonedunits[j].original == snapshot.clonedunits[i].original)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                    snapshot.clonedunits[i].Undo();
            }
            snapshot.clonedunits.Clear();*/
        }
    }

    private void Undo(Snapshot snapshot)
    {
        AddStucktoOtherList(engine.stuckedunits, snapshot.stuckedunits);
        for (int i = 0; i < snapshot.clonedunits.Count; i++)
        {
            snapshot.clonedunits[i].Undo();
        }
    }
}

public class Snapshot
{
    public List<CloneableUnit> clonedunits;
    public List<Unit> stuckedunits;
    public Snapshot()
    {
        clonedunits = new List<CloneableUnit>();
        stuckedunits = new List<Unit>();
    }
}

