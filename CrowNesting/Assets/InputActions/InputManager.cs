using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static InputManager Instance { get; private set; }

	public InputSystem_Actions Actions { get; private set; }

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);

		Actions = new InputSystem_Actions();
	}

	private void OnEnable()
	{
		Actions.Gameplay.Enable();
	}

	private void OnDisable()
	{
		Actions.Disable();
	}

	public void EnableGameplay()
	{
		Actions.Disable();
		Actions.Gameplay.Enable();
	}

	public void EnableUI()
	{
		Actions.Disable();
		Actions.UI.Enable();
	}
}
