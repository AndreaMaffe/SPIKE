using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiattaformaAppesaChainAdapter : MonoBehaviour {

    GameObject chainSx;
    GameObject chainDx;

	// Use this for initialization
	void Start ()
    {
        chainSx = transform.Find("CatenaSx").gameObject;
        chainDx = transform.Find("CatenaDx").gameObject;
        AdaptChainLenght();
    }

    void AdaptChainLenght() {
        GameObject astaOrizzontale = GameObject.Find("HorizontalBar");
        float yOffset =  astaOrizzontale.transform.position.y - transform.position.y ;
        chainSx.GetComponent<SpriteRenderer>().size = new Vector2(chainSx.GetComponent<SpriteRenderer>().size.x, yOffset);
        chainDx.GetComponent<SpriteRenderer>().size = chainSx.GetComponent<SpriteRenderer>().size;

    }


}
