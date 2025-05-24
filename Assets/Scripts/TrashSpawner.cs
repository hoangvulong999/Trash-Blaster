using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour {
    public static TrashSpawner _instance;

    [SerializeField] GameObject[] trashPrefabs;  // Array of different trash prefabs

    public int trashToSpawn, trashDestroyed;
    public float timeBetweenSpawns = 1f;

    public List<Trash> trashObjects;  // List to keep track of spawned trash

    private void Awake() {
        _instance = this;
    }

    public void StartWave() {
        trashToSpawn = Mathf.Min(15 + LevelManager._instance.currentLevel * 3, 40);
        trashDestroyed = 0;

        trashObjects = new List<Trash>();

        coroutine = StartCoroutine(SpawnCoroutine());
    }

    public Coroutine coroutine;

    IEnumerator SpawnCoroutine() {
        for (int i = 0; i < trashToSpawn; i++) {
            SpawnTrash();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    void SpawnTrash() {
        GameObject trash = Instantiate(trashPrefabs[Random.Range(0, trashPrefabs.Length)]);
        trash.transform.position = GetTrashSpawnPosition();

        var trashComponent = trash.GetComponent<Trash>();
        trashComponent.hp = GetTrashHealth();

        trashObjects.Add(trashComponent);
    }

    int GetTrashHealth() {
        int lvl = LevelManager._instance.currentLevel;
        float multiplier = 1 + lvl / 10f;
        return 3 + (int)Random.Range(lvl * 5 * 0.7f * multiplier, lvl * 5 * multiplier);
    }

    public void TrashDestroyed(Trash trash, bool planet) {
        trashObjects.Remove(trash);

        trashDestroyed++;
        LevelManager._instance.SetFill(trashDestroyed / (float)trashToSpawn);

        if (planet)
            return;

        if (trashDestroyed >= trashToSpawn) {
            LevelManager._instance.LevelCompleted(true);
        }
    }

    Vector3 GetTrashSpawnPosition() {
        int n = Random.Range(0, 4);

        float x = 3.9f, y = 6f;

        if (n == 0) {
            //Left side
            return new Vector3(-x, Random.Range(y, -y), 0);
        } else if (n == 1) {
            //Right side
            return new Vector3(x, Random.Range(y, -y), 0);
        } else if (n == 2) {
            //Up side
            return new Vector3(Random.Range(-x, x), y, 0);
        } else {
            //Bottom side
            return new Vector3(Random.Range(-x, x), -y, 0);
        }
    }
}
