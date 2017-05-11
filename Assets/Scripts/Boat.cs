using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour {

	MarkerManager manager;
	GameObject rider;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("MarkerManager").GetComponent<MarkerManager> ();
	}
	

	private static Marker nextMarker = null;
	private static Marker previousMarker = null;
	private const float speed = 0.05f;
	private bool move = false;
	private bool beforeHalfway = true;
	// Update is called once per frame
	void Update () {
		if (move) {
			if (nextMarker == null || previousMarker == null) {
				previousMarker = manager.getNextMarker ();
				nextMarker = manager.getNextMarker ();

				transform.position = previousMarker.transform.position;

			}

			Vector3 newBoatPosition = transform.position + (nextMarker.transform.position - previousMarker.transform.position).normalized * speed * previousMarker.getSpeed ();

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

			if (beforeHalfway && previousMarker.name.Equals("heart_ride_marker_035")) {
				move = false;
				beforeHalfway = false;

				rider.transform.SetParent (null);
				rider.transform.position = this.transform.position + new Vector3 (-2f, 1f, 0f);
				PlayerController playerController = rider.GetComponent<PlayerController> ();
				playerController.setInBoat (false);
			}
		}
	}

	public void setMove(bool move){
		this.move = move;
	}

	public void setRider(GameObject rider){
		this.rider = rider;
	}
}
