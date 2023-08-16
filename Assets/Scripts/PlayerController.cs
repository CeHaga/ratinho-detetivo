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

	private enum Directions
	{
		UP,
		DOWN,
		LEFT,
		RIGHT
	}

	[SerializeField] private float playerSpeed;
	[SerializeField] private string[] interactableTags;

	private DiaryManager diaryManager;
	private DialogueManager dialogueManager;
	[SerializeField] private PlayerInput playerInput;

	[Header("Animations")]
	[SerializeField] private AnimationClip idleFrontAnimation;
	[SerializeField] private AnimationClip idleBackAnimation;
	[SerializeField] private AnimationClip idleSideAnimation;
	[SerializeField] private AnimationClip walkingFrontAnimation;
	[SerializeField] private AnimationClip walkingBackAnimation;
	[SerializeField] private AnimationClip walkingSideAnimation;

	private Rigidbody2D playerRigidBody;
	private Animator playerAnimator;
	private Vector2 rawInputMovement = Vector2.zero;
	private States currentState = States.IDLE;
	private GameObject interactable;
	private bool isDialogueActive;
	private Directions lastDirection;

	private void Awake()
	{
		playerRigidBody = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();
		interactable = null;
		lastDirection = Directions.DOWN;
	}

	private void Start()
	{
		diaryManager = DiaryManager.Instance;
		dialogueManager = DialogueManager.Instance;
		dialogueManager.onDialogueFinish += OnDialogueFinish;
		dialogueManager.onDialogueStart += OnDialogueStart;
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
			switch (lastDirection)
			{
				case Directions.UP:
					playerAnimator.Play(idleBackAnimation.name);
					MirrorAnimation(false);
					break;
				case Directions.DOWN:
					playerAnimator.Play(idleFrontAnimation.name);
					MirrorAnimation(false);
					break;
				case Directions.LEFT:
					playerAnimator.Play(idleSideAnimation.name);
					MirrorAnimation(true);
					break;
				case Directions.RIGHT:
					playerAnimator.Play(idleSideAnimation.name);
					MirrorAnimation(false);
					break;
			}
		}
		else if (animationToPlay == States.WALKING)
		{
			playerAnimator.speed = 1;

			if (rawInputMovement.y > 0)
			{
				playerAnimator.Play(walkingBackAnimation.name);
				MirrorAnimation(false);
				lastDirection = Directions.UP;
			}
			else if (rawInputMovement.y < 0)
			{
				playerAnimator.Play(walkingFrontAnimation.name);
				MirrorAnimation(false);
				lastDirection = Directions.DOWN;
			}
			else if (rawInputMovement.x > 0)
			{
				playerAnimator.Play(walkingSideAnimation.name);
				MirrorAnimation(false);
				lastDirection = Directions.RIGHT;
			}
			else if (rawInputMovement.x < 0)
			{
				playerAnimator.Play(walkingSideAnimation.name);
				MirrorAnimation(true);
				lastDirection = Directions.LEFT;
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

	public void OnInteractableInteract(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		if (interactable == null) return;

		Interactable interactableComponent = interactable.GetComponent<Interactable>();
		if (interactableComponent != null)
		{
			interactableComponent.Interact();
		}
	}

	public void OnDialogueInteract(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		dialogueManager.Continue();
	}

	public void OnDialogueChoose(InputAction.CallbackContext context)
	{
		if (!context.performed) return;

		float rawInput = context.ReadValue<float>();
		if (rawInput > 0)
		{
			dialogueManager.MoveOption(MoveDialogueOptions.UP);
		}
		else if (rawInput < 0)
		{
			dialogueManager.MoveOption(MoveDialogueOptions.DOWN);
		}
	}

	public void OnDialogueStart()
	{
		playerInput.SwitchCurrentActionMap("Dialogue");
	}

	public void OnDialogueFinish()
	{
		playerInput.SwitchCurrentActionMap("Walking");
	}

	public void OnActionDiary(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			diaryManager.ToggleDiary();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		foreach (string tag in interactableTags)
		{
			if (other.CompareTag(tag))
			{
				interactable = other.gameObject;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		interactable = null;
	}

	private void MirrorAnimation(bool mirror)
	{
		int mult = mirror ? -1 : 1;
		transform.localScale = new Vector3(mult, 1, 1);
	}
}
