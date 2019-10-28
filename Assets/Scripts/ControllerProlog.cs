using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerProlog : MonoBehaviour
{
    public float SpawnDelay = 4;
    public GameObject Virus;
    public List<Material> MonsterMats;

    public Transform Goal;

    public Transform[] WaypointsTrojan;



    void Start() {
        Goal = GameManager.Instance.World.transform;
        StartCoroutine(SpawnMonster(SpawnDelay));
    }


    void Update() {

    }


    IEnumerator SpawnMonster(float timeToWait) {
        for (int i = 0; i < 3; i++) {
            yield return new WaitForSeconds(timeToWait);
            GameObject virus = Instantiate(Virus, Camera.main.transform.position, Virus.transform.rotation);
            virus.transform.parent = WaypointsTrojan[0].parent.parent;
            virus.transform.LookAt(Goal);
            //virus.transform.localScale *= 5f;
            //EnemyControllerLevel2 ecl = virus.AddComponent<EnemyControllerLevel2>();
            //ecl.goal = goalParts[goalNumber];
            virus.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = MonsterMats[i];

            MonsterProlog mp = virus.AddComponent<MonsterProlog>();
            mp.Goal = Goal;
            mp.waypoints = new List<Transform>(WaypointsTrojan);
            mp.waypoints.RemoveRange(mp.waypoints.Count-i, i);

            Rigidbody rb = virus.GetComponent<Rigidbody>();
            rb.AddForce((Goal.position + new Vector3((i - 1) / 4f, 0, 0) - virus.transform.position) * 5f);
        }

    }

}


public class MonsterProlog : MonoBehaviour
{
    public Transform Goal;
    private Rigidbody rg;
    private Animator anim;


    public List<Transform> waypoints;

    private bool landing = true;
    private bool toTroja;
    private float t;
    private float scale = 5;

    private void Start() {
        rg = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        if (landing) {
            float distance = Goal.position.z - transform.position.z;
            if (distance < 0.07f) {
                rg.velocity = Vector3.zero;
                rg.useGravity = true;
                landing = false;
                toTroja = true;
            }
        }
        if (toTroja) {
            if (t < 2) {
                t += Time.deltaTime;
            } else if (rg.useGravity) {
                rg.useGravity = false;
            }

            float step = 0.3f / scale * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, waypoints[0].position, step);
            transform.LookAt(waypoints[0]);
            if (Vector3.Distance(transform.position, waypoints[0].position) <= 0.05 / scale) {
                if (waypoints[0].Equals(waypoints[waypoints.Count - 1]) && waypoints[0].name == "p5") { // last virus rolled in
                    GameObject.Find("TrojanHorse").GetComponent<Animator>().Play("trojanMovement");
                }
                waypoints.RemoveAt(0);
            }
            if (waypoints.Count == 0) {
                toTroja = false;
                rg.velocity = Vector3.zero;
                rg.angularVelocity = Vector3.zero;
                anim.Play("Idle", 0, 0.25f);
            }
        }

    }

}