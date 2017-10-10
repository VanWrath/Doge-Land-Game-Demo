using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1 : PhysicsObject {

	public int maxHealth = 5;
	public GameObject target;
	public float jumpTakeOffSpeed = 7;
	public float maxSpeed = 7;
	public GameObject Weakness;
	public bool isInvulnerable;
	public int health;
	public Slider healthBar;
	public Text healthText;
	public AudioClip attackSound;
	public AudioClip bossMusic;

	private BoxCollider2D attackHitBoxR;
	private BoxCollider2D attackHitBoxL;
	private BoxCollider2D attackHitBox;
	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private bool isAttacking;
	private float distance;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
		animator = GetComponent<Animator> ();
		health = maxHealth;
		isAttacking = false;
		isInvulnerable = false;
		attackHitBoxL = gameObject.GetComponents <BoxCollider2D> () [0];
		attackHitBoxR = gameObject.GetComponents <BoxCollider2D> () [1];

	}

	void Start ()
	{
		healthBar.gameObject.SetActive (true);
		healthBar.maxValue = maxHealth;
		healthBar.value = maxHealth;

		SoundManager.instance.PlayMusic (bossMusic);
	}

	protected override void ComputeVelocity()
	{
		//runs boss movement ai
		if (health > 0) {
			MoveToPlayer ();
		}
	}

	private void MoveToPlayer()
	{
		Vector2 move = Vector2.zero;
		//move +1 if going right. move -1 is going left
		if (target.activeInHierarchy) {
			if (target.transform.position.x < gameObject.transform.position.x - 1.5 && !isAttacking) {
				//face left
				spriteRenderer.flipX = true;
				attackHitBox = attackHitBoxL;
				move.x = -1;
			} else if (target.transform.position.x > gameObject.transform.position.x + 1.5 && !isAttacking) {
				//face right.
				spriteRenderer.flipX = false;
				attackHitBox = attackHitBoxR;
				move.x = 1;
			}
		}

		animator.SetBool ("grounded", grounded);
		animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);
		targetVelocity = move * maxSpeed;
	}
	

	IEnumerator AttackTarget()
	{
		isAttacking = true;
		animator.SetTrigger ("attack");
		SoundManager.instance.PlaySingle (attackSound);
		attackHitBox.enabled = true;
		yield return new WaitForSeconds (0.1f);
		attackHitBox.enabled = false;
		isAttacking = false;
	}
	private IEnumerator MakeInvulerable()
	{
		isInvulnerable = true;
		yield return new WaitForSeconds (2f);
		isInvulnerable = false;
	}

	public void TakeDmg()
	{
		health--;
		StartCoroutine (MakeInvulerable ());
		if (health <= 0)
		{
			animator.SetBool("IsDead",true);
			Destroy (gameObject, 2f);
		}
	}

	protected override void Update()
	{
		base.Update ();
		healthBar.value = health;
		healthText.text = health + "/" + maxHealth;
		//calculates distance from player and attack when distance is small enough.
		distance = Vector3.Distance (target.transform.position, gameObject.transform.position);
		if (distance < 1.5f) {
			StartCoroutine (AttackTarget ());
		}
		if (health <= 0)
		{
			animator.SetBool("IsDead",true);
			Destroy (gameObject, 2f);
			LevelManager.instance.SetBossDead ();
		}
	}
}
