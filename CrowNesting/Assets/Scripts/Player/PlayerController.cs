using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Vector2 MoveInput { get; private set; }
	public bool Ascend { get; private set; }
	public bool Descend { get; private set; }
	public bool DropStone { get; private set; }

	private InputSystem_Actions.GameplayActions gameplay;

	private void Awake()
	{
		if (InputManager.Instance == null)
		{
			Debug.LogError("InputManager Ç™ÉVÅ[ÉìÇ…ë∂ç›ÇµÇ‹ÇπÇÒ");
			enabled = false;
			return;
		}

		gameplay = InputManager.Instance.Actions.Gameplay;
	}

	private void Update()
	{
		MoveInput = gameplay.Move.ReadValue<Vector2>();
		Ascend = gameplay.Ascend.IsPressed();
		Descend = gameplay.Descend.IsPressed();
		DropStone = gameplay.DropStone.IsPressed();

	}
}
