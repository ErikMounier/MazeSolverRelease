using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Fps : MonoBehaviour {
    Text text;
    float fps;
	// Use this for initialization
	void Start () {
        text = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        fps += (Time.unscaledDeltaTime - fps) * 0.1f;
        text.text = fps.ToString();
	}
}
