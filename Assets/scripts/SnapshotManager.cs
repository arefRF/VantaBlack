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
        database.snapshots.Add(snapshot);
        snapshot = new Snapshot();
    }

    public void AddToSnapShot(Unit unit)
    {
        snapshot.clonedunits.Add(unit.Clone());
    }
    public void Undo()
    {
        if (database.snapshots.Count != 0)
        {
            Snapshot snapshot = database.snapshots[database.snapshots.Count - 1];
            database.snapshots.RemoveAt(database.snapshots.Count - 1);
            Undo(snapshot);
        }
    }

    private void Undo(Snapshot snapshot)
    {
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

