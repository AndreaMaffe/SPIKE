using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementProva : MonoBehaviour {

	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = new Vector3(transform.position.x + Time.deltaTime * 2, transform.position.y, 0);
	}
}
