using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementController : MonoBehaviour
{

    [SerializeField]
    private GameObject placedPrefab;

    public GameObject PlacedPrefab
    {
        get {
            return placedPrefab;
        }
        set {
            placedPrefab = value;
        }
    }

    private bool worldPlaced;

    private ARRaycastManager arRaycastManager;

    void Awake() {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    private bool TryGetTouchPosition(out Vector2 touchPosition) {
        if (Input.touchCount > 0) {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;

        return false;
    }

    private void RemovePlanes() {
        ARPlaneManager pm = GetComponent<ARPlaneManager>();
        foreach (ARPlane plane in pm.trackables) {
            plane.gameObject.SetActive(false);
        }
        pm.enabled = false;
    }


    void Update() {
        if (!worldPlaced) {
            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;

            if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)) {
                Pose hitPose = hits[0].pose;
                Instantiate(placedPrefab, hitPose.position, placedPrefab.transform.rotation);
                worldPlaced = true;
                RemovePlanes();
            }
        }
    }


    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
}