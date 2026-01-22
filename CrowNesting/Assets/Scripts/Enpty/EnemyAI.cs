using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	[Header("巡回ポイント")]
	public Transform[] patrolPoints;   // 巡回地点配列
	private int currentPointIndex = 0; // 今向かっている地点

	public Transform player;      // プレイヤー

	[Header("視界設定")]
	public float viewDistance = 8f;   // 視界距離
	public float viewAngle = 60f;     // 視界角（度）
	public LayerMask obstacleMask;    // 壁などのレイヤー

	private NavMeshAgent agent;
	private bool isChasing = false;

	// 追加
	[SerializeField]private PlayerManager playerManager;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();

		if (patrolPoints.Length > 0)
		{
			agent.SetDestination(patrolPoints[0].position);
		}
		else
		{
			Debug.LogError("巡回ポイントが設定されていません！");
		}
	}

	void Update()
	{

		// 視界内にプレイヤーがいるか？
		if (CanSeePlayer())
		{
			isChasing = true;

			// 追加_ミニゲーム実行.
			playerManager.MiniGameStart();
		}
		else
		{
			isChasing = false;
		}

		if (isChasing)
		{
			agent.SetDestination(player.position);
		}
		else
		{
			Patrol();
		}
	}

	// 視界判定(Raycast)
	bool CanSeePlayer()
	{
		Vector3 dirToPlayer = (player.position - transform.position).normalized;
		float distance = Vector3.Distance(transform.position, player.position);

		// 距離チェック
		if (distance > viewDistance)
			return false;

		// 角度チェック
		float angle = Vector3.Angle(transform.forward, dirToPlayer);
		if (angle > viewAngle / 2f)
			return false;

		// 障害物チェック
		if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dirToPlayer, distance, obstacleMask))
		{
			return false; // 壁に遮られている
		}

		return true;
	}


	// 巡回処理
	void Patrol()
	{
		if (patrolPoints.Length == 0) return;

		if (!agent.pathPending && agent.remainingDistance < 0.5f)
		{
			// 次のポイントへ
			currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
			agent.SetDestination(patrolPoints[currentPointIndex].position);
		}
	}


	// 視界表示表示（シーンのみ）
	void OnDrawGizmos()
	{
		if (player == null) return;

		// 視界距離
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, viewDistance);

		// 視界角
		Vector3 left = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
		Vector3 right = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;

		Gizmos.color = Color.blue;
		Gizmos.DrawRay(transform.position, left * viewDistance);
		Gizmos.DrawRay(transform.position, right * viewDistance);

		// プレイヤーへの視線
		if (CanSeePlayer())
			Gizmos.color = Color.red;
		else
			Gizmos.color = Color.gray;

		Gizmos.DrawLine(transform.position, player.position);

		// 巡回ポイント表示
		if (patrolPoints != null)
		{
			Gizmos.color = Color.green;
			foreach (Transform t in patrolPoints)
			{
				if (t != null)
					Gizmos.DrawSphere(t.position, 0.3f);
			}
		}
	}
}
