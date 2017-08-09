using UnityEngine;
using System.Collections.Generic;

public class LaserGraphics : MonoBehaviour {

    List<GameObject> BeamObjectPool, UsedBeams, PartialBeamObjectPool, PartialUsedBeams;
    public GameObject Beam, PartialBeam;
    GameObject beamParent;
    private Texture2D BeamTexture;
    // Use this for initialization
    void Awake () {
        BeamObjectPool = new List<GameObject>();
        UsedBeams = new List<GameObject>();
        PartialBeamObjectPool = new List<GameObject>();
        PartialUsedBeams = new List<GameObject>();
        BeamTexture = Resources.Load<Texture2D>("lazer\\lazer line");
        beamParent = new GameObject("Laser Beams");
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void AddLaser(Vector2 pos1, Vector2 pos2, Direction direction)
    {
        float rotation = 0;
        if(pos1.x == pos2.x && pos1.y == pos2.y)
        {
             if(direction == Direction.Up || direction == Direction.Down)
                rotation = 90;
        }
        else if (pos1.x == pos2.x)
        {
            if (pos1.y > pos2.y) {
                AddLaser(pos2, pos1, direction);
                return;
            }
            else
            {
                rotation = 90;
            }
            
        }
        else if (pos1.y == pos2.y)
        {
            if (pos1.x > pos2.x)
            {
                AddLaser(pos2, pos1, direction);
                return;
            }
            else
            {
                rotation = 0;
            }
        }
        Vector2 temppos = new Vector2(pos1.x, pos1.y);
        if (pos1.x == pos2.x)
            for (; temppos.y <= pos2.y; temppos.y++)
                makeBeam(temppos, rotation);
        else
            for (; temppos.x <= pos2.x; temppos.x++)
                makeBeam(temppos, rotation);

    }

    public void AddPartialLaser(Vector2 pos, Direction direction)
    {
        float rot = 0;
        if (direction == Direction.Right)
            rot = 270;
        else if (direction == Direction.Down)
            rot = 180;
        else if (direction == Direction.Left)
            rot = 90;
        makePartialBeam(pos, rot);
    }
    public void RemoveLasers()
    {
        for(int i=0;  UsedBeams.Count > 0;i++)
        {
            UsedBeams[0].transform.position = new Vector3(-1, -1, 0);
            BeamObjectPool.Add(UsedBeams[0]);
            UsedBeams.RemoveAt(0);
        }
    }

    private void makeBeam(Vector2 pos, float rotation)
    {
        GameObject beamcolon;
        if (BeamObjectPool.Count == 0)
        {
            beamcolon = Instantiate(Beam);
            beamcolon.transform.SetParent(beamParent.transform);
            UsedBeams.Add(beamcolon);
        }
        else
        {
            beamcolon = BeamObjectPool[0];
            BeamObjectPool.RemoveAt(0);
            UsedBeams.Add(beamcolon);
        }
        beamcolon.transform.position = pos;
        beamcolon.transform.rotation = Quaternion.Euler(Beam.transform.rotation.x, Beam.transform.rotation.y, rotation);


    }

    private void makePartialBeam(Vector2 pos, float rotation)
    {
        GameObject beamcolon;
        if (PartialBeamObjectPool.Count == 0)
        {
            beamcolon = Instantiate(PartialBeam);
            beamcolon.transform.SetParent(beamParent.transform);
            PartialUsedBeams.Add(beamcolon);
        }
        else
        {
            beamcolon = PartialBeamObjectPool[0];
            PartialBeamObjectPool.RemoveAt(0);
            PartialUsedBeams.Add(beamcolon);
        }
        beamcolon.transform.position = pos;
        beamcolon.transform.rotation = Quaternion.Euler(PartialBeam.transform.rotation.x, PartialBeam.transform.rotation.y, rotation);
    }
}
