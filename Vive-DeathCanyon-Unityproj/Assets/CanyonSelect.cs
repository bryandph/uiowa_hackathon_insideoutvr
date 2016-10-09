using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanyonSelect : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
		Debug.Log ("Collision selection");
		SceneManager.LoadScene ("canyonofdeath");
	}
}
