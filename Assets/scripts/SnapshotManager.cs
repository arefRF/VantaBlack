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
        for (int i = 0; i < snapshot.clonedunits.Count; i++)
        {
            if (snapshot.clonedunits[i] is CloneablePlayer)
                UndoPlayer(snapshot.clonedunits[i]);
        }
    }

    private void UndoPlayer(CloneableUnit unit)
    {
        CloneablePlayer player = (CloneablePlayer)unit;
        Debug.Log(player.position);
        engine.apiunit.RemoveFromDatabase(player.original);
        player.original.position = player.position;
        engine.apiunit.AddToDatabase(player.original);
        player.original.abilities = new List<AbilityType>();
        for (int i = 0; i < player.abilities.Count; i++)
            player.original.abilities.Add(player.abilities[i]);
        player.move_direction = new List<Direction>();
        for (int i = 0; i < player.move_direction.Count; i++)
            player.move_direction.Add(player.move_direction[i]);
        player.original.direction = player.direction;
        player.original.movepercentage = player.movepercentage;
        player.original.state = player.state;
        player.original.leandirection = player.leandirection;
        player.original.lean = player.lean;
        player.original.onramp = player.onramp;
        player.original.gravity = player.gravity;
        player.original.nextpos = new Vector2(player.nextpos.x, player.nextpos.y);

        player.original.transform.position = player.position;
    }
}

public class Snapshot
{
    public List<CloneableUnit> clonedunits;
    public Snapshot(List<Unit> units)
    {
        clonedunits = new List<CloneableUnit>();
        foreach (Unit u in units)
        {
<<<<<<< HEAD
=======
            originalunits.Add(u);
>>>>>>> 1ae737e01ed0f834e120731571da1cfd8ca2035d
            clonedunits.Add(u.Clone());
        }
    }
}

