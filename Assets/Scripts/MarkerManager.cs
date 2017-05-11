using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarkerManager : MonoBehaviour {

	private List<Marker> markers = null;

	// Use this for initialization
	void Start () {
		GameObject root = GameObject.Find ("tunnelOfLove_layout");
		markers = getMarkers (root);
		markers = orderMarkers (markers);
		markers = calculateSpeed (markers);
		drawPath (markers);
	}
		
	// Update is called once per frame
	void Update () {
		
	}

	private static int counter = 0;
	public Marker getNextMarker(){
		if (markers.Count > 0) {

			Marker currentMarker = markers [counter % markers.Count];
			counter++;

			return currentMarker;
		}

		return null;
	}

	private void drawArrowDebug(Vector3 startPosition, Vector3 endPosition){
		Debug.DrawLine (startPosition, endPosition, Color.red, float.MaxValue );
		Vector3 arrowReverseDirection = (startPosition - endPosition).normalized;
		Vector3 arrowHead01 = Quaternion.Euler (0f, 30f, 0f) * arrowReverseDirection; 
		Vector3 arrowHead02 = Quaternion.Euler (0f, -30f, 0f) * arrowReverseDirection;
		Debug.DrawLine (endPosition, endPosition + arrowHead01, Color.red, float.MaxValue );
		Debug.DrawLine (endPosition, endPosition + arrowHead02, Color.red, float.MaxValue );
		//Debug.DrawLine (startPosition, endPosition, Color.red, float.MaxValue );
	}

	private void drawPath(List<Marker> markers){
		Debug.Log (markers.Count);
		for (int i = 1; i < markers.Count; i++) {
			drawArrowDebug (markers [i - 1].transform.position, markers [i].transform.position);
		}

		if (markers.Count > 1) {
			drawArrowDebug (markers [markers.Count - 1].transform.position, markers [0].transform.position);
		}
	}

	private int findIndexOfFirstMarker(List<Marker> markers){
		int index = 0;
		for (int i = 0; i < markers.Count; i++) {
			if (markers [i].isFirst()) {
				index = i;
				break;
			}

		}
		return index;
	}

	private int findIndexOfMarkerWithSmallestDistance(List<Marker> markers, Marker relativeMarker){
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

	private List<Marker> orderMarkers(List<Marker> markers){
		List<Marker> markersOrdered = new List<Marker> ();

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

	private List<Marker> getMarkers(GameObject root){
		List<Marker> list = new List<Marker> ();
		if (root.name.Contains ("marker"))
			list.Add (root.GetComponent<Marker>());
		else {
			for (int i = 0; i < root.transform.childCount; i++) {
				list.AddRange(getMarkers (root.transform.GetChild (i).gameObject));
			}
		}
		return list;
	}

	private List<Marker> calculateSpeed(List<Marker> markers){

		for (int i = 0; i < markers.Count; i++) {
			markers [i].setSpeed (1f);
		}

		return markers;
	}
}
