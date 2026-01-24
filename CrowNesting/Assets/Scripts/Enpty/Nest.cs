using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Nest : MonoBehaviour
{
	[Header("必要数（ゲームごとに変更）")]
	public int needCotton = 3;
	public int needWire = 2;
	public int needBranch = 4;

	private int currentCotton;
	private int currentWire;
	private int currentBranch;

	[Header("達成度スライダー")]
	[SerializeField] private Slider cottonSlider;
	[SerializeField] private Slider wireSlider;
	[SerializeField] private Slider branchSlider;

	[SerializeField] private PlayerManager playerManager;

	[Header("クリア画面シーン名")]
	public string clearSceneName = "ClearScene";

	void Start()
	{
		// Slider初期設定
		cottonSlider.maxValue = needCotton;
		wireSlider.maxValue = needWire;
		branchSlider.maxValue = needBranch;

		cottonSlider.value = 0;
		wireSlider.value = 0;
		branchSlider.value = 0;
	}

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

		UpdateSliders();
		CheckClear();
	}

	void UpdateSliders()
	{
		cottonSlider.value = currentCotton;
		wireSlider.value = currentWire;
		branchSlider.value = currentBranch;

		// 達成したら色変更
		if (currentCotton >= needCotton)
		{
			cottonSlider.fillRect.GetComponent<Image>().color = Color.yellow;
			CheckClear();
		}

		if (currentWire >= needWire)
		{
			wireSlider.fillRect.GetComponent<Image>().color = Color.yellow;
			CheckClear();
		}

		if (currentBranch >= needBranch)
		{
			branchSlider.fillRect.GetComponent<Image>().color = Color.yellow;
			CheckClear();
		}
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

	/// <summary>
	/// アイテム個数直接セット（ロード・引き継ぎ用）
	/// </summary>
	public void GetItemInfo(int cotton, int wire, int branch)
	{
		currentCotton = cotton;
		currentWire = wire;
		currentBranch = branch;

		UpdateSliders();
		playerManager.ItemReset();
	}

	void GameClear()
	{
		Debug.Log("ゲームクリア！");
		SceneManager.LoadScene(clearSceneName);
	}
}
