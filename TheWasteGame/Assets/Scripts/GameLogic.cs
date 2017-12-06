using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // added

public class GameLogic : MonoBehaviour {

	public GameObject sea;
	public GameObject dust;
	public int pollutionLevel;

	public float targetSeaLevel;

	public int state;

	public Color seaOriginalColor;
	public Color seaWorstColor;

	public AudioSource ambientSource;

	private GameObject d;
	private ParticleSystem.MainModule dustParticleSystem;

	private VoiceoverPlayer voice;
	private LaunchHorizontalTrash launcher;
	private ScreenFader fader;
	private ActionsSoundPlayer actionSound;

	private Renderer seaRenderer;

	// Use this for initialization
	void Start () {
		targetSeaLevel = 0;

		voice = FindObjectOfType<VoiceoverPlayer> ();
		launcher = FindObjectOfType<LaunchHorizontalTrash> ();
		fader = FindObjectOfType<ScreenFader> ();
		actionSound = FindObjectOfType<ActionsSoundPlayer> ();

		// Dust storm as pollution
		d = Instantiate (dust);
		d.SetActive (true);
		d.transform.position = new Vector3 (0, 2.0f, 0);
		dustParticleSystem = d.GetComponent<ParticleSystem> ().main;
		dustParticleSystem.simulationSpeed = 0.5f;
		dustParticleSystem.maxParticles=0; // Set different duration

		seaRenderer = sea.GetComponent<Renderer> (); 

		StartCoroutine ("coroutineUpdatePollution");
		StartCoroutine ("coroutineUpdateSeaLevel");

		state = 1;
		//FindObjectOfType<GvrReticlePointer> ().enabled = false;



		StartCoroutine (introSequence ());

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void incrementSeaLevelWith(float inc){
		targetSeaLevel += inc;
	}

	IEnumerator coroutineUpdatePollution(){
		for (;;) {
			if (pollutionLevel > 3) {
				pollutionLevel -= 3;
			}
			int count = GameObject.FindGameObjectsWithTag ("trash").Length;
			pollutionLevel += count;

			if (pollutionLevel > 1000) {
				Debug.Log ("Lost by pollution");
				StartCoroutine (lostByPollution ());
			}
			else if (pollutionLevel > 800) {
				voice.playPollutionLevel (2);
			} else if (pollutionLevel > 600) {
				voice.playPollutionLevel (1);
			} else if (pollutionLevel > 400) {
				voice.playPollutionLevel (0);
			}

			float t = pollutionLevel / 1000.0f;
			Debug.Log (t);

			seaRenderer.material.SetColor("_horizonColor", Color.Lerp(seaOriginalColor,seaWorstColor,t));
				
		
			dustParticleSystem.maxParticles=pollutionLevel/30;
			yield return new WaitForSeconds(1.0f);
		}
	}

	IEnumerator coroutineUpdateSeaLevel(){
		for (;;) {
			float currentSeaLevel = sea.transform.position.y;
			float error = targetSeaLevel - currentSeaLevel;
			if (error > 0.005f) {
				sea.transform.position = sea.transform.position + new Vector3 (0f, error * 0.01f, 0f);
			//	Debug.Log (sea.transform.position.y);
			}

			if (sea.transform.position.y >= 1.9f) {
				Debug.Log ("Lost by sealevel");
				StartCoroutine (lostByGlobalWarming());
			} else if (sea.transform.position.y > 1.5f) {
				voice.playSeaLevel (2);
			} else if (sea.transform.position.y > 1.0f) {
				voice.playSeaLevel (1);
			} else if (sea.transform.position.y > 0.5f) {
				voice.playSeaLevel (0);
			}
			yield return null;
		}
	}

	IEnumerator loadAsyncGameOver(){
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync ("Standby");

		while (!asyncLoad.isDone) {
			yield return null;
		}
	}

	IEnumerator introSequence(){
		voice.playIntroClipDelayed(0, 1.0f);
		yield return new WaitForSeconds (10.0f);
		launcher.launchOnce ();
		yield break;
	}

	IEnumerator startGameSequence(){
		yield return new WaitForSeconds (20.0f);
		launcher.startLaunching ();
		ambientSource.Play ();
		while (voice.isPlaying ()) {
			yield return new WaitForSeconds (0.2f);
		}
		nextState ();
		yield break;
	}

	IEnumerator lostByPollution(){
		GameOver ();
		voice.playLostByPollution ();
		yield return new WaitForSeconds (10.0f);
		fader.EndGame ();
		StartCoroutine (loadAsyncGameOver ());
	}

	IEnumerator lostByGlobalWarming(){
		GameOver ();
		voice.playLostByGlobalWarming();
		yield return new WaitForSeconds (10.0f);
		fader.EndGame ();
		StartCoroutine (loadAsyncGameOver ());
	}

	void GameOver(){
		StopCoroutine ("coroutineUpdatePollution");
		StopCoroutine ("coroutineUpdateSeaLevel");
		actionSound.setVolume (0.02f);
		fader.GameOver ();
		voice.playGameOver ();
	}

	public int getState(){
		return state;
	}

	public bool readyToChange(){
		//Debug.Log ("Voice playing: " + voice.isPlaying ());
		return !voice.isPlaying ();
	}

	public void nextState(){
		state++;
		Debug.Log ("State " + state);
		switch (state) {
		case 2:
			voice.playIntroClipDelayed(1, 0.5f);
			break;
		case 3:
			voice.playIntroClipDelayed (2, 0.2f);
			//voice.playIntroClipDelayed (3, 6.0f);
			break;
		case 4:
			voice.playIntroClipDelayed (4, 0.2f);
			voice.playIntroClipDelayed (5, 8.0f);
			StartCoroutine (startGameSequence ());
			break;
		case 5:
			actionSound.setVolume (0.2f);
			break;
		}
	}
}
