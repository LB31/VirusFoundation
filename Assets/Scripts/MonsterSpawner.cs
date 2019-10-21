using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public float SpawnDelay;

    public GameObject Virus;
    public Material[] MonsterMats;

    public bool Spawning = true;

    public GameObject Goal;

    public List<Transform> goalParts;

    void Start()
    {
        foreach (Transform item in Goal.transform) {
            goalParts.Add(item);
        }

        StartCoroutine(SpawnMonsters(SpawnDelay));
    }

    void Update()
    {
        
    }

    IEnumerator SpawnMonsters(float timeToWait) {
        while (Spawning) {
            yield return new WaitForSeconds(timeToWait);
            int matNumber = Random.Range(0, MonsterMats.Length);
            int goalNumber = Random.Range(0, goalParts.Count);
            GameObject virus = Instantiate(Virus, transform.position, transform.rotation);
            virus.transform.parent = transform;
            virus.transform.LookAt(goalParts[goalNumber]);
            virus.transform.localScale *= 1.5f;
            EnemyControllerLevel2 ecl = virus.AddComponent<EnemyControllerLevel2>();
            ecl.goal = goalParts[goalNumber];
            virus.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = MonsterMats[matNumber];
        }

    }
}
