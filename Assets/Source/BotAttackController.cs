using UnityEngine;
using System.Collections;

public class BotAttackController : MonoBehaviour {

    private Transform mainBase;

    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private Blaster[] blasters;

    [SerializeField]
    private GameObject destroyEffect;

    [SerializeField]
    private GameObject smokeTrail;

    private Transform tr;
    private Rigidbody rb;
    private Vector3 targetPoint;
    private bool attack = true;

    private float HP = 500;

	void Start () {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        mainBase = GameObject.FindGameObjectWithTag("MainBase").transform;
	}
	
	void Update () {
        if (HP <= 0) return;

        Vector3 tg = Vector3.zero;
        if (attack) tg = mainBase.position;
        else tg = targetPoint;

        var rot = tr.rotation;
        tr.LookAt(tg);
        tr.rotation = Quaternion.RotateTowards(rot, tr.rotation, rotationSpeed * Time.deltaTime);
        ResetTarget();

        RaycastHit hit;
        if(Physics.Raycast(tr.position, tr.forward, out hit, 350f) &&
            hit.collider.tag == "MainBase") {
            foreach (var b in blasters) b.Shoot();
        }
    }

    void FixedUpdate() {
        if (HP <= 0) maxSpeed -= .1f;

        float currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;
        if (currentSpeed < maxSpeed) {
            rb.velocity = tr.forward * maxSpeed;
        }
    }

    public void ApplyDamage(float damage) {
        HP -= damage;

        if (HP <= 0) StartCoroutine(Destroy());
    }

    private IEnumerator Destroy() {
        rb.useGravity = true;
        var e = Instantiate(destroyEffect, tr.position, Quaternion.identity) as GameObject;
        smokeTrail.SetActive(true);
        Destroy(e, 5);

        EnemySpawner.Instance.attackers.Remove(tr);
        GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(15);
        smokeTrail.transform.SetParent(null);
        Destroy(smokeTrail, 30);
        Destroy(gameObject);
        Airbase.Score += 50;
    }

    private void ResetTarget() {
        if (targetPoint != Vector3.zero) {
            if (Vector3.Distance(targetPoint, tr.position) < 10) {
                attack = true;
                targetPoint = Vector3.zero;
            }
        } else {
            if (Vector3.Distance(mainBase.position, tr.position) < 200) {
                attack = false;
                targetPoint = mainBase.position - tr.forward * 500;
            }
        }
    }
}
