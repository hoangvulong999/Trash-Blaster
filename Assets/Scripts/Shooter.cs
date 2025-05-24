using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {
    [Header("Shooting")] public GameObject ProjectilePrefab; //Projectile to instantiate
    public Transform fireTransform;
    public float fireRate;
    public float damage;
    public float ProjectileSpeed;

    float shooterTimer = 0f;

    [HideInInspector]
    public List<GameObject> ProjectilePool; //for object pooling -> better fps

    public float multiplier = 1;

    public GameObject laser;

    // Start is called before the first frame update
    void Start() {
        ProjectilePool = new List<GameObject>();

    }

    // Update is called once per frame
    void Update() {
        if (GameManager._instance.isPaused) {
            if (laser != null)
                laser.SetActive(false);

            return;
        }

        if (laser != null)
            laser.SetActive(true);

        Shoot();
    }

    /// <summary>
    /// Sho0ting Projectiles, called in every frame
    /// </summary>
    void Shoot() {
        shooterTimer += Time.deltaTime;

        //If you cant shoot return
        if (shooterTimer < 1f / (fireRate * (1 + GameManager._instance.atkSpeedBonus / 100f))) return;

        shooterTimer = 0;

        //Getting Projectile from pool
        var g = GetProjectileFromPool();
        g.SetActive(true);
        g.transform.position = fireTransform.position;
        g.transform.eulerAngles = transform.eulerAngles;

        g.GetComponent<Projectile>().damage = damage * multiplier;
        g.GetComponent<Projectile>().baseSpeed = ProjectileSpeed;

        float angle = transform.parent.GetComponent<ItemSlot>().rotation + Player._instance.transform.localEulerAngles.z + 90;
        g.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * ProjectileSpeed;
    }


    /// <summary>
    /// if you have a free Projectile returns it, and if all Projectile are in usage instantiate one
    /// </summary>
    /// <returns></returns>
    GameObject GetProjectileFromPool() {
        foreach (GameObject g in ProjectilePool)
            if (g.activeSelf == false)
                return g;

        var newProjectile = Instantiate(ProjectilePrefab);
        ProjectilePool.Add(newProjectile);

        return newProjectile;
    }
}
