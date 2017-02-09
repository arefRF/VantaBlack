using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnapshotManager{
    Database database;
    LogicalEngine engine;
    public SnapshotManager(LogicalEngine engine)
    {
        this.engine = engine;
        database = engine.database;
    }

    public void takesnapshot(List<Unit> units)
    {
        Snapshot snp = new Snapshot(units);
        database.snapshots.Add(snp);
    }

    public void Reverse()
    {
        if (database.snapshots.Count != 0)
        {
            Snapshot snapshot = database.snapshots[database.snapshots.Count - 1];
            database.snapshots.RemoveAt(database.snapshots.Count - 1);
            Undo(snapshot);
        }
    }

    public void Undo(Snapshot snapshot)
    {
        for(int i=0; i<snapshot.clonedunits.Count; i++)
        {
            Debug.Log(snapshot.originalunits[i].position);
            Debug.Log(snapshot.clonedunits[i].position);
            if (engine.database.units[(int)snapshot.originalunits[i].position.x, (int)snapshot.originalunits[i].position.y].Contains(snapshot.originalunits[i]))
                Debug.Log("containes");
            engine.apiunit.RemoveFromDatabase(snapshot.originalunits[i]);
            if (snapshot.originalunits[i] is Player)
            {
                engine.database.player.Remove((Player)snapshot.originalunits[i]);
            }
            snapshot.originalunits[i] = snapshot.clonedunits[i].Clone();
            Debug.Log(((Player)snapshot.originalunits[i]).move_direction.Count);
            if (snapshot.originalunits[i] is Player)
            {
                engine.database.player.Add((Player)snapshot.originalunits[i]);
            }
            engine.apiunit.AddToDatabase(snapshot.originalunits[i]);
            if (snapshot.originalunits[i] is Player)
            {
                snapshot.originalunits[i].transform.position = snapshot.clonedunits[i].position;
            }
            else
                snapshot.originalunits[i].transform.parent.transform.position = snapshot.originalunits[i].position - (Vector2)snapshot.originalunits[i].transform.position;
            Debug.Log(snapshot.originalunits[i].position);
            if(engine.database.units[(int)snapshot.originalunits[i].position.x, (int)snapshot.originalunits[i].position.y][engine.database.units[(int)snapshot.originalunits[i].position.x, (int)snapshot.originalunits[i].position.y].Count - 1] == snapshot.originalunits[i])
                Debug.Log("wtf akhe");
        }
    }
}

public class Snapshot
{
    public List<Unit> clonedunits;
    public List<Unit> originalunits;
    public Snapshot(List<Unit> units)
    {
        clonedunits = new List<Unit>();
        originalunits = new List<Unit>();
        foreach (Unit u in units)
        {
            Debug.Log(u.position);
            originalunits.Add(u);
            clonedunits.Add(u.Clone());
        }
    }
}

