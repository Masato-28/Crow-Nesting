using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
	public static MiniGameManager Instance;

	[SerializeField] int totalRingCount;   // リング総数
	private int passedRingCount;
	private bool gameStarted;

	private void Awake()
	{
		Instance = this;
	}

	public void StartGame()
	{
		gameStarted = true;
		passedRingCount = 0;
		Debug.Log("ミニゲーム開始！");
	}

	public void PassRing()
	{
		if (!gameStarted) return;

		passedRingCount++;
		Debug.Log($"リング通過 {passedRingCount}/{totalRingCount}");
	}

	public bool CanGoal()
	{
		return gameStarted && passedRingCount >= totalRingCount;
	}

	public void Goal()
	{
		Debug.Log("🎉 ミニゲーム成功！");
		gameStarted = false;
	}
}
