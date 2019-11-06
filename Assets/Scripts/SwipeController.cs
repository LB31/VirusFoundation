using UnityEngine;
using System.Collections;

public class SwipeController : MonoBehaviour
{
    public GameObject AntiVirusPrefab;
    public Material AntiMat;

    private GameObject AntiVirus;

    [SerializeField]
    private float throwSpeed = 30f;
    private float speed;
    private float lastMouseX, lastMouseY;

    private bool thrown, holding;

    private Rigidbody rigidBody;
    private Vector3 newPosition;
    private bool virusIsPresent;

    private bool touched;
    private bool threw, initialThrew; 
    private bool moved;

    void Start() {

    }

    void Update() {
        if (!virusIsPresent) {
            return;
        }
            

        if (holding)
            OnTouch();

        if (thrown)
            return;




#if UNITY_EDITOR
        touched = Input.GetMouseButtonDown(0);
        threw = Input.GetMouseButtonUp(0);
        moved = Input.GetMouseButton(0);
#elif UNITY_ANDROID || UNITY_IOS
        int touches = Input.touchCount;
        TouchPhase status = Input.GetTouch(0).phase;
        touched = touches == 1 && status == TouchPhase.Began;
        threw = touches == 1 && status == TouchPhase.Ended;
        moved = touches == 1 && status == TouchPhase.Moved;
#endif



        if (touched) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f)) {
                if (AntiVirus && hit.transform == AntiVirus.transform) {
                    holding = true;
                    //AntiVirus.transform.SetParent(null);
                }
            }
        }

        if (threw) {
            
            if (lastMouseY < Input.mousePosition.y && initialThrew) {
                ThrowBall(Input.mousePosition);
                initialThrew = false;
            }
            initialThrew = true;
        }

        if (moved) {
            lastMouseX = Input.mousePosition.x;
            lastMouseY = Input.mousePosition.y;
        }
    }

    void Reset() {
        CancelInvoke();
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.1f, Camera.main.nearClipPlane * 7.5f));
        newPosition = transform.position;
        thrown = holding = false;

        rigidBody.useGravity = false;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.SetParent(Camera.main.transform);
    }

    void OnTouch() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane * 5f;
        newPosition = Camera.main.ScreenToWorldPoint(mousePos);
        AntiVirus.transform.position = Vector3.Lerp(AntiVirus.transform.position, newPosition, 50f * Time.deltaTime);
    }

    void ThrowBall(Vector2 mousePos) {
        rigidBody.useGravity = true;
        float differenceY = (mousePos.y - lastMouseY) / Screen.height * 100;
        speed = throwSpeed * differenceY;
        float x = (mousePos.x / Screen.width) - (lastMouseX / Screen.width);
        x = Mathf.Abs(Input.mousePosition.x - lastMouseX) / Screen.width * 100 * x;
        Vector3 direction = new Vector3(x, 0f, 1f);
        direction = Camera.main.transform.TransformDirection(direction);
        rigidBody.AddForce((direction * speed / 2f) + (Vector3.up * speed));
        holding = false;
        thrown = true;
        //Invoke("Reset", 5.0f);
        StartCoroutine(GameManager.Instance.PlaySound("Scream"));

        virusIsPresent = false;
    }

    public void SpawnAntiVirus(int type /* 0 = red, 1 = blue, 2 = green */) {
        if (AntiVirus) Destroy(AntiVirus);

        Color color = Color.black;

        if (type == 0) color = Color.red; 
        if (type == 1) color = Color.green;
        if (type == 2) color = Color.blue;

        AntiMat.color = color;

        AntiVirus = Instantiate(AntiVirusPrefab);
        AntiVirus.transform.localScale /= 3;
        AntiVirus.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.3f, Camera.main.nearClipPlane * 5f));
        newPosition = AntiVirus.transform.position;
        thrown = holding = false;

        rigidBody = AntiVirus.GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
        AntiVirus.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        AntiVirus.transform.SetParent(Camera.main.transform);

        virusIsPresent = true;
        threw = false;
    }
}