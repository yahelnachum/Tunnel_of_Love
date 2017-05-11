using UnityEngine;
using System.Collections;

public class Marker : MonoBehaviour {

	public bool isFirst_bool = false;
	public float speed = -1f;

	public bool isFirst(){
		return isFirst_bool;
	}

	public float getSpeed(){
		return speed;
	}

	public void setSpeed(float speed){
		this.speed = speed;
	}
}
