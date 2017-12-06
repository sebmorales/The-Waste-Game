using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	public GameObject projectile;
	public float force;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Ray ray = GetComponent<Physics.Raycast
		//Debug.DrawRay(
		if (Input.anyKeyDown) {
			GameObject thisProjectile = Instantiate (projectile);

			thisProjectile.transform.position = transform.position + Camera.main.transform.forward * 1;
			Rigidbody rb = thisProjectile.GetComponent<Rigidbody> ();
			rb.AddForce (Camera.main.transform.forward * force);
		}
	}
}
