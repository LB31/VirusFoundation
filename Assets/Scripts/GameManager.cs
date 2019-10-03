using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform ExitLevel1;

    public AudioSource Music;
    public AudioSource Sound;

    public AudioClip[] AllClips;

    private bool AudioIsPlaying;

    private void Awake() {
        if (Instance) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        StartCoroutine(PlaySound("Swipe"));
    }

    public IEnumerator ChangeMusic(string clipName) {
        Music.clip = AllClips[0];
        Music.Play();
        while (Music.isPlaying) {
            yield return new WaitForSeconds(1);
        }
        foreach (AudioClip item in AllClips) {
            if(item.name == clipName) {
                Music.clip = item;
                Music.Play();
                break;
            }
        }
    }

    public IEnumerator PlaySound(string soundName) {
        foreach (AudioClip item in AllClips) {
            if (item.name == soundName) {
                Sound.clip = item;
                Sound.Play();
                yield return null;
            }
        }
    }

}
