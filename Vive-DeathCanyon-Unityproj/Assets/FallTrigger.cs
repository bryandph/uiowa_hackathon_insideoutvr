using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour {

	private GameObject localPlayerObject;
	private Rigidbody localRigidBody;

	// Use this for initialization
	void OnTriggerExit(Collider other) {
		localPlayerObject = GameObject.FindGameObjectWithTag ("Player");
		localRigidBody = localPlayerObject.AddComponent<Rigidbody> ();
		//localRigidBody.useGravity = true;
	}
}
