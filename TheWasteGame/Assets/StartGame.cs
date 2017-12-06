using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // added
using UnityEngine.UI;

public class StartGame : MonoBehaviour {

	public GameObject loadingMessage;
	public Image FadeImg;

	public AudioClip[] audios;

	private AudioSource src;

	// Use this for initialization
	void Awake () {
		loadingMessage.SetActive (false);
		//FadeImg.rectTransform.localScale = new Vector2 (Screen.width, Screen.height);
	}

	void Start(){
		src = GetComponent<AudioSource> ();
		StartCoroutine (playIntroSounds ());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			StartCoroutine (loadAsyncGame ());
		}
	}

	IEnumerator loadAsyncGame(){
		loadingMessage.SetActive (true);
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync ("Plano");

		while (!asyncLoad.isDone) {
			yield return null;
		}
	}

	IEnumerator playIntroSounds(){
		yield return new WaitForSeconds (5.0f);
		for (;;) {
			for (int i = 0; i < audios.Length; i++) {
				src.PlayOneShot (audios [i]);
				yield return new WaitForSeconds (3.5f);
			}
			yield return new WaitForSeconds (20.0f);
		}
	}
}
