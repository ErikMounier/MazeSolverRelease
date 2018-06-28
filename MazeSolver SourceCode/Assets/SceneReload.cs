using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneReload : MonoBehaviour {
    Scene GetScene;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R))
        {
            GameObject.Find("ListHandeler").GetComponent<PointHandeler>().ClearList();
            SceneManager.LoadScene(0);
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
	}
}
