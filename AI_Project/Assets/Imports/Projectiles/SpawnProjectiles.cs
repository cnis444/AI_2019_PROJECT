﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    public GameObject firePoint;
    public List<GameObject> vfx = new List<GameObject>();
    public RotateToMouse rotateToMouse;
    public AudioClip sound;

    private GameObject effectToSpawn;
    private float timeToFire = 0;
    private AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        effectToSpawn = vfx[0];
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= timeToFire) {
            timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Projectile>().fireRate;
            SpawnVFX();
        }
    }

    void SpawnVFX() {
        GameObject vfx;

        if (firePoint != null) {
            //vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);
            //if (rotateToMouse != null) {
            //    vfx.transform.localRotation = rotateToMouse.GetRotation();
            //}
            vfx = Instantiate(effectToSpawn, firePoint.transform);
            audioSrc.clip = sound;
            audioSrc.Play();
        }
        else {
            Debug.Log("No fire point!");
        }
    }
}
