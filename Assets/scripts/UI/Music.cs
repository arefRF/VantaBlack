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
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            source = GetComponent<AudioSource>();
            Load_Musics();
            int i = Random.Range(0, 3);
            Music.instance.source.PlayOneShot(Music.instance.sounds[i]);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Load_Musics()
    {
        sounds = Resources.LoadAll<AudioClip>("Musics");
    }
}
