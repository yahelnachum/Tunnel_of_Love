using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float movementSpeed = 10.0f;

	Vector2 mouseLook;
	Vector2 smoothV;

	public float mouseSensitivity = 5.0f;
	public float mouseSmoothing = 2.0f;

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
			playerMovement_ForwardBackward ();
			playerMovement_StraffeLeftRight ();
		}

		playerCamera_LeftRight ();
		playerCamera_UpDown ();

		if (Input.GetKeyDown ("escape"))
			Cursor.lockState = CursorLockMode.None;
	}

	private void playerMovement_ForwardBackward(){

		float translation = Input.GetAxis ("Vertical") * movementSpeed;

		translation *= Time.deltaTime;
		transform.Translate (0f, 0f, translation);
	}

	private void playerMovement_StraffeLeftRight(){

		float straffe = Input.GetAxis ("Horizontal") * movementSpeed;

		straffe *= Time.deltaTime;
		transform.Translate (straffe, 0f, 0f);
	}

	float cameraAngle_UpDown = 0f;
	float CAMERA_ANGLE_UP_DOWN_LIMIT = 90f;
	private void playerCamera_UpDown(){
		cameraAngle_UpDown += Input.GetAxisRaw ("Mouse Y") * mouseSensitivity;
		cameraAngle_UpDown = Mathf.Clamp (cameraAngle_UpDown, -CAMERA_ANGLE_UP_DOWN_LIMIT, CAMERA_ANGLE_UP_DOWN_LIMIT);
		transform.GetChild(0).transform.localRotation = Quaternion.AngleAxis (-cameraAngle_UpDown, Vector3.right);
	}

	float cameraAngle_LeftRight = 0f;

	private void playerCamera_LeftRight(){
		cameraAngle_LeftRight += Input.GetAxisRaw ("Mouse X")  * mouseSensitivity;
		transform.localRotation = Quaternion.AngleAxis (cameraAngle_LeftRight, Vector3.up);
	}

	bool inBoat = false;
	void OnCollisionEnter(Collision col){
		Debug.Log ("collision");
		if (inBoat == false && col.gameObject.name.Equals ("boat")) {
			inBoat = true;
			Debug.Log ("with boat");
			GameObject gameObjectBoat = col.gameObject;
			Boat scriptBoat = col.gameObject.GetComponent<Boat> ();
			scriptBoat.setRider (this.gameObject);
			scriptBoat.setMove (true);
			transform.position = scriptBoat.transform.position + new Vector3(0f, 0.5f, 0f);
			transform.SetParent (scriptBoat.transform);
		}
	}

	public void setInBoat(bool inBoat){
		this.inBoat = inBoat;
	}
}
