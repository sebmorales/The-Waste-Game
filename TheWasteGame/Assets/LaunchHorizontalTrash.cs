using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchHorizontalTrash : MonoBehaviour {

	public GameObject launched;
	public float horizontalRange;
	public int minForce;
	public int maxForce;
	public int minAngle;
	public int maxAngle;

	public float maxSecondsToWait;
	public int maxObjects;
	private int MAX_OBJECTS;

	// Use this for initialization
	void Start () {
		MAX_OBJECTS = maxObjects;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void launch(){
		GameObject obj;
		obj= Instantiate (launched);
		launchGameObject (obj);
	}

	void launchGameObject(GameObject obj){
		obj.transform.position = transform.position + new Vector3(0,0, (Random.value*2-1)*horizontalRange);

		Rigidbody rb = obj.GetComponent<Rigidbody> ();
		//Vector2 v = Random.insideUnitCircle * 600.0f;

		int aDeg = Random.Range (minAngle, maxAngle);
		float a = Mathf.Deg2Rad*aDeg;
		float r = Random.Range(minForce,maxForce);

		rb.AddForce (new Vector3 (-r*Mathf.Cos(a), r*Mathf.Sin(a), 0));
		//rb.AddForce (new Vector3 (r*Mathf.Cos(a), 1000.0f, r*Mathf.Sin(a)));

		//rb.AddForce (new Vector3 (1000.0f, 1000.0f,0.0f));
		Debug.Log ("Launching " + r + " " + aDeg);
		//objects [ptr] = obj;
	}

	public void relaunch(GameObject obj){
		StartCoroutine (delayedLauncher (obj));
	}

	IEnumerator delayedLauncher(GameObject obj){
		yield return new WaitForSeconds (20.0f);
		launchGameObject (obj);
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
