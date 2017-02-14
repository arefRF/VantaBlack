﻿using UnityEngine;
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
            Debug.Log(database.snapshots.Count);
            for (int i = 0; i < snapshot.clonedunits.Count; i++)
            {
                bool flag = true;
                for(int j=0; j< database.snapshots[database.snapshots.Count - 1].clonedunits.Count; j++)
                {
                    if(snapshot.clonedunits[i].original == database.snapshots[database.snapshots.Count - 1].clonedunits[j].original)
                    {
                        database.snapshots[database.snapshots.Count - 1].clonedunits.RemoveAt(j);
                        database.snapshots[database.snapshots.Count - 1].clonedunits.Add(snapshot.clonedunits[i]);
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
            {
                return;
            }
        snapshot.clonedunits.Add(unit.Clone());
    }
    public void AddToSnapShot(List<Unit> units)
    {
        for (int i = 0; i < units.Count; i++)
            AddToSnapShot(units[i]);
    }
    public void Undo()
    {
        Debug.Log(database.snapshots.Count);
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
        Debug.Log(snapshot.clonedunits.Count);
        for (int i = 0; i < snapshot.clonedunits.Count; i++)
        {
            snapshot.clonedunits[i].Undo();
        }
    }
}

public class Snapshot
{
    public List<CloneableUnit> clonedunits;
    public Snapshot()
    {
        clonedunits = new List<CloneableUnit>();
    }
}

