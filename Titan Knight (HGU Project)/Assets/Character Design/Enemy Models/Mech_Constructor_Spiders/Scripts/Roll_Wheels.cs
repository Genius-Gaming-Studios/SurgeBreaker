using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll_Wheels : MonoBehaviour {
	public float wheelDiameter = 1;
	private float wheelLength;
	private Vector3 oldPosition;
	private Vector3 newPosition;
	private float distance;
	
	void  Start (){
		
		oldPosition = new Vector3(0,0,0);
		wheelLength = wheelDiameter*Mathf.PI;
		
	}
	
	void  Update (){
		
		newPosition = this.transform.position;
		distance = Vector3.Distance(oldPosition, newPosition);
		oldPosition = newPosition;

		this.transform.Rotate(distance/wheelLength*360,0,0);
	}
}
