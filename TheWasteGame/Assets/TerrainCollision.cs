using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCollision : MonoBehaviour {

	private bool listen;
	// Use this for initialization
	void Start () {
		listen = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision){
		if (listen) {
			Debug.Log ("First collision");
			GameLogic gl = FindObjectOfType<GameLogic> ();
			if (gl.getState () == 1) {
				gl.nextState ();
				listen = false;
			}
		}
	}
}
