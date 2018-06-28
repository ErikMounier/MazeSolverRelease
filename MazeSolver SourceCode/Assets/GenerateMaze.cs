using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMaze : MonoBehaviour {

    GameObject wallprefab;

    [SerializeField] float SizeX_Pos;
    [SerializeField] float SizeX_Neg;
    [SerializeField] float SizeY_Pos;
    [SerializeField] float SizeY_Neg;

    float randomplacement;

    Vector3 PlaceCoords;

	// Use this for initialization
	void Start () {
        wallprefab = Resources.Load("Wall", typeof(GameObject)) as GameObject;
        PlaceCoords = new Vector3(SizeX_Neg,1,SizeY_Neg);
        for (float i = SizeY_Pos; i <= SizeY_Neg; i++)
        {
            PlaceCoords.x = SizeX_Neg;
            for (float j = SizeX_Pos; j <= SizeX_Neg; j++)
            {
                
                randomplacement = Random.Range(-1, 2);
                if (randomplacement == 1)
                {
                    
                    Instantiate(wallprefab, PlaceCoords, transform.rotation, transform);
                }
                PlaceCoords.x -= 1;
            }
            PlaceCoords.z -= 1;
        }
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
