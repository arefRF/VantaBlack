using UnityEngine;
using System.Collections.Generic;

public class LaserGraphics : MonoBehaviour {

    List<GameObject> BeamObjectPool, UsedBeams, PartialBeamObjectPool, PartialUsedBeams;
    public GameObject Beam, PartialBeam;
    GameObject beamParent;
    private Texture2D BeamTexture;
    private GameObject laserParent;
    private GameObject laserParent2;
    private bool switcher = false;
    // Use this for initialization
    void Awake () {
        BeamObjectPool = new List<GameObject>();
        UsedBeams = new List<GameObject>();
        PartialBeamObjectPool = new List<GameObject>();
        PartialUsedBeams = new List<GameObject>();
        BeamTexture = Resources.Load<Texture2D>("lazer\\lazer line");
        beamParent = new GameObject("Laser Beams");
        laserParent = new GameObject();
    }


    public void AddLaserLine(Vector2 pos1, Vector2 pos2,GameObject parent)
    {
        if (laserParent == null)
            laserParent = new GameObject();
             GameObject myLine = new GameObject();
             myLine.transform.position = pos1;
             if(switcher)
                myLine.transform.parent = laserParent.transform;
             else
                 myLine.transform.parent = laserParent2.transform;
             myLine.AddComponent<LineRenderer>();
             LineRenderer lr = myLine.GetComponent<LineRenderer>();
             lr.SetColors(Color.red, Color.red);
             lr.SetWidth(0.1f, 0.1f);
             lr.SetPosition(0, pos1);
             lr.SetPosition(1, pos2);
    }

    public void DestroyLasers()
    {
        if (switcher)
            GameObject.Destroy(laserParent);
        else
            GameObject.Destroy(laserParent2);
        switcher = !switcher;
    }
    public void AddLaser(Vector2 pos1, Vector2 pos2, Direction direction, GameObject parent)
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
                AddLaser(pos2, pos1, direction, parent);
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
                AddLaser(pos2, pos1, direction, parent);
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
                makeBeam(temppos, rotation, parent);
        else
            for (; temppos.x <= pos2.x; temppos.x++)
                makeBeam(temppos, rotation, parent);

    }

    public void AddPartialLaser(Vector2 pos, Direction direction, GameObject parent)
    {
        float rot = 0;
        if (direction == Direction.Right)
            rot = 270;
        else if (direction == Direction.Down)
            rot = 180;
        else if (direction == Direction.Left)
            rot = 90;
        makePartialBeam(pos, rot, parent);
    }
    public void RemoveLasers()
    {
        for(int i=0;  UsedBeams.Count > 0;i++)
        {
            UsedBeams[0].transform.SetParent(beamParent.transform);
            UsedBeams[0].transform.position = new Vector3(-1, -1, 0);
            BeamObjectPool.Add(UsedBeams[0]);
            UsedBeams.RemoveAt(0);
        }
        for(int i=0; PartialUsedBeams.Count > 0; i++)
        {
            PartialUsedBeams[0].transform.position = new Vector3(-1, -1, 0);
            PartialBeamObjectPool.Add(PartialUsedBeams[0]);
            PartialUsedBeams[0].transform.SetParent(beamParent.transform);
            PartialUsedBeams.RemoveAt(0);
        }
    }

    private void makeBeam(Vector2 pos, float rotation, GameObject parent)
    {
        GameObject beamcolon;
        if (BeamObjectPool.Count == 0)
        {
            beamcolon = Instantiate(Beam);
        }
        else
        {
            beamcolon = BeamObjectPool[0];
            BeamObjectPool.RemoveAt(0);
        }
        beamcolon.transform.SetParent(parent.transform);
        UsedBeams.Add(beamcolon);
        beamcolon.transform.position = pos;
        beamcolon.transform.localPosition = new Vector3(Mathf.Round(beamcolon.transform.localPosition.x), Mathf.Round(beamcolon.transform.localPosition.y), Mathf.Round(beamcolon.transform.localPosition.z));
        beamcolon.transform.rotation = Quaternion.Euler(Beam.transform.rotation.x, Beam.transform.rotation.y, rotation);


    }

    private void makePartialBeam(Vector2 pos, float rotation, GameObject parent)
    {
        GameObject beamcolon;
        if (PartialBeamObjectPool.Count == 0)
        {
            beamcolon = Instantiate(PartialBeam);
        }
        else
        {
            beamcolon = PartialBeamObjectPool[0];
            PartialBeamObjectPool.RemoveAt(0);
        }

        beamcolon.transform.SetParent(parent.transform);
        PartialUsedBeams.Add(beamcolon);
        beamcolon.transform.position = pos;
        beamcolon.transform.localPosition = new Vector3(Mathf.Round(beamcolon.transform.localPosition.x), Mathf.Round(beamcolon.transform.localPosition.y), Mathf.Round(beamcolon.transform.localPosition.z));
        beamcolon.transform.rotation = Quaternion.Euler(PartialBeam.transform.rotation.x, PartialBeam.transform.rotation.y, rotation);
    }
}
