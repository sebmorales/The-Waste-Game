using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceoverPlayer : MonoBehaviour {

	public AudioClip[] introClips;
	public AudioClip[] pollutionLevels;
	public AudioClip[] firstActions;
	public AudioClip[] seaLevels;

	public AudioClip gameOver;
	public AudioClip lostByPollution;
	public AudioClip lostByGlobalWarming;

	private AudioSource src;
	private bool [] playedAction;
	private bool [] playedPollutionLevel;
	private bool [] playedSeaLevel;
	private bool waiting;
	// Use this for initialization
	void Start () {
		src = GetComponent<AudioSource> ();
		playedAction = new bool[firstActions.Length];
		playedPollutionLevel = new bool[pollutionLevels.Length];
		playedSeaLevel = new bool[seaLevels.Length];
		waiting = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void playSeaLevel(int i){
		if (!playedSeaLevel [i]) {
			//src.PlayOneShot (seaLevels [i]);
			StartCoroutine(queuePlay (seaLevels [i]));
			playedSeaLevel [i] = true;
		}
	}

	public void playPollutionLevel(int i){
		if (!playedPollutionLevel [i]) {
			//src.PlayOneShot (pollutionLevels [i]);
			StartCoroutine(queuePlay (pollutionLevels [i]));
			playedPollutionLevel [i] = true;
		}
	}

	public void playAction(int i){
		if(!playedAction[i]){
			StartCoroutine(queuePlay (firstActions [i]));
			//src.clip = firstActions [i];
			//src.PlayDelayed (0.2f);
			playedAction [i] = true;
		}
	}

	public void playGameOver(){
		StartCoroutine(playDelayed (gameOver, 1.0f));
	}

	public void playLostByPollution(){
		StartCoroutine (queuePlay (lostByPollution));
	}

	public void playLostByGlobalWarming(){
		StartCoroutine (queuePlay (lostByGlobalWarming));
	}

	public void playIntroClip(int i){
		src.PlayOneShot (introClips [i]);
	}

	public void playIntroClipDelayed(int i, float delay){
		IEnumerator coroutine = playDelayed (introClips [i], delay);
		StartCoroutine (coroutine);
	}

	IEnumerator queuePlay(AudioClip clip){
		while (isPlaying()) {
			//Debug.Log ("Waiting to play");
			yield return new WaitForSeconds (0.2f);
		}
		src.PlayOneShot (clip);
	}

	IEnumerator playDelayed(AudioClip clip, float delay){
		waiting = true;
		yield return new WaitForSeconds (delay);
		src.PlayOneShot(clip);
		while (src.isPlaying) {
			yield return new WaitForSeconds (0.2f);
		}
		waiting = false;
	}

	public bool isPlaying(){
		return src.isPlaying || waiting;
	}
}
