using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	public GameObject headPrefab;
	public GameObject tangoPrefab;
	public GameObject instanceHead;
	public Transform tangoReference = null;
	private bool isTangoDevice = false;
	private Transform childHeadTransform;
	static public PlayerController instance = null;

	void Start() {
		DontDestroyOnLoad (this.gameObject);
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>().farClipPlane = 2000f;
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ().nearClipPlane = 0.1f;
	}

	public override void OnStartLocalPlayer() {
		Debug.Log ("PlayerController : OnStartLocalPlayer()");

		if (isLocalPlayer && isServer) {
			Debug.Log ("isLocalPlayer && isServer");
			instanceHead = (GameObject)Instantiate (headPrefab, this.transform, false);
			instanceHead = instanceHead.GetComponentInChildren<GvrHead> ().gameObject;
			instanceHead.tag = "Head";
			this.gameObject.tag = "Player";
			Debug.Log ("instanceHead is: ");
			Debug.Log(instanceHead);
			instance = this;
		} else if (isLocalPlayer && !isServer) {
			Debug.Log ("isLocalPlayer && !isServer");
			isTangoDevice = true;
			GameObject temporary = (GameObject)Instantiate (tangoPrefab, Vector3.zero, Quaternion.identity);
			tangoReference = temporary.transform;
			//instanceHead = temporary;
			//GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().setTangoReference (this.transform);
			Debug.Log ("Is Tango device");
			CmdSetTangoReference ();
		} else if (!isServer && !isLocalPlayer) {
			Debug.Log ("!isServer && !isLocalPlayer");
			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().setTangoReference (this.GetComponentInChildren<Transform> ());
		}
		//CmdSetTangoReference ();
		if (tangoReference == null) {
			Debug.Log ("Tango Reference is null");
		} else {
			Debug.Log ("Tango reference is set");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (tangoReference == null) {
			return;
		}
		if (isTangoDevice) {
			this.transform.position = tangoReference.position;
		} else {
			instanceHead.transform.position = tangoReference.position;
			//Debug.Log (instanceHead.transform.position);
		}
		//Debug.Log (this.transform.position);
		//Debug.Log (tangoReference.position);
	}

	public void setTangoReference(Transform reference) {
		this.tangoReference = reference;
	}

	[Command]
	public void CmdSetTangoReference() {
		Debug.Log ("COMMAND SET REFERENCE");
		//GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().setTangoReference (this.transform);
		PlayerController.instance.setTangoReference(this.transform);
	}
}
