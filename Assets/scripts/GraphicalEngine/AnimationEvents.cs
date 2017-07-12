using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour {

    public bool call;
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
        transform.parent.GetComponent<Player>().LeanUndoFinished();
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

    private void ChangeDirectionToLeftFinished()
    {
        transform.parent.GetComponent<PlayerGraphics>().Change_Direction_Finished(Direction.Left);
    }


    private void ChangeDirectionToRightFinished()
    {
        transform.parent.GetComponent<PlayerGraphics>().Change_Direction_Finished(Direction.Right);

    }

    ///  this is being called after MoveToBranch
    private void InTheBranch()
    {
        transform.localScale = new Vector2(1, 1);
        if(call)
            transform.parent.GetComponent<Player>().MoveToBranchFinished();
    }
}
