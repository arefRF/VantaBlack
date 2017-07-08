using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour {
    private void DrainFinished()
    {
        transform.parent.parent.GetComponent<PlayerGraphics>().DrainFinished();
    }

    private void ChangeColorFinished()
    {
        transform.parent.parent.GetComponent<PlayerGraphics>().ChangeColorFinished();
    }

    private void EnterBranchFinished()
    {
       transform.localScale = new Vector2(0, 0);
        transform.parent.transform.position = transform.parent.GetComponent<Player>().position;
        Starter.GetEngine().apigraphic.MovePlayerFinished(transform.parent.gameObject);
        
    }

    private void InTheBranch()
    {
        transform.localScale = new Vector2(1, 1);
    }
}
