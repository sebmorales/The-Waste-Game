﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarder : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	//	transform.forward = Camera.main.transform.forward;
	//	transform.LookAt(Camera.main.transform.position, -Vector3.up);
	}

	void LateUpdate(){
		transform.forward = Camera.main.transform.forward;
	}
}
