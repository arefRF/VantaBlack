using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Intro : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Wait());
	}

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("Menu V2");
    }

}
