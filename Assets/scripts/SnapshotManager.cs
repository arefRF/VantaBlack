using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnapshotManager{

    public SnapshotManager()
    {

    }

    public void takesnapshot(List<CloneableUnit> units, Vector3 CameraPos, float CameraSize)
    {
        Snapshot snp = new Snapshot(units, CameraPos, CameraSize);
        Database.database.snapshots.Add(snp);
    }

    public Snapshot Revese()
    {
        if (Database.database.snapshots.Count != 0)
        {
            Snapshot snapshot = Database.database.snapshots[Database.database.snapshots.Count - 1];
            Database.database.snapshots.RemoveAt(Database.database.snapshots.Count - 1);
            return snapshot;
        }
        return null;
    }
}

public class Snapshot
{
    public List<CloneableUnit> units;
    public Vector3 cameraPosition;
    public float cameraSize;
    public long turn;
    public Snapshot(List<CloneableUnit> units, Vector3 Camerapos, float CameraSize)
    {
        this.units = new List<CloneableUnit>();
        foreach (CloneableUnit u in units)
            this.units.Add(u);
        turn = Database.database.turn;
        cameraPosition = Camerapos;
        cameraSize = CameraSize;
    }
}

