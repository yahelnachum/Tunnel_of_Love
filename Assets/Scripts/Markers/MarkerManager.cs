using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarkerManager : MonoBehaviour {

	private List<GameObject> markers = null;
	private string firstMarkerName = "heart_ride_marker_0001";

	// Use this for initialization
	void Start () {
		GameObject root = GameObject.Find ("tunnelOfLove_layout");
		List<GameObject> markers = getMarkers (root);
		markers = orderMarkers (markers);
		drawPath (markers);
	}
	
	// Update is called once per frame
	void Update () {

	}

	private void drawArrowDebug(Vector3 startPosition, Vector3 endPosition){
		Debug.DrawLine (startPosition, endPosition, Color.red, float.MaxValue );
		//Vector3 arrowHead01 = 
		//Debug.DrawLine (startPosition, endPosition, Color.red, float.MaxValue );
	}

	private void drawPath(List<GameObject> markers){
		for (int i = 1; i < markers.Count; i++) {
			drawArrowDebug (markers [i - 1].transform.position, markers [i].transform.position);
		}

		if (markers.Count > 1) {
			drawArrowDebug (markers [markers.Count - 1].transform.position, markers [0].transform.position);
		}
	}

	private int findIndexOfFirstMarker(List<GameObject> markers){
		int index = 0;
		for (int i = 0; i < markers.Count; i++) {
			if (markers [i].name.Equals ("heart_ride_marker_0001")) {
				index = i;
				break;
			}

		}
		return index;
	}

	private int findIndexOfMarkerWithSmallestDistance(List<GameObject> markers, GameObject relativeMarker){
		int smallestDistanceIndex = 0;
		float smallestDistance = float.MaxValue;

		for (int i = 0; i < markers.Count; i++) {
			float distance = Vector3.Distance(relativeMarker.transform.position, markers[i].transform.position);
			if (smallestDistance > distance) {
				smallestDistance = distance;
				smallestDistanceIndex = i;
			}
		}

		return smallestDistanceIndex;
	}

	private List<GameObject> orderMarkers(List<GameObject> markers){
		List<GameObject> markersOrdered = new List<GameObject> ();

		int indexOfFirstMarker = findIndexOfFirstMarker (markers);
		markersOrdered.Add (markers[indexOfFirstMarker]);
		markers.RemoveAt (indexOfFirstMarker);

		int orderedIndex = 0;
		while (markers.Count != 0) {
			int smallestDistanceIndex = findIndexOfMarkerWithSmallestDistance (markers, markersOrdered [orderedIndex]);

			markersOrdered.Add (markers [smallestDistanceIndex]);
			markers.RemoveAt (smallestDistanceIndex);
			orderedIndex++;
		}

		return markersOrdered;
	}

	private List<GameObject> getMarkers(GameObject root){
		List<GameObject> list = new List<GameObject> ();
		if (root.name.Contains ("marker"))
			list.Add (root);
		else {
			for (int i = 0; i < root.transform.childCount; i++) {
				list.AddRange(getMarkers (root.transform.GetChild (i).gameObject));
			}
		}
		return list;
	}
}
