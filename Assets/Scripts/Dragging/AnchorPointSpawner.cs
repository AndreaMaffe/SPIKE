using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Alignment { Horizontal, Vertical };

public class AnchorPointSpawner : MonoBehaviour {   

    public GameObject anchorPoint;
    private List<AnchorPoint> anchorPoints;
    public int anchorPointAmount;
    public float anchorPointDistance;
    [Header("allineamento dei punti di ancoraggio")]
    public Alignment alignment;
    [Header("Top, Side o Platform")]
    public AnchorPointPosition position;
    [Header("Posizine iniziale del primo punto di ancoraggioo")]
    public Transform startingPosition;

	// Use this for initialization
	void Start () {
        anchorPoints = new List<AnchorPoint>();

        for (int i = 0; i < anchorPointAmount; i++) {
            GameObject anchorPointInstance = Instantiate(anchorPoint, startingPosition, true);
            anchorPoints.Add(anchorPointInstance.GetComponent<AnchorPoint>());

            SetAnchorPointPosition(anchorPointInstance, i);
            SetAnchorPointData(i);

        }
	}

    void SetAnchorPointPosition(GameObject instance, int i) {
        if (alignment == Alignment.Horizontal)
        {
            instance.transform.localPosition = new Vector3(i * anchorPointDistance, 0, 0);
        }
        else if (alignment == Alignment.Vertical) {
            instance.transform.localPosition = new Vector3(0, i * anchorPointDistance, 0);
        }
    }

    void SetAnchorPointData(int i)
    {
        if (i == 0 || i == anchorPointAmount - 1)
            anchorPoints[i].SetEdge(true);
        else
            anchorPoints[i].SetEdge(false);

        anchorPoints[i].SetOccupied(false);
        anchorPoints[i].SetPosition(position);
    }
}
