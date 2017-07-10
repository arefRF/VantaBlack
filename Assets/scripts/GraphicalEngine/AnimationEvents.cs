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

   

    private void InTheBranch()
    {
        transform.localScale = new Vector2(1, 1);
    }

    private void LeanUndoFinished()
    {
        Debug.Log("Lean Undo Finished");
        transform.parent.GetComponent<Player>().LeanUndoFinished();
    }

    private void MoveToBranchFinished()
    {
        Debug.Log("Move To Branch");
        transform.parent.GetComponent<Player>().MoveToBranchFinished();
    }
}
