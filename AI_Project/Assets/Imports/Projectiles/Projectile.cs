using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float fireRate;
    public GameObject muzzlePrefab;
    public GameObject projectilePrefab;
    public GameObject hitPrefab;

    // Start is called before the first frame update
    void Start() {
        // Muzzle
        if (muzzlePrefab != null) {
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.parent = transform;
            muzzleVFX.transform.forward = gameObject.transform.forward;
            var psMuzzle = muzzleVFX.GetComponent<ParticleSystem>();
            if (psMuzzle != null) {
                Destroy(muzzleVFX, psMuzzle.main.duration);
            }
            else {
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            }
        }

        // Projectile
        if (projectilePrefab != null) {
            var projVFX = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projVFX.transform.forward = gameObject.transform.forward;
            var psProj = projVFX.GetComponent<ParticleSystem>();
            if (psProj != null) {
                Destroy(projVFX, psProj.main.duration);
            }
            else {
                var psChild = projVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(projVFX, psChild.main.duration);
            }
            var move = projVFX.GetComponent<ProjectileMove>();
            if (move != null) {
                move.speed = speed;
                move.hitPrefab = hitPrefab;
            }
        }

        Destroy(gameObject, 10);
    }
}
