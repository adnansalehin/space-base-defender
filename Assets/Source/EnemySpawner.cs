using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
    public static EnemySpawner Instance;

    void Awake() {
        Instance = this;
    }

    [SerializeField]
    private GameObject enemyShipPrefab;

    public List<Transform> attackers;

	void Start () {
        StartCoroutine(SpawnLoop());
	}
	
	private IEnumerator SpawnLoop() {
        while (true) {
            yield return new WaitForSeconds(3);

            if (attackers.Count > 30) continue;

            var pos = Random.insideUnitSphere * 300;
            var e = Instantiate(enemyShipPrefab, pos, Quaternion.identity) as GameObject;
            attackers.Add(e.transform);
        }
    }
}
