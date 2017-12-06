using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeStateOnGaze : MonoBehaviour {

	public Sprite[] messages; // Messages to show as sprites
	public SpriteRenderer message;
	public float interval; // Seconds to wait for each message
	public GameObject smoke; // Effect to show when there's fossil fuel consumption
	public GameObject fire; // Effect to show when burning
	public GameObject explosion; // Effect to show when moving something



	private Ray ray;
	private RaycastHit hit;
	private int time;
	private bool hitting;
	private bool raycastHit;
	private Renderer thisHitRenderer;
	private SpriteRenderer thisHitSpriteRenderer;
	private SpriteRenderer msg;
	private Rigidbody hitRigidBody;
	private int actionId;
	private Color[] actionColors;
	private int[] actionSmokeMaxParticles;

	private GameLogic game;


	private IEnumerator coroutine;
	// Use this for initialization
	void Start () {
		time = 0;
		hitting = false;
		raycastHit = false;
		msg = Instantiate (message);
		msg.enabled = false;

		actionId = 0;
		actionColors = new Color[]{Color.red, Color.yellow, Color.green, Color.blue};
		actionSmokeMaxParticles = new int[]{ 200, 20, 100, 20 };

		game = GameObject.FindObjectOfType<GameLogic> ();


	}
	
	// Update is called once per frame
	void Update () {
		ray = Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0.0f));

		Debug.DrawRay (ray.origin, ray.direction * 10, Color.blue);
		raycastHit = Physics.Raycast (ray, out hit, 50, LayerMask.GetMask("Trash"));

		if (raycastHit) {
			if (game.readyToChange() && game.getState () == 2) {
				game.nextState ();
			}
		}

		if (!hitting) {
			if (raycastHit){ // Started to hit
				Debug.Log ("hit");
				hitting = true;

				hitRigidBody = hit.collider.attachedRigidbody;
				//hitRigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

				msg.enabled = true;
				msg.sprite = messages [0];
			//	msg.transform.position = ray.direction*4.0f + new Vector3(0, 1.0f, 0);
				Vector3 p = hit.collider.transform.position;
				msg.transform.position = new Vector3(p.x, p.y + 1.0f, p.z);
				thisHitRenderer = hit.collider.GetComponentInChildren<Renderer> ();



				StartCoroutine ("rotateSelectedAction");
			}
		}
		else { // if hitting			
			if (raycastHit) { // If still hitting
				// Set message in position
				Vector3 p = hit.collider.transform.position;
				msg.transform.position = new Vector3(p.x, p.y + 1.0f, p.z);

				// Actions!!!
				if ((game.getState()>3 && Input.anyKeyDown) ||  (Input.anyKeyDown && game.getState()==3 && game.readyToChange()) ) {
					if (game.readyToChange() && game.getState () == 3) {
						game.nextState ();
					}
					// Smoke for fossil fuel consumption
					GameObject s = Instantiate (smoke);
					s.transform.position = thisHitRenderer.transform.position;
					ParticleSystem.MainModule ps = s.GetComponent<ParticleSystem> ().main;
					ps.maxParticles=actionSmokeMaxParticles[actionId]; // Set different duration

					// Increment sea level according to fossil fuel consumption
					float incrementSeaLevel = actionSmokeMaxParticles [actionId] / 2000.0f;
					game.incrementSeaLevelWith (incrementSeaLevel);

					Rigidbody rb = hit.collider.attachedRigidbody;
					GameObject f;

					if (game.getState () == 5) {
						GameObject.FindObjectOfType<VoiceoverPlayer> ().playAction (actionId);
					}
					switch (actionId) { // Specific actions
					case 0:	 // Burn it
						f = Instantiate (fire); // create fire and put it in trash position
						f.transform.position = thisHitRenderer.transform.position;

						hit.collider.gameObject.layer = 0; // Remove "Trash" layer so that it won't hit anymore
						stopHit (); // Stop the hitting
						Destroy (hit.collider.gameObject, 1.0f);
						break;
					case 1: // Trash it
						createExplosion (thisHitRenderer.transform.position);
						Vector3 camfw = Camera.main.transform.forward;
						Vector3 fwd = new Vector3 (camfw.x, 0, camfw.z);
						rb.AddForce (fwd * 800f + new Vector3 (0, 500f, 0));
						hit.collider.gameObject.GetComponent<TrashSounds> ().playTrashSound ();
						break;
					case 2: // Recycle it
						createExplosion (thisHitRenderer.transform.position);
						hit.collider.gameObject.GetComponent<TrashSounds> ().playRecycleSound ();
						rb.AddForce (new Vector3 (0, 400f, 0));
						thisHitRenderer.transform.localScale = thisHitRenderer.transform.localScale * 0.3f;
						if (thisHitRenderer.transform.localScale.x < 0.2) {
							Destroy (hit.collider.gameObject, 1.0f);
						}
						break;
					case 3: // Reuse it
					/*	createExplosion (thisHitRenderer.transform.position);
						rb.AddForce (new Vector3 (0, 2000f, 0));*/
						hit.collider.gameObject.GetComponent<TrashSounds> ().playReuseSound ();
						StartCoroutine (makeRigidBodyFly (rb));
						FindObjectOfType<LaunchHorizontalTrash> ().relaunch (hit.collider.gameObject);
						break;
					}
				}
			} else { // if stopped hitting
				stopHit();
			}
		}

	}

	private void createExplosion(Vector3 pos){
		GameObject f = Instantiate (explosion);
		f.transform.position = pos;
		Destroy (f, 1.0f);
	}


	void stopHit(){
		Debug.Log("Stopped hitting");
		msg.enabled = false;
		hitting = false;
		thisHitRenderer.material.color = Color.white;
		StopCoroutine ("rotateSelectedAction");

	}

	IEnumerator rotateSelectedAction(){
		actionId = 0;

		for (;;) {
			GameObject.FindObjectOfType<ActionsSoundPlayer>().playClip(actionId);
			msg.sprite = messages [actionId]; // Set corresponding message
			thisHitRenderer.material.color = actionColors[actionId]; // Set a color

			// Wait
			yield return new WaitForSeconds (interval);

			actionId = (actionId + 1) % 4;
		}
	}

	IEnumerator makeRigidBodyFly(Rigidbody rb){
		for (int i = 0; i < 20; i++) {
			rb.AddForce (new Vector3 (10f, 200f, 0));
			yield return new WaitForSeconds (0.1f);
		}
		rb.AddForce (new Vector3 (4000f, 4000f, 0));
	}

}
