using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private enum States
	{
		IDLE,
		WALKING
	}

	[SerializeField] private float playerSpeed;
	[SerializeField] private UnityEvent OnDiaryToggle;

	private Rigidbody2D playerRigidBody;
	private Animator playerAnimator;
	private Vector2 rawInputMovement = Vector2.zero;
	private States currentState = States.IDLE;
	private string currentAnimation = "Idle";

	private void Awake()
	{
		playerRigidBody = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();
	}

	private void Update()
	{
		switch (currentState)
		{
			case States.IDLE:
				PlayAnimation(States.IDLE);
				break;
			case States.WALKING:
				PlayAnimation(States.WALKING);
				break;
		}
	}

	private void FixedUpdate()
	{
		switch (currentState)
		{
			case States.IDLE:
				Stop();
				break;
			case States.WALKING:
				Move();
				break;
		}  
	}

	private void Stop()
	{
		playerRigidBody.velocity = Vector2.zero;
	}

	private void Move()
	{
		playerRigidBody.velocity = rawInputMovement * playerSpeed;
	}

	private void PlayAnimation(States animationToPlay)
	{
		if (animationToPlay == States.IDLE)
		{
			playerAnimator.speed = 0;
			playerAnimator.Play(currentAnimation, 0, 0);
		}
		else if (animationToPlay == States.WALKING)
		{
			playerAnimator.speed = 1;

			if (rawInputMovement.y > 0)
			{
				playerAnimator.Play("WalkingNorth", 0);
				currentAnimation = "WalkingNorth";
			}
			else if (rawInputMovement.y < 0)
			{
				playerAnimator.Play("WalkingSouth", 0);
				currentAnimation = "WalkingSouth";
			}
			else if (rawInputMovement.x > 0)
			{
				playerAnimator.Play("WalkingWest", 0);
				currentAnimation = "WalkingWest";
			}
			else if (rawInputMovement.x < 0)
			{
				playerAnimator.Play("WalkingEast", 0);
				currentAnimation = "WalkingEast";
			}
		}
	}

	public void OnActionMovement(InputAction.CallbackContext context)
	{
		rawInputMovement = context.ReadValue<Vector2>();

		if (rawInputMovement == Vector2.zero)
		{
			currentState = States.IDLE;
		}
		else
		{
			currentState = States.WALKING;
		}
	}

	public void OnActionInteract(InputAction.CallbackContext context) 
	{
		if(context.performed) {
			if(DialogueManager.Instance.hasStarted == false) {
				DialogueManager.Instance.StartDialogue();
			} else if (DialogueManager.Instance.hasStarted) {
				DialogueManager.Instance.NextLine();
			}
		}
	}
	
	public void OnActionDiary(InputAction.CallbackContext context) 
	{
		if(context.performed) {
			OnDiaryToggle.Invoke();
		}
	}
}
