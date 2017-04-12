using UnityEngine;
using System.Collections.Generic;

public class PipeController {

    LogicalEngine engine;
    Database database;
    List<Pipe> pipes;
    Direction Gravitydirection_Total;
    public PipeController(LogicalEngine engine)
    {
        this.engine = engine;
        database = engine.database; 
        Gravitydirection_Total = database.gravity_direction;
        pipes = database.pipes;
    }

	public void CheckPipes()
    {
        for(int i=0; i<pipes.Count; i++)
        {
            pipes[i].Action();
        }
    }
}
