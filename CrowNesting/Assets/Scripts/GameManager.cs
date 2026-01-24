using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[Header("Input Actions")]
	[SerializeField] private InputActionReference escAction;

	[SerializeField] public Light sceneLight;



	private bool isGameClear;

	private void Awake()
	{
		if (sceneLight != null)
		{
			sceneLight.color = Color.white;
		}
	}

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





	public void GameClear()
	{
		isGameClear = true;
		Debug.Log("GM:Clear");
	}

	public void Clear()
	{
		SceneManager.LoadScene("ClearScene");
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
