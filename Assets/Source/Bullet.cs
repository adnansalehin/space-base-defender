using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    [SerializeField]
    private bool isEnemy;

	void Start () {
        Destroy(gameObject, 3);
	}

    void OnTriggerEnter(Collider col) {
        if (!isEnemy && col.tag == "Enemy") {
            col.GetComponent<BotAttackController>().ApplyDamage(100);
        } else if (isEnemy && col.tag == "MainBase") {
            col.GetComponent<Airbase>().ApplyDamage(1);
        }
        Destroy(gameObject);
    }
}
