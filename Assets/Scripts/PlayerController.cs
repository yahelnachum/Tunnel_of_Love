using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float movementSpeed = 10.0f;

	Vector2 mouseLook;
	Vector2 smoothV;

	public float mouseSensitivity = 5.0f;
	public float mouseSmoothing = 2.0f;

	GameObject boat = null;
	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
		if (inBoat == true) {
			Debug.Log ("boat != null");
		} else {
			Debug.Log ("boat == null");
			playerMovement ();
		}

		playerCamera ();

		if (Input.GetKeyDown ("escape"))
			Cursor.lockState = CursorLockMode.None;
	}

	private void playerMovement(){
		float translation = Input.GetAxis ("Vertical") * movementSpeed;
		float straffe = Input.GetAxis ("Horizontal") * movementSpeed;

		translation *= Time.deltaTime;
		straffe *= Time.deltaTime;

		transform.Translate (straffe, 0, translation);
	}

	Vector2 angle = new Vector2(0f,0f);

	private void playerCamera(){
		Vector2 mouseDelta = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));
		mouseDelta = Vector2.Scale (mouseDelta, new Vector2 (mouseSensitivity * mouseSmoothing, mouseSensitivity * mouseSmoothing));

		smoothV.x = Mathf.Lerp (smoothV.x, mouseDelta.x, 1f / mouseSmoothing);
		smoothV.y = Mathf.Lerp (smoothV.y, mouseDelta.y, 1f / mouseSmoothing);

		mouseLook += smoothV;

		angle += mouseDelta;

		transform.GetChild(0).transform.localRotation = Quaternion.AngleAxis (-angle.y, Vector3.right);
		transform.localRotation = Quaternion.AngleAxis (angle.x, Vector3.up); // line that is screwing up boat stuff
	}

	bool inBoat = false;
	void OnCollisionEnter(Collision col){
		Debug.Log ("collision");
		if (inBoat == false && col.gameObject.name.Equals ("boat")) {
			inBoat = true;
			Debug.Log ("with boat");
			boat = col.gameObject;
			col.gameObject.GetComponent<Boat> ().startRide ();
			transform.position = boat.transform.position + new Vector3(0f, 0.5f, 0f);
			transform.SetParent (boat.transform);
		}
	}
}
