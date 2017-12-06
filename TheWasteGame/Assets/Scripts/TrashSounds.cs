using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSounds : MonoBehaviour {

	public AudioClip earthSound;
	public AudioClip waterSound;

	public AudioClip trashSound;
	public AudioClip recycleSound;
	public AudioClip reuseSound;

	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void playTrashSound(){
		source.PlayOneShot (trashSound);
	}

	public void playRecycleSound(){
		source.PlayOneShot (recycleSound);
	}

	public void playReuseSound(){
		source.PlayOneShot (reuseSound);
	}

	void OnCollisionEnter(Collision collision){
		if (collision.collider.tag.Equals ("terrain")) {
			source.clip = earthSound;
		}
		if (collision.collider.tag.Equals ("water")) {
			source.clip = waterSound;
		}
		source.Play ();
	}
}
