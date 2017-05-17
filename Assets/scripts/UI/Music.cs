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
            if (SceneManager.GetActiveScene().name == "Part-0")
            {
                Music.instance.source.Stop();
                Music.instance.source.PlayOneShot(Music.instance.sounds[0]);
            }
            else if(SceneManager.GetActiveScene().name == "Part-4")
            {
                Music.instance.source.Stop();
                Music.instance.source.PlayOneShot(Music.instance.sounds[1]);
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
