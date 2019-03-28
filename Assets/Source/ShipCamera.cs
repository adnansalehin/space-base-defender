using UnityEngine;
using System.Collections;

public class ShipCamera : MonoBehaviour {
    public Transform target;
    public Transform place;

    private float distance = .2f;
    private float maxDistance = 4f;
    private float minDistance = .05f;
    private float zoomRate = 300;

    private Transform tr;

    void Start() {
        tr = transform;
        tr.parent = null;
    }

    void FixedUpdate() {
        distance -= (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomRate * Mathf.Abs(distance);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        Quaternion newRot = Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, 0);
        Quaternion rotation = Quaternion.Lerp(tr.rotation, newRot, Time.deltaTime * 10);

        Vector3 newPos = place.position - (rotation * Vector3.forward * distance);
        tr.rotation = rotation;
        if(Time.timeScale > 0) {
            tr.position = Vector3.Lerp(tr.position, newPos, Time.deltaTime * 30);
        }
    }
}
