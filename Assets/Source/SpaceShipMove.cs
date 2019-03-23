using UnityEngine;
using System.Collections;

public class SpaceShipMove : MonoBehaviour {

	public float speed;
	private Transform tr;

	void Start () {
		this.tr = this.transform;
	}
	
	void Update () {
		tr.Translate(Vector3.forward * this.speed);
	}
}
