using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking;

public class SceneController :  MonoBehaviour {

	static public SceneController instance = null;
	public bool isTangoConnected = false;

	// Use this for initialization
	void Start () {
		
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this.gameObject);
		} else {
			DestroyImmediate (this);
		}

		Screen.sleepTimeout = SleepTimeout.NeverSleep;

	}


}