using UnityEngine;
using System.Linq;
using System.Collections;

public class BotDefendController : MonoBehaviour {

    [SerializeField]
    public float maxSpeed;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private Blaster[] blasters;

    private Transform target;
    private Transform tr;
    private Rigidbody rb;

	void Start () {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(SetTargetLoop());
	}
	
	void Update () {
        var rot = tr.rotation;
        tr.LookAt(target);
        tr.rotation = Quaternion.RotateTowards(rot, tr.rotation, rotationSpeed * Time.deltaTime);

        RaycastHit hit;
        if (Physics.Raycast(tr.position, tr.forward, out hit, 150f) &&
            hit.collider.tag == "Enemy") {
            foreach (var b in blasters) b.Shoot();
        }
    }

    void FixedUpdate() {
        float currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;
        if (currentSpeed < maxSpeed) {
            rb.velocity = tr.forward * maxSpeed;
        }
    }

    private IEnumerator SetTargetLoop() {
        while (true) {
            yield return new WaitForSeconds(2);
            if (target == null) SetTarget();
        }
    }

    private void SetTarget() {
        var targets = EnemySpawner.Instance.attackers.Where(
            a => Vector3.Distance(tr.position, a.position) < 350).ToList();

        if (targets.Count == 0) return;
        target = targets[Random.Range(0, targets.Count - 1)];
    }
}
