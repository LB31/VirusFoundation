using UnityEngine;
using System.Collections;

//If you're using please put my name in the credit, or link my Youtube page. :)

public class Pokeball : MonoBehaviour {
	[SerializeField]
	private float throwSpeed = 35f;
	private float speed;
	private float lastMouseX, lastMouseY;

	private bool thrown, holding;

	private Rigidbody _rigidbody;
	private Vector3 newPosition;

	void Start() {
		_rigidbody = GetComponent<Rigidbody> ();
		Reset ();
	}

	void Update() {
		if (holding)
			OnTouch ();

		if (thrown)
			return;

        bool touched = false;
        bool threw = false;
        bool moved = false;



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
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 100f)) {
				if (hit.transform == transform) {
					holding = true;
					transform.SetParent (null);
				}
			}
		}

		if(threw) { 
			if (lastMouseY < Input.mousePosition.y) {
				ThrowBall (Input.mousePosition);
			}
		}

		if(moved) {
			lastMouseX = Input.mousePosition.x;
			lastMouseY = Input.mousePosition.y;
		}
	}

	void Reset(){
		CancelInvoke ();
		transform.position = Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.1f, Camera.main.nearClipPlane * 7.5f));
		newPosition = transform.position;
		thrown = holding = false;

		_rigidbody.useGravity = false;
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.angularVelocity = Vector3.zero;
		transform.rotation = Quaternion.Euler (0f, 0f, 0f);
		transform.SetParent (Camera.main.transform);
	}

	void OnTouch() {
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = Camera.main.nearClipPlane * 7.5f;

		newPosition = Camera.main.ScreenToWorldPoint (mousePos);

		transform.localPosition = Vector3.Lerp (transform.localPosition, newPosition, 50f * Time.deltaTime);
	}

	void ThrowBall(Vector2 mousePos) {
		_rigidbody.useGravity = true;

		float differenceY = (mousePos.y - lastMouseY) / Screen.height * 100;
        
		speed = throwSpeed * differenceY;

		float x = (mousePos.x / Screen.width) - (lastMouseX / Screen.width);
		x = Mathf.Abs (Input.mousePosition.x - lastMouseX) / Screen.width * 100 * x;

		Vector3 direction = new Vector3 (x, 0f, 1f);
		direction = Camera.main.transform.TransformDirection (direction);

		_rigidbody.AddForce((direction * speed / 2f) + (Vector3.up * speed));

		holding = false;
		thrown = true;

		Invoke ("Reset", 5.0f);
	}
}