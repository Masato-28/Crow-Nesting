using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Terrain;

public class Nest : MonoBehaviour
{
	[Header("必要数（ゲームごとに変更）")]
	public int needCotton = 3;
	public int needWire = 2;
	public int needBranch = 4;

	private int currentCotton;
	private int currentWire;
	private int currentBranch;

	public UIManager uiManager;

	[Header("クリア画面シーン名")]
	public string clearSceneName = "ClearScene";

	public void AddMaterial(MaterialType type)
	{
		switch (type)
		{
			case MaterialType.Cotton:
				currentCotton++;
				break;
			case MaterialType.Wire:
				currentWire++;
				break;
			case MaterialType.Branch:
				currentBranch++;
				break;
		}

		uiManager.UpdateUI(
			needCotton - currentCotton,
			needWire - currentWire,
			needBranch - currentBranch
		);

		CheckClear();
	}

	void CheckClear()
	{
		if (currentCotton >= needCotton &&
			currentWire >= needWire &&
			currentBranch >= needBranch)
		{
			GameClear();
		}
	}

	void GameClear()
	{
		Debug.Log("ゲームクリア！");
		SceneManager.LoadScene(clearSceneName);
	}
}

