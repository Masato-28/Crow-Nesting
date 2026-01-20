using UnityEngine;

public class Enpty_Bird : MonoBehaviour
{
	[Header("Target")]
	[SerializeField] Transform nest;   // 巣（中心）

	[Header("Orbit Settings")]
	[SerializeField] public float radius = 5f;        // 周回半径
	[SerializeField] float orbitSpeed = 60f;   // 回転速度（度/秒）
	[SerializeField] float height = 2f;        // 飛行高度

	void Start()
	{
		// 初期位置を円周上に配置
		Vector3 offset = new Vector3(radius, height, 0);
		transform.position = nest.position + offset;
	}

	void Update()
	{
		// 巣の周りを回転
		transform.RotateAround(
			nest.position,
			Vector3.up,
			orbitSpeed * Time.deltaTime
		);

		// 高さを固定
		Vector3 pos = transform.position;
		pos.y = nest.position.y + height;
		transform.position = pos;

		// 進行方向を向かせる
		Vector3 direction = (transform.position - nest.position).normalized;
		transform.forward = Vector3.Cross(Vector3.up, direction);
	}
}
