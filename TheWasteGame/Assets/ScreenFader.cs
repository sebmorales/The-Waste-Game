using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// From 
public class ScreenFader : MonoBehaviour {

	public Image FadeImg;
	public float fadeSpeed = 0.1f;
	public float midAlpha = 0.5f;

	void Awake(){
		//FadeImg.rectTransform.localScale = new Vector2 (Screen.width, Screen.height);
	}

	IEnumerator FadeToClear(){
		while (FadeImg.color.a >= 0.05f) {
			FadeImg.color = Color.Lerp (FadeImg.color, Color.clear, fadeSpeed*Time.deltaTime);
			yield return null;
		}
		FadeImg.color = Color.clear;
		FadeImg.enabled = false;
		//FindObjectOfType<GvrReticlePointer> ().enabled = true;
	}

	IEnumerator FadeToMid(){
		FadeImg.enabled = true;
		FindObjectOfType<GvrReticlePointer> ().enabled = false;
		while (FadeImg.color.a < midAlpha) {
			FadeImg.color = Color.Lerp (FadeImg.color, Color.black, fadeSpeed*Time.deltaTime);
			yield return null;
		}

	}

	IEnumerator FadeToBlack(){
		FadeImg.enabled = true;
		while (FadeImg.color.a < 0.95f) {
			FadeImg.color = Color.Lerp (FadeImg.color, Color.black, fadeSpeed*Time.deltaTime);
			yield return null;
		}
		FadeImg.color = Color.black;
	}

	public void StartScene(){
		StartCoroutine (FadeToClear ());
	}

	public void GameOver(){
		StartCoroutine (FadeToMid ());
	}

	public void EndGame(){
		StartCoroutine (FadeToBlack ());
	}

	// Use this for initialization
	void Start () {
		StartScene ();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
