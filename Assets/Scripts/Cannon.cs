using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public float rateOfFire;
    public GameObject bullet;

    private float timer;

	// Use this for initialization
	void Start () {

        timer = rateOfFire;
	}
	
	// Update is called once per frame
	void Update () {

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Instantiate(bullet, this.transform.position, this.transform.rotation);
            timer = rateOfFire;
        }
	}
}
