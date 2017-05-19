using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boat : MonoBehaviour {

	MarkerManager manager;
	GameObject rider;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("MarkerManager").GetComponent<MarkerManager> ();
		for (int i = 0; i < 30; i++) {
			slowdownSpeedChangeQueue.Enqueue (flatSpeed);
		}
	}
	

	private static Marker nextMarker = null;
	private static Marker previousMarker = null;
	private const float flatSpeed = 0.05f;
	private const float downwardSpeed = 0.1f;
	private const float upwardSpeed = 0.01f;
	private bool move = false;
	private bool beforeHalfway = true;
	private Queue<float> slowdownSpeedChangeQueue = new Queue<float> ();
	// Update is called once per frame
	void Update () {
		if (move) {
			if (nextMarker == null || previousMarker == null) {
				previousMarker = manager.getNextMarker ();
				nextMarker = manager.getNextMarker ();

				transform.position = previousMarker.transform.position;

			}

			float speedUpDown = 0f;
			if (transform.rotation.eulerAngles.x < 90f) {
				speedUpDown = (1f - transform.rotation.eulerAngles.x / 45f) * (flatSpeed - upwardSpeed) + upwardSpeed;
			} else {
				speedUpDown = (1f - ((transform.rotation.eulerAngles.x - (360f - 45f)) / 45f)) * (downwardSpeed - flatSpeed) + flatSpeed;
			}

			slowdownSpeedChangeQueue.Enqueue (speedUpDown);

			Vector3 newBoatPosition = transform.position + (nextMarker.transform.position - previousMarker.transform.position).normalized * slowdownSpeedChangeQueue.Dequeue();

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
