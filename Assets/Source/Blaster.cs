using UnityEngine;
using System.Collections;

public class Blaster : MonoBehaviour {

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private GameObject shootEffect;

    [SerializeField]
    private float cooldownMax = .2f;

    private new Transform camera;
    private bool isOnCoolDown;

	void Start () {
        camera = Camera.main.transform;
	}

    public void Shoot() {
        if (isOnCoolDown) return;

        if (Time.timeScale > 0)
        {

            var effect = Instantiate(shootEffect, transform.position, Quaternion.identity) as GameObject;
            effect.transform.parent = transform;
            Destroy(effect, .1f);

            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 80, ForceMode.Impulse);

            RaycastHit hit;
            if (Physics.Raycast(camera.position, camera.forward, out hit))
            {
                bullet.transform.LookAt(hit.point);
            }

            isOnCoolDown = true;
            StartCoroutine(Cooldown());
        }
    }
	
	private IEnumerator Cooldown() {
        yield return new WaitForSeconds(Random.Range(.1f, cooldownMax));
        isOnCoolDown = false;
    }
}
