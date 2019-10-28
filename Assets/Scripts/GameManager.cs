using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject World;
    public GameObject[] UIs;

    public Transform ExitLevel1;

    public AudioSource Music;
    public AudioSource Sound;

    public AudioClip[] AllClips;

    private bool AudioIsPlaying;

    public int PlayerLife = 20;
    public TextMeshProUGUI LifeText;
    public int KilledEnemies;
    public TextMeshProUGUI EnemyText;

 

    private void Awake() {
        if (Instance) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start() {

    }

    public void ChangeLevel(int number) {
        foreach (Transform item in World.transform) {
            item.gameObject.SetActive(false);
        }
        foreach (GameObject item in UIs) {
            item.SetActive(false);
        }

        UIs[number].SetActive(true);
        World.transform.GetChild(number).gameObject.SetActive(true);
        
        //StartCoroutine(ChangeMusic("Level1"));
    }




    public IEnumerator ChangeMusic(string clipName) {
        //Music.clip = AllClips[0];
        //Music.Play();
        //while (Music.isPlaying) {
        //    yield return new WaitForSeconds(1);
        //}
        foreach (AudioClip item in AllClips) {
            if(item.name == clipName) {
                Music.clip = item;
                Music.Play();
                yield return null;
            }
        }

    }

    public IEnumerator PlaySound(string soundName) {
        while (Sound.isPlaying) {
            yield return new WaitForSeconds(0.001f);
        }

        foreach (AudioClip item in AllClips) {
            if (item.name == soundName) {
                Sound.clip = item;
                Sound.Play();
                yield return null;
            }
        }
    }

    private void Update() {
        //if (GameManager.Instance.KilledEnemies >= 10) {
        //    GameManager.Instance.ChangeLevel(2);
        //}
    }

}
