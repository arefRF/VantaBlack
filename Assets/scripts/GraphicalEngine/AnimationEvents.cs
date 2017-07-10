﻿using UnityEngine;
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
        transform.parent.GetComponent<Player>().LeanUndoFinished();
    }

    private void MoveToBranchFinished()
    {
        transform.localScale = new Vector2(0, 0);
        transform.parent.transform.position = transform.parent.GetComponent<Player>().position;
        Starter.GetEngine().apigraphic.MovePlayerFinished(transform.parent.gameObject);
        transform.parent.GetComponent<Player>().MoveToBranchFinished();
    }
}
