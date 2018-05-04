using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : PhysicsObject {

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
    private SpriteRenderer spriteRenderer;
    private Animator animator;
	private bool isInvulnerable;
	private WaitForSeconds invulnerableTime = new WaitForSeconds (2.0f);
	//private Vector2 respawnPoint;
	private bool checkOnce = false;
	private bool canMove;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
		health = 3;
		coinScore = 100;
		isInvulnerable = false;
		healthText.text = "Health: " + health;
		//respawnPoint = startPoint.transform.position;
		canMove = true;

    }

    protected override void ComputeVelocity()
    {	
		Vector2 move = Vector2.zero;
		if (health > 0 && canMove) {
			move.x = Input.GetAxis ("Horizontal");

			//jump
			if (Input.GetButtonDown ("Jump") && grounded) { //if player pressed down the jump button, the vertical velocity is the jump speed.
				velocity.y = jumpTakeOffSpeed;
				//play jump sound
				SoundManager.instance.PlayerFX (jumpSound);
			} else if (Input.GetButtonUp ("Jump")) { //if player releases the jump button, velocity gets reduced.
				if (velocity.y > 0) {
					velocity.y = velocity.y * 0.5f;
				}
			}

			//run
			/*if (Input.GetButton ("Fire1")) 
			{
				move.x = move.x * 1.5f;
			}*/
		}

		bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f)); //flips sprite so it faces the right direction.
		if (flipSprite) {
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}

		animator.SetBool ("grounded", grounded);
		animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);
		targetVelocity = move * maxSpeed;
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

	/*IEnumerator HurtAnimation()
	{
		animator.SetBool ("hurt", true);
		yield return new WaitForSeconds (0.25f);
		animator.SetBool ("hurt", false);
	}*/

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Boss") {
			velocity.y = jumpTakeOffSpeed;
		}
	}
		
	//actions for triggers
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Trap" || other.tag == "Boss") {
			//play hurt animation.
			if (!isInvulnerable) {
				animator.SetTrigger ("Hurt");
				//StartCoroutine (HurtAnimation ());
				health--;
				StartCoroutine (MakeInvulnerable (invulnerableTime));
				//CheckIfGameOver ();
				score = score - coinScore;
				UpdateText ();
			}
		} else if (other.tag == "Coin") {
			//play coin sound
			SoundManager.instance.PlaySingle (coinSound);
			score += coinScore;
			UpdateText ();
			other.gameObject.SetActive (false);
		} else if (other.tag == "Heart") {
			SoundManager.instance.PlaySingle (heartSound);
			health++;
			UpdateText ();
			other.gameObject.SetActive (false);
		}
		else if (other.tag == "Boss Trig") {
			LevelManager.instance.SpawnBoss ();
		} 
		else if (other.tag == "Weak Spot") {
			velocity.y = jumpTakeOffSpeed;
			LevelManager.instance.DmgBoss ();
		}
		else if (other.tag == "Finish") {
			if (LevelManager.instance.isBossDead == true) {
				SoundManager.instance.musicSource.Stop ();
				SoundManager.instance.PlaySingle (winSound);
				GameManager.instance.FinishLevel (score);
				canMove = false;
			}
		}
	}

	private void UpdateText()
	{
		healthText.text = "Health: " + health;
		scoreText.text = "Score: " + score;
	}
		
	protected override void Update()
	{
		base.Update ();
		//cehck game over
		if (health <= 0 && !checkOnce) {
			animator.SetBool ("IsDead", true);
			SoundManager.instance.musicSource.Stop ();
			//play game over music.
			SoundManager.instance.PlaySingle (gameOverSound);
			Destroy(gameObject, 3f);
			//GameManager.instance.isGameFinished = true;
			ShowPanels.instance.ShowGameOverPanel ();
			(ShowPanels.instance.gameOverPanel.GetComponentsInChildren <Text>())[1].text = "Score: " + score;
			checkOnce = true;
			canMove = false;
		}
	}
}
