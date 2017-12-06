using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashLife : MonoBehaviour {

	public int life;

	private float newscale;

	// Use this for initialization
	void Start () {
		/*IEnumerator coroutine = updatePollution ();
		StartCoroutine (coroutine);*/
		
	}
	
	// Update is called once per frame
	void Update () {
		// Increase life only when in floor level
		/*if (transform.position.y < 2.0f) {
			life++;
		}*/
		/*if (life % 100 == 0) {
			newscale = 1.0f + life / 1000.0f;
			//transform.localScale = new Vector3(newscale, newscale,newscale);
		}
		float error = newscale - transform.localScale.x;
		if (error > 0.001) {
			transform.localScale = transform.localScale + new Vector3 (error * 0.001f, error * 0.001f, error * 0.001f);
		}*/
	/*
		if (life % 200 == 0) {
			Camera.main.GetComponent<ChangeStateOnGaze> ().pollutionLevel += 1.0f;

		}*/
	}

	/*IEnumerator updatePollution(){
		for (;;) {
			if (transform.position.y < 2.0f) {
				Camera.main.GetComponent<ChangeStateOnGaze> ().pollutionLevel += 1.0f;
			}
			yield return new WaitForSeconds(1.0f);
		}
	}*/
}
