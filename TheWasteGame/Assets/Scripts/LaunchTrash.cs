using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTrash : MonoBehaviour {

	public GameObject launched;
	public float maxSecondsToWait;

	private int MAX_OBJECTS;
	// Use this for initialization
	void Start () {
		MAX_OBJECTS = 1000;
		
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (enabled && timeCounter == interval) {
			launch ();
			timeCounter = 0;
			interval = Mathf.FloorToInt (interval * 0.9f);
		}
		
		timeCounter++;*/
	}

	void launch(){
		GameObject obj;
			obj= Instantiate (launched);
			obj.transform.position = transform.position;

			Rigidbody rb = obj.GetComponent<Rigidbody> ();
			Vector2 v = Random.insideUnitCircle * 600.0f;

			/*float a = Random.value * Mathf.PI;
			float r = (Random.value)*200.0f;*/

			rb.AddForce (new Vector3 (v.x, 0.0f, v.y));
			//rb.AddForce (new Vector3 (r*Mathf.Cos(a), 1000.0f, r*Mathf.Sin(a)));

			//rb.AddForce (new Vector3 (1000.0f, 1000.0f,0.0f));
			Debug.Log ("Launching " + v.x + " " + v.y);
			//objects [ptr] = obj;

	}

	public void launchOnce(){
		launch ();
	}

	public void startLaunching(){
		Debug.Log ("Starting launcher");
		StartCoroutine("launcher");
	}

	public void stopLaunching(){
		StopCoroutine ("launcher");
	}

	IEnumerator launcher(){
		float secondsToWait = maxSecondsToWait;
		for (int i = 0; i < MAX_OBJECTS; i++) {
			launch ();
			secondsToWait -= maxSecondsToWait / MAX_OBJECTS;
			yield return new WaitForSeconds (secondsToWait);
		}

	}
}

