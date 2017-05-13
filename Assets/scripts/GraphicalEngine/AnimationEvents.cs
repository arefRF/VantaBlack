using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour {
    private void DrainFinished()
    {
        transform.parent.parent.GetComponent<PlayerGraphics>().DrainFinished();
    }
}
