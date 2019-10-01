using UnityEngine;
using System.Collections;

public class YouWinTest : MonoBehaviour {
    CreateLevel gameCtrl;

    // Use this for initialization
    void Start()
    {
        gameCtrl = FindObjectOfType<CreateLevel>();
    }

    void OnTriggerEnter(Collider other)
    {
        gameCtrl.winTrigger(other.gameObject);
    }
}
