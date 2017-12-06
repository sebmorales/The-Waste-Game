using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupTrash : MonoBehaviour {
	public Sprite[] sprites;
	// Use this for initialization
	void Start () {
		int index = Random.Range (0, sprites.Length);
		GetComponent<SpriteRenderer> ().sprite = sprites [index];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
