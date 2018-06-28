using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPoint : MonoBehaviour {

    private GameObject StartPoint;
    private GameObject EndPoint;

    float S_Cost;
    float E_Cost;
    float T_Cost;
    bool Colliding;
	// Use this for initialization
	void Start () {
        StartPoint = GameObject.Find("StartPoint");
        EndPoint = GameObject.Find("TARGET");
        if(EndPoint != null)
        {
            Debug.Log(EndPoint.transform.position);
        }

        S_Cost = Vector3.Distance(StartPoint.transform.position, transform.position);
        E_Cost = Vector3.Distance(EndPoint.transform.position, transform.position);
        T_Cost = S_Cost + E_Cost;
	}

    

    // Update is called once per frame
    void Update () {
       // Debug.Log(EndPoint.transform.position + " EndPoint");
	}

    public float GiveTileCost()
    {
        return T_Cost;
    }

    public float GiveECost()
    {
        return E_Cost;
    }

    public bool Iscolliding()
    {
        return Colliding;
    }

   


}
