using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll_Tank_Tracks : MonoBehaviour {

	public float scrollSpeed = 18;
	private Vector3 oldPosition;
	private Vector3 newPosition;
	private float distance;
	private float offset;
	private Renderer rend;

	// Use this for initialization
	void Start () {
		oldPosition = new Vector3(0,0,0);
		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		newPosition = this.transform.position;
		distance = Vector3.Distance(oldPosition, newPosition);
		oldPosition = newPosition;

		offset = (offset + distance * scrollSpeed * Time.deltaTime)%10;
		rend.material.SetTextureOffset ("_MainTex", new Vector2(0, offset)); 
		rend.material.SetTextureOffset ("_BumpMap", new Vector2(0, offset)); 
	}
}
