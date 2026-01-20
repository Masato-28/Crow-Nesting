using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	[Header("Input Actions")]
	[SerializeField] private InputActionReference escAction;

	private void OnEnable()
	{
		escAction.action.Enable();
		escAction.action.performed += OnEsc;
	}

	private void OnDisable()
	{
		escAction.action.performed -= OnEsc;
		escAction.action.Disable();
	}

	// EscÇ™âüÇ≥ÇÍÇΩéû
	private void OnEsc(InputAction.CallbackContext context)
	{
		EndGame();
	}

	// ÉQÅ[ÉÄèIóπ
	private void EndGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
	}
}
