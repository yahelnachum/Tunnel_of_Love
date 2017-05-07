using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour {

	MarkerManager manager;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("MarkerManager").GetComponent<MarkerManager> ();
	}
	

	private static GameObject nextMarker = null;
	private static GameObject previousMarker = null;
	private const float speed = 0.01f;
	private bool move = false;
	// Update is called once per frame
	void Update () {
		if (move) {
			if (nextMarker == null || previousMarker == null) {
				previousMarker = manager.getNextMarker ();
				nextMarker = manager.getNextMarker ();

				transform.position = previousMarker.transform.position;

			}

			Vector3 newBoatPosition = transform.position + (nextMarker.transform.position - previousMarker.transform.position).normalized * speed;

			float interpolation = Vector3.Distance (newBoatPosition, previousMarker.transform.position) / Vector3.Distance (nextMarker.transform.position, previousMarker.transform.position);

			while (interpolation > 1f) {
				previousMarker = nextMarker;
				nextMarker = manager.getNextMarker ();

				float newSpeed = Vector3.Distance (newBoatPosition, previousMarker.transform.position);
				newBoatPosition = previousMarker.transform.position + (nextMarker.transform.position - previousMarker.transform.position).normalized * newSpeed;

				interpolation = Vector3.Distance (newBoatPosition, previousMarker.transform.position) / Vector3.Distance (nextMarker.transform.position, previousMarker.transform.position);
			}

			Quaternion newBoatRotation = Quaternion.Slerp (previousMarker.transform.rotation, nextMarker.transform.rotation, interpolation);

			transform.position = newBoatPosition;
			transform.rotation = newBoatRotation;
		}
	}

	public void startRide(){
		Debug.Log ("starting ride");
		move = true;
	}
}
