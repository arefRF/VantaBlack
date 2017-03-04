using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChangeEvent : MonoBehaviour
{
    public string scenename;
    public bool smoothchange;
    SceneLoader sceneloader;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (smoothchange)
            {

                SceneManager.LoadSceneAsync(scenename);
            }
        }
    }
}
