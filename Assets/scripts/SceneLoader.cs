using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour{
    private bool loadScene = false;
    private string scene;

    public void Load(string scene)
    {
        this.scene = scene;
        // If the player has pressed the space bar and a new scene is not loading yet...
        if (loadScene == false)
        {
            loadScene = true;
            Debug.Log("starting coroutine");
            // ...and start a coroutine that will load the desired scene.
            StartCoroutine(LoadNewScene());

        }
    }


    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene()
    {

        // This line waits for 3 seconds before executing the next line in the coroutine.
        // This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
        yield return new WaitForSeconds(1);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }

    }

}
