using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
	public ItemType itemType;   // このポイントが生成するアイテム種別

	[HideInInspector] public bool active = false;
	[HideInInspector] public bool used = false;

	public void Spawn(GameObject prefab)
	{
		if (!active || used) return;

		Instantiate(prefab, transform.position, Quaternion.identity);
		used = true;
	}
}

public enum ItemType
{
	Branch,
	Stone,
	Fruit,
	Feather,
	Rare,
	
	Hanger,
	Cotton,
	Branches,
	SmollStone,
	harigane,
	wata,
	eda,
	Nest
}
