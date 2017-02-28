using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {
    public string scene_name;
    void OnTriggerEnter2D()
    {
        SceneManager.LoadScene(scene_name);
    }
}
