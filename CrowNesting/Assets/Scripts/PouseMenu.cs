using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] private GameObject pauseMenuUI;  // ポーズメニューのUIパネル
	[SerializeField] private Button resumeButton;     // 再開ボタン
	[SerializeField] private Button quitButton;       // 終了ボタン

	private bool isPaused = false;

	private void Start()
	{
		// パネルを初期状態で非表示に
		pauseMenuUI.SetActive(false);

		// ボタンにリスナーを追加
		resumeButton.onClick.AddListener(ResumeGame);
		quitButton.onClick.AddListener(QuitGame);
	}

	private void Update()
	{
		// ESCキーでポーズ切り替え
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePause();
		}
	}

	public void TogglePause()
	{
		if (isPaused)
		{
			ResumeGame();
		}
		else
		{
			PauseGame();
		}
	}

	private void PauseGame()
	{
		// ゲーム時間を停止
		Time.timeScale = 0f;

		// UIを表示
		pauseMenuUI.SetActive(true);

		// ポーズ状態を更新
		isPaused = true;
	}

	public void ResumeGame()
	{
		// ゲーム時間を通常に戻す
		Time.timeScale = 1f;

		// UIを非表示
		pauseMenuUI.SetActive(false);

		// ポーズ状態を更新
		isPaused = false;
	}

	public void QuitGame()
	{
		// ゲーム時間を元に戻す
		Time.timeScale = 1f;

		// アプリケーション終了（エディタでは動作しない）
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
	}

	// シーンをロードする機能も追加可能
	public void LoadMainMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("GameScene"); // メインメニューシーンの名前を指定
	}
}