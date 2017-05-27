using UnityEngine;
using System.Collections.Generic;

public class EyeMoveLocker : MonoBehaviour{
    public List<GameObject> eyes;

    public void LockEyes()
    {
        for(int i=0; i<eyes.Count; i++)
        {
            eyes[i].GetComponent<EyeMove>().EyeLock = true;
        }
    }
}
