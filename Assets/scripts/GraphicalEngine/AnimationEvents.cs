using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour {

    public bool call;
    private Vector2 pos;
    private Jump ability;
    private Direction dir;
    private bool hit;

    public void SetJumpCordinates(Vector2 pos1, Jump ability1, Direction dir1, bool hit1)
    {
        pos = pos1;
        ability = ability1;
        dir = dir1;
        hit = hit1;
    }

    private void BlockToFallFinished()
    {
        transform.parent.parent.GetComponent<PlayerGraphicsV5>().BlockToFallFinished();
    }
    private void OpenEye()
    {
        transform.parent.parent.GetComponent<PlayerGraphics>().OpenEye();
    }

    private void ResetEye()
    {
        transform.parent.parent.GetComponent<PlayerGraphics>().ResetEye();
    }
    private void DrainFinished()
    {
        transform.parent.parent.GetComponent<PlayerGraphics>().DrainFinished();
    }
    
    private void ChangeColorFinished()
    {
        transform.parent.parent.GetComponent<PlayerGraphics>().ChangeColorFinished();
    }
    private void LeanUndoFinished()
    {
        gameObject.GetComponentInParent<Player>().LeanUndoFinished();
    }

    private void BranchExitFinished()
    {
        GetComponent<Animator>().SetInteger("Branch", 0);
        
    }

    private void MoveToBranchFinished()
    {
        transform.localScale = new Vector2(0, 0);
        transform.parent.transform.position = transform.parent.GetComponent<Player>().position;
    }

    ///  this is being called after MoveToBranch
    private void InTheBranch()
    {
        transform.localScale = new Vector2(1, 1);
        if(call)
            transform.parent.GetComponent<Player>().MoveToBranchFinished();
    }

    private void LandFinished()
    {
        transform.GetComponentInParent<PlayerGraphics>().LandFinished();
    }

    private void NowJump()
    {
        Debug.Log("Jump");
        transform.parent.GetComponent<PlayerPhysics>().Jump(pos, ability, dir, hit);
    }
}
