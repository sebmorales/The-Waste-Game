using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsSoundPlayer : MonoBehaviour {

	public AudioClip [] clips;

	private float targetVolume;
	private AudioSource src;

	// Use this for initialization
	void Start () {
		src = GetComponent<AudioSource> ();	
		setVolume (0.02f);

		StartCoroutine (controlVolume ());
	}

	// Update is called once per frame
	void Update () {

	}

	public void setVolume(float v){
		/*targetVolume = v;
		StartCoroutine (reachVolume ());*/
	}

	IEnumerator controlVolume(){
		float dif, error;
		for (;;) {
			if (FindObjectOfType<VoiceoverPlayer> ().isPlaying ()) {
				targetVolume = 0.02f;
			} else {
				targetVolume = 0.2f;
			}
			error = targetVolume - src.volume;
			dif = Mathf.Abs (error);
			src.volume += error*0.1f;
			yield return new WaitForSeconds (0.1f);
		}
	}

	IEnumerator reachVolume(){
		float dif;
		float error;
		do {
			error = targetVolume - src.volume;
			dif = Mathf.Abs (error);
			src.volume += error*0.1f;
			yield return new WaitForSeconds(0.1f);
		} while (dif > 0.01);
	}

	public void playClip(int i){
		//src.clip = clips [i];
		src.PlayOneShot(clips[i]);
	}
}
