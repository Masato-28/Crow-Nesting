using UnityEngine;

public enum MiniGameResult
{
	Failed,
	Clear,
	Perfect
}

public class MiniGameManager : MonoBehaviour
{
	public static MiniGameManager Instance;

	[Header("Ring Settings")]
	[SerializeField] private int totalRingCount = 5;   // 全リング数
	[SerializeField] private int clearRingCount = 3;   // 最低通過数

	private int passedRingCount;
	private bool isPlaying;

	private void Awake()
	{
		Instance = this;
	}

	public void StartGame()
	{
		isPlaying = true;
		passedRingCount = 0;
	}

	public void PassRing()
	{
		if (!isPlaying) return;
		passedRingCount++;
	}

	public MiniGameResult CheckResult()
	{
		if (passedRingCount >= totalRingCount)
			return MiniGameResult.Perfect;

		if (passedRingCount >= clearRingCount)
			return MiniGameResult.Clear;

		return MiniGameResult.Failed;
	}

	public void Goal()
	{
		if (!isPlaying) return;

		MiniGameResult result = CheckResult();

		switch (result)
		{
			case MiniGameResult.Perfect:
				Debug.Log("🌟 PERFECT!");
				break;

			case MiniGameResult.Clear:
				Debug.Log("✅ CLEAR");
				break;

			case MiniGameResult.Failed:
				Debug.Log("❌ FAILED");
				break;
		}

		isPlaying = false;
	}
}
