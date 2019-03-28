using UnityEngine;
using System.Collections;

public class SpaceShipMove : MonoBehaviour {

	public float speed;
	private Transform tr;

	void Start () {
		this.tr = this.transform;
	}
	
	void Update () {
        if(Time.timeScale > 0) {
            tr.Translate(Vector3.forward * this.speed);
        }
	}
}
