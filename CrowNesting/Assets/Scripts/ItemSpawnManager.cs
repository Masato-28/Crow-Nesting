using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemSpawnManager : MonoBehaviour
{
	[System.Serializable]
	public class ItemSpawnSetting
	{
		public ItemType itemType;          // 枝 / ハンガー / 針金 / 石 / 骨
		public GameObject itemPrefab;      // 対応するPrefab
		public int minActivePoints = 10;   // 最小有効ポイント数
		public int maxActivePoints = 20;   // 最大有効ポイント数
	}

	[Header("Item Spawn Settings (5 Types)")]
	public ItemSpawnSetting[] itemSettings;

	void Start()
	{
		// シーン内の全生成ポイントを取得
		ItemSpawnPoint[] allPoints = FindObjectsOfType<ItemSpawnPoint>();

		foreach (var setting in itemSettings)
		{
			// 該当アイテム用ポイントのみ抽出
			List<ItemSpawnPoint> points = allPoints
				.Where(p => p.itemType == setting.itemType)
				.ToList();

			if (points.Count == 0) continue;

			// 初期化
			foreach (var p in points)
			{
				p.active = false;
				p.used = false;
			}

			// 有効化するポイント数を決定
			int activeCount = Random.Range(
				setting.minActivePoints,
				setting.maxActivePoints + 1
			);

			// シャッフル（Fisher–Yates）
			for (int i = 0; i < points.Count; i++)
			{
				int r = Random.Range(i, points.Count);
				(points[i], points[r]) = (points[r], points[i]);
			}

			// 有効化 & 生成（1回のみ）
			for (int i = 0; i < activeCount && i < points.Count; i++)
			{
				points[i].active = true;
				points[i].Spawn(setting.itemPrefab);
			}
		}
	}
}
