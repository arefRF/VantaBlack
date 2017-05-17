using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Music : MonoBehaviour {
    private static Music instance = null;
    private AudioSource source;
    private AudioClip[] sounds;
    // Use this for initialization
    void Start () {
        if (instance != null && instance != this)
        {
            Debug.Log(SceneManager.GetActiveScene().name);
            if (SceneManager.GetActiveScene().name == "Part-0")
            {
                Debug.Log(Music.instance.source);
                Music.instance.source.PlayOneShot(sounds[0]);
            }
            else if(SceneManager.GetActiveScene().name == "Part-4")
            {

            }
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            source = GetComponent<AudioSource>();
            Load_Musics();
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Load_Musics()
    {
        sounds = Resources.LoadAll<AudioClip>("Musics");
    }
}
