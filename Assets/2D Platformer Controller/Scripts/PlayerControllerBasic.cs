using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerBasic : MonoBehaviour {

	public float jumpTakeOffSpeed = 7;
	public float maxSpeed = 7;
	public AudioClip moveSound;
	public AudioClip jumpSound;
	public AudioClip hitSound;
	public AudioClip gameOverSound;
	public AudioClip coinSound;
	public AudioClip heartSound;
	public AudioClip winSound;
	public Text healthText;
	public Text scoreText;
	public GameObject startPoint;

	private int health;
	private int score;
	private int coinScore;
	private float moveX;
	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private bool grounded;
	private Vector2 velocity;
	private bool isInvulnerable;
	private WaitForSeconds invulnerableTime = new WaitForSeconds (2.0f);
	//private Vector2 respawnPoint;
	//private bool isLevelFinished;

	// Use this for initialization
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		health = 3;
		coinScore = 100;
		isInvulnerable = false;
		healthText.text = "Health: " + health;
		//respawnPoint = startPoint.transform.position;
		//isLevelFinished = false;
	}
	
	// Update is called once per frame
	void Update () {
		PlayerMove ();
		if (health <= 0) {
			animator.SetBool ("IsDead", true);
			SoundManager.instance.musicSource.Stop ();
			//play game over music.
			SoundManager.instance.PlaySingle (gameOverSound);
			Destroy(gameObject, 3f);
			//GameManager.instance.isGameFinished = true;
			ShowPanels.instance.ShowGameOverPanel ();
			(ShowPanels.instance.gameOverPanel.GetComponentsInChildren <Text>())[1].text = "Score: " + score;
		}
	}

	void PlayerMove(){
		if (health > 0) {
			//horizontal movement
			moveX = Input.GetAxis ("Horizontal");

			//jump
			if (Input.GetButtonDown ("Jump") && grounded) { //if player pressed down the jump button, the vertical velocity is the jump speed.
				GetComponent<Rigidbody2D>().AddForce (Vector2.up * jumpTakeOffSpeed);
				grounded = false;
				//play jump sound
				SoundManager.instance.PlayerFX (jumpSound);
			}

			/*else if (Input.GetButtonUp ("Jump")) { //if player releases the jump button, velocity gets reduced.
				if (GetComponent ()> 0) {
					velocity.y = velocity.y * 0.5f;
				}
			}*/

			bool flipSprite = (spriteRenderer.flipX ? (moveX > 0.01f) : (moveX < -0.01f)); //flips sprite so it faces the right direction.
			if (flipSprite) {
				spriteRenderer.flipX = !spriteRenderer.flipX;
			}

			gameObject.GetComponent <Rigidbody2D>().velocity = new Vector2(moveX * maxSpeed, gameObject.GetComponent <Rigidbody2D>().velocity.y);
			velocity = gameObject.GetComponent <Rigidbody2D> ().velocity;
			animator.SetBool ("grounded", grounded);
			animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);
			//targetVelocity = move * maxSpeed;
		}
	}

	//coroutine for invulnerability when damaged
	IEnumerator MakeInvulnerable(WaitForSeconds t)
	{
		isInvulnerable = true;
		StartCoroutine(InvulnerableAnimation());
		yield return t;
		isInvulnerable = false;
	}

	//coroutine for invulnerability animation.
	IEnumerator InvulnerableAnimation()
	{
		bool flickerOn = true;
		while (isInvulnerable == true) {
			flickerOn = !flickerOn;
			spriteRenderer.enabled = flickerOn;
			yield return null;
		}
		spriteRenderer.enabled = true;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Boss") {
			//Debug.Log ("hitting boss");
			velocity.y = jumpTakeOffSpeed;
		}
		if (other.gameObject.tag == "Ground") {
			grounded = true;
		}
	}

	//actions for triggers
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Trap" || other.tag == "Boss") {
			//play hurt animation.
			if (isInvulnerable == false) {
				animator.SetTrigger ("Hurt");
				//StartCoroutine (HurtAnimation ());
				health--;
				healthText.text = "Health: " + health;
				StartCoroutine (MakeInvulnerable (invulnerableTime));
				//CheckIfGameOver ();
			}
		} else if (other.tag == "Coin") {
			//play coin sound
			SoundManager.instance.PlaySingle (coinSound);
			score += coinScore;
			scoreText.text = "Score: " + score;
			other.gameObject.SetActive (false);
		} else if (other.tag == "Heart") {
			SoundManager.instance.PlaySingle (heartSound);
			health++;
			healthText.text = "Health: " + health;
			other.gameObject.SetActive (false);
		} 
		/*else if (other.tag == "Respawn") {
			respawnPoint = other.transform.position;
		} */
		else if (other.tag == "Boss Trig") {
			LevelManager.instance.SpawnBoss ();
		} 
		else if (other.tag == "Weak Spot") {
			LevelManager.instance.DmgBoss ();
		}
		else if (other.tag == "Finish") {
			if (LevelManager.instance.isBossDead == true) {
				SoundManager.instance.musicSource.Stop ();
				SoundManager.instance.PlaySingle (winSound);
				GameManager.instance.FinishLevel (score);
			}
		}
	}
}
