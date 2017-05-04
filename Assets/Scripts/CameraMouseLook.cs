using UnityEngine;
using System.Collections;

public class CameraMouseLook : MonoBehaviour {

	Vector2 mouseLook;
	Vector2 smoothV;

	public float sensitivity = 5.0f;
	public float smoothing = 2.0f;

	GameObject character;

	// Use this for initialization
	void Start () {
		character = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		var mouseDelta = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));
		mouseDelta = Vector2.Scale (mouseDelta, new Vector2 (sensitivity * smoothing, sensitivity * smoothing));

		smoothV.x = Mathf.Lerp (smoothV.x, mouseDelta.x, 1f / smoothing);
		smoothV.y = Mathf.Lerp (smoothV.y, mouseDelta.y, 1f / smoothing);

		mouseLook += smoothV;

		transform.localRotation = Quaternion.AngleAxis (-mouseLook.y, Vector3.right);
		character.transform.localRotation = Quaternion.AngleAxis (mouseLook.x, character.transform.up);
	}
}
