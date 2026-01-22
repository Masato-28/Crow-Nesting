using UnityEngine;
using UnityEngine.InputSystem;

/*public enum ItemType
{
	Branch,
	Stone,
	Fruit,
	Feather,
	Rare
}*/

public class GameManager : MonoBehaviour
{
	[Header("Input Actions")]
	[SerializeField] private InputActionReference escAction;

	//[SerializeField] public ItemType itemType;

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
