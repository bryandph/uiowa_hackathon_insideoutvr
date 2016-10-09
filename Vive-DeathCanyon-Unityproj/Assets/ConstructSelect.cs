using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConstructSelect : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
		Debug.Log ("return to construct");
		SceneManager.LoadScene ("TheConstruct");
	}
}
