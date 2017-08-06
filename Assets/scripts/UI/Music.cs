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
            Debug.Log(Music.instance.sounds[i].length);
            Music.instance.source.PlayOneShot(Music.instance.sounds[i]);
            StartCoroutine(MusicShuffle(Music.instance.sounds[i].length));
        }

        DontDestroyOnLoad(this.gameObject);
    }

    IEnumerator MusicShuffle(float time)
    {
        yield return new WaitForSeconds(time);
        int i = Random.Range(0, 3);
        Music.instance.source.Stop();
        Music.instance.source.PlayOneShot(Music.instance.sounds[i]);
        StartCoroutine(MusicShuffle (Music.instance.sounds[i].length));
    }
    void Load_Musics()
    {
        sounds = Resources.LoadAll<AudioClip>("Musics");
    }
}
