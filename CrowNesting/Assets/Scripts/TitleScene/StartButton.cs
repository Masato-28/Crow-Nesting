using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StartButton : MonoBehaviour
{
	[SerializeField] private InputActionReference startAction;

	private void OnEnable()
	{
		if (startAction == null) return;

		startAction.action.Enable();
		startAction.action.performed += OnStartPerformed;
	}

	private void OnDisable()
	{
		if (startAction == null) return;

		startAction.action.performed -= OnStartPerformed;
		startAction.action.Disable();
	}

	private void OnStartPerformed(InputAction.CallbackContext context)
	{
		StartGame();
	}

	private void StartGame()
	{
		SceneManager.LoadScene("GameScene");
	}
}
