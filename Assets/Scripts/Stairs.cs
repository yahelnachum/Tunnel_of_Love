using UnityEngine;
using System.Collections;

public class Stairs : MonoBehaviour {

	int numberOfStairs = 10;

	// Use this for initialization
	void Start () {
		for (int i = 1; i < numberOfStairs; i++) {
			GameObject stair = GameObject.CreatePrimitive (PrimitiveType.Cube);
			stair.transform.position = this.transform.position + new Vector3 (0f, this.transform.localScale.y * i, this.transform.localScale.z * i);
			stair.transform.localScale = this.transform.localScale;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
