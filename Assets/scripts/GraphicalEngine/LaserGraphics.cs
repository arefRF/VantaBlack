using UnityEngine;
using System.Collections.Generic;

public class LaserGraphics : MonoBehaviour {

    List<GameObject> BeamObjectPool;
    List<GameObject> UsedBeams;
    public GameObject beam;
    GameObject beamParent;
    private Texture2D BeamTexture;
    // Use this for initialization
    void Awake () {
        BeamObjectPool = new List<GameObject>();
        UsedBeams = new List<GameObject>();
        BeamTexture = Resources.Load<Texture2D>("lazer\\lazer line");
        beamParent = new GameObject("Laser Beams");
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void AddLaser(Vector2 pos1, Vector2 pos2)
    {
        Debug.Log("addlaser");
        float rotation = 0;
        if (pos1.x == pos2.x)
        {
            if (pos1.y > pos2.y) {
                AddLaser(pos2, pos1);
                return;
            }
            else
            {
                rotation = 0;
            }
            
        }
        else if (pos1.y == pos2.y && pos1.x > pos2.x)
        {
            if (pos1.y > pos2.y)
            {
                AddLaser(pos2, pos1);
                return;
            }
            else
            {
                rotation = 90;
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

    public void RemoveLasers()
    {
        for(int i=0;  UsedBeams.Count > 0;i++)
        {
            UsedBeams[i].transform.position = new Vector3(-1, -1, 0);
            BeamObjectPool.Add(UsedBeams[i]);
            UsedBeams.RemoveAt(0);
        }
    }

    private void makeBeam(Vector2 pos, float rotation)
    {
        GameObject beamcolon;
        if (BeamObjectPool.Count == 0)
        {
            beamcolon = Instantiate(beam);
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
        beamcolon.transform.rotation = Quaternion.Euler(beam.transform.rotation.x, beam.transform.rotation.y, rotation);


    }
}
