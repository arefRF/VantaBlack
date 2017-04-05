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
        //TakeCameraSnapshot();
        database.snapshots.Add(snapshot);
        snapshot = new Snapshot();
    }

    private void TakeCameraSnapshot()
    {
       CameraController controller = Camera.main.GetComponent<CameraController>();
        snapshot.camerasnapshot.left_bound = controller.left_bound;
        snapshot.camerasnapshot.right_bound = controller.right_bound;
        snapshot.camerasnapshot.upper_bound = controller.upper_bound;
        snapshot.camerasnapshot.lower_bound = controller.lower_bound;
        snapshot.camerasnapshot.zoom = controller.zoom;
        snapshot.camerasnapshot.rotation = controller.rotation;
        snapshot.camerasnapshot.move_time = controller.move_time; 
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
            if(database.snapshots.Count > 1)
                database.snapshots.RemoveAt(database.snapshots.Count - 1);
            Undo(snp);
            UndoCamera(snp);
            try
            {
                GameObject.Find("HUD").transform.GetChild(0).GetComponent<HUD>().AbilityChanged(database.player[0]);
            }
            catch
            {
                GameObject.Find("UI").GetComponent<Get>().hud.SetActive(true);
                GameObject.Find("HUD").transform.GetChild(0).GetComponent<HUD>().AbilityChanged(database.player[0]);
            }
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

    private void UndoCamera(Snapshot snp)
    {
        CameraController controller = Camera.main.GetComponent<CameraController>();
        controller.Camera_Offset_Change(snp.camerasnapshot.left_bound, snp.camerasnapshot.right_bound, snp.camerasnapshot.lower_bound, snp.camerasnapshot.upper_bound);
        controller.Camera_Rotation_Change(snp.camerasnapshot.rotation, snp.camerasnapshot.move_time);
        controller.Camera_Size_Change(snp.camerasnapshot.zoom, 0);
    }
}

public class Snapshot
{
    public List<CloneableUnit> clonedunits;
    public List<Unit> stuckedunits;
    public CameraSnapShot camerasnapshot;
    public Snapshot()
    {
        clonedunits = new List<CloneableUnit>();
        stuckedunits = new List<Unit>();
        camerasnapshot = new CameraSnapShot();
    }
}

public class CameraSnapShot
{
    public float left_bound, right_bound, upper_bound, lower_bound;
    public float zoom;
    public float rotation;
    public float move_time;
}
