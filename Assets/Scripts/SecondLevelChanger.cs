using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLevelChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameManager.Instance.ChangeMusic("Level2"));
        GameManager.Instance.ChangeLevel(1);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
