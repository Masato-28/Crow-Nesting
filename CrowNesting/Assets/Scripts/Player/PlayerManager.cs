using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.UI.Image;

#region 列挙型変数
public enum MoveType
{
	Physics,     // 通常のモード（Rigidbody使用）.
	Debug,    // Debug用（Transform直操作）.
}

public enum BirdState
{
	Flying,         // 滑空.
	Ascending,      // 上昇.
	Descending,     // 下降.
	Landing,        // 何かに着地中（乗っている状態）.
	Idle,           // 静止(Debug用).
}
#endregion

[System.Serializable]
public class ItemData
{
	public int current;   // 所持数
	public int required;  // 必要数
}


[RequireComponent(typeof(Rigidbody))]
public class PlayerManager : MonoBehaviour
{
	#region 変数宣言

	private PlayerController controller;
	private Rigidbody rb;
	private Animator anim;
	private Vector2 moveInput;  // 移動の入力値.
	private MoveType lastMoveType;

	[SerializeField] private MoveType moveType = MoveType.Physics;			// 挙動管理.
	[SerializeField] private BirdState birdState;							// 行動状態.

	[SerializeField] private float ascendSpeed = 5f;		// 上昇スピード.
	[SerializeField] private float verticalSpeed = 3.0f;	// 下降スピード. 
	[SerializeField] private float rotationSpeed = 5f;		// 回転スピード.
	[SerializeField] private float moveSpeed = 5.0f;        // 移動スピード. 

	private int Life = 3; // 残基.
	[SerializeField] private int correntLife; // 残基.

	//[Header("Player")]
	[SerializeField] Sprite[] LifeSprite;
	[SerializeField] UnityEngine.UI.Image ImageLife;


	[SerializeField] private GameObject SmollStonePrefab;


	[Header("DebugLog")]
	[SerializeField] private bool IsBirdState;
	[SerializeField] private bool IsCheckAnimations;


	private bool ascend;		// 上昇入力.
	private bool descend;       // 下降入力.
	private bool dropStone;		// 石を落とす.

	[Header("Item")]
	[SerializeField] private ItemData branches = new ItemData();
	[SerializeField] private ItemData hangar = new ItemData();
	[SerializeField] private ItemData cotton = new ItemData();
	[SerializeField] private bool isStone;


	#endregion

	#region 初期処理
	private void Awake()
	{
		controller = GetComponent<PlayerController>();
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();

		// Phtsicsの場合Rigidbodyコンポーネントを取得.
		rb = GetComponent<Rigidbody>();

		rb.interpolation = RigidbodyInterpolation.Interpolate;
		rb.collisionDetectionMode = CollisionDetectionMode.Continuous;


		if (moveType == MoveType.Physics)
		{
			rb.useGravity = false;
		}

		// Animatorを取得する.
		anim = GetComponent<Animator>();
	}

	void Start()
	{
		lastMoveType = moveType;
		ApplyMoveType(moveType);

		correntLife = Life;

		// Item リセット.
		// 所持数
		branches.current = 0;
		hangar.current = 0;
		cotton.current = 0;

		isStone = false;	// 石を持っていない状態にする.

		// 必要数
		branches.required = 3;
		hangar.required = 2;
		cotton.required = 5;
	}
	#endregion

	#region Update
	void Update()
	{
		// 入力取得.
		ReadInput();

		DropStone();

		// MoveType の変更を検出する.
		CheckMoveTypeChange();

		// MoveType Debugの場合.
		if (moveType == MoveType.Debug) HandleDebugMove();

		// 状態判定とアニメーション更新.
		UpdateBirdState();
		UpdateAnimation();

		// UIの更新.
		UpdateUI();
	}

	void FixedUpdate()
	{
		// MoveType Debug以外なら.
		if (moveType == MoveType.Physics) HandlePhysicsMove();
	}
	#endregion

	#region 関数

	// 入力取得.
	private void ReadInput()
	{
		moveInput = controller.MoveInput;
		ascend = controller.Ascend;
		descend = controller.Descend;
		dropStone = controller.DropStone;
	}


	// MoveType変更検出.
	private void CheckMoveTypeChange()
	{
		if (moveType != lastMoveType)
		{
			ApplyMoveType(moveType);
			lastMoveType = moveType;
		}
	}

	/// <summary>
	/// Phydics
	/// Rigidbody(慣性)型の挙動処理.
	/// </summary>
	private void HandlePhysicsMove()
	{
		// 着地中は物理的な移動・旋回処理を完全停止
		if (birdState == BirdState.Landing)
		{
			rb.velocity = Vector3.zero;
			return;
		}

		// 水平移動
		Vector3 forwardMove = transform.forward * moveInput.y * moveSpeed;
		Vector3 strafeMove = transform.right * moveInput.x * (moveSpeed * 0.7f);
		Vector3 targetVelocity = forwardMove + strafeMove;

		// 上昇 / 下降
		float vertical = ascend ? ascendSpeed : descend ? -ascendSpeed : -1f;
		targetVelocity.y = vertical;

		// 慣性適用
		rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, 0.05f);

		// Rotation 合成
		ApplyRotationPhysics();
	}

	/// <summary>
	/// Physics用Rotation合成.
	/// </summary>
	/*	private void ApplyRotationPhysics()
		{
			// 着地中は処理しない.
			if (birdState == BirdState.Landing)
			{
				rb.rotation = Quaternion.Slerp(
					rb.rotation,
					Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0),
					0.1f
				);
				return;
			}

			// 入力がない場合は傾きを戻す.
			if (moveInput == Vector2.zero)
			{
				Quaternion flatRot = Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0);
				rb.rotation = Quaternion.Slerp(rb.rotation, flatRot, 0.1f);
				return;
			}

			// 入力方向に向く（yaw を蓄積しない）.
			float targetYaw = transform.eulerAngles.y + moveInput.x * rotationSpeed * Time.deltaTime;
			float roll = -moveInput.x * 25f;

			// 上下の角度.
			float pitch = ascend ? 20f : descend ? -30f : 0f;

			// 回転構成.
			Quaternion targetRot = Quaternion.Euler(pitch, targetYaw, roll);

			// 反映.
			rb.rotation = Quaternion.Slerp(rb.rotation, targetRot, 0.1f);
		}*/

	private void ApplyRotationPhysics()
	{
		// 着地中は処理しない
		if (birdState == BirdState.Landing)
		{
			rb.rotation = Quaternion.Slerp(
				rb.rotation,
				Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0),
				0.1f
			);
			return;
		}

		// 追加：左右入力で Yaw 回転（旋回）
		float yaw = moveInput.x * rotationSpeed * Time.deltaTime;
		rb.rotation = Quaternion.Euler(
			rb.rotation.eulerAngles.x,
			rb.rotation.eulerAngles.y + yaw,
			rb.rotation.eulerAngles.z
		);

		// 上下方向(ピッチ)
		float pitch = ascend ? 20f : descend ? -30f : 0f;
		float roll = -moveInput.x * 25f;  // ロールは今の仕様を利用

		Quaternion targetRot = Quaternion.Euler(pitch, rb.rotation.eulerAngles.y, roll);

		rb.rotation = Quaternion.Slerp(rb.rotation, targetRot, 0.1f);
	}


	/// <summary>
	/// Debug
	/// </summary>
	private void HandleDebugMove()
	{
		// 移動
		Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
		if (ascend) move.y += verticalSpeed;
		if (descend) move.y -= verticalSpeed;
		transform.Translate(move * Time.deltaTime, Space.World);

		// 向き変更
		Vector3 lookDir = new Vector3(moveInput.x, 0, moveInput.y);
		if (lookDir != Vector3.zero) transform.forward = lookDir;
	}

	#region Animation処理
	/// <summary>
	/// アニメーション管理
	/// </summary>
	void UpdateAnimation()
	{
		switch (birdState)
		{
			case BirdState.Ascending:   // 上昇.
				SetAnimState(true, false, false);
				if(IsBirdState)Debug.Log("birdState : Ascending");
				break;

			case BirdState.Descending:  // 下降.
				SetAnimState(false, true, false);
				if (IsBirdState) Debug.Log("birdState : Descending");
				break;

			case BirdState.Landing: // 着地中.
				SetAnimState(false, false, true);
				if (IsBirdState) Debug.Log("birdState : Landing");
				break;

			case BirdState.Flying:    // 滑空.
				SetAnimState(false, false, false);
				if (IsBirdState) Debug.Log("birdState : Flying");
				break;

			case BirdState.Idle:    // Debug用.
				SetAnimState(false, false, false, true);
				if (IsBirdState) Debug.Log("birdState : Debug");
				break;
		}
		if(IsCheckAnimations)	Debug.Log("PlayerAnimations : " + birdState);
	}

	/// <summary>
	/// Animationの状態設定
	/// </summary>
	/// <param name="ascend">上昇</param>
	/// <param name="descend">下降</param>
	/// <param name="landing">着地中</param>
	/// <param name="idle">Debug用</param>
	void SetAnimState(bool ascend, bool descend, bool landing, bool idle = false)
	{
		anim.SetBool("IsAscending", ascend);
		anim.SetBool("IsDescending", descend);
		anim.SetBool("IsLanding", landing);
		anim.SetBool("IsIdle", idle);
	}
	#endregion

	/// <summary>
	/// Movetype変更時に実行する
	/// </summary>
	/// <param name="type"></param>
	void ApplyMoveType(MoveType type)
	{
		if (type == MoveType.Physics)
		{
			rb.useGravity = false;
		}
		else
		{
			rb.useGravity = true;
			rb.velocity = Vector3.zero;
		}
	}

	/// <summary>
	/// 状態判定
	/// </summary>
	/*	private void UpdateBirdState()
		{
			// 地面に居たら処理をしない.
			if (birdState == BirdState.Landing) return;

			// 上昇時.
			if (ascend) birdState = BirdState.Ascending;
			// 下降時.
			else if (descend) birdState = BirdState.Descending;
			// デバッグモード.
			else if (moveInput == Vector2.zero && moveType == MoveType.Debug) birdState = BirdState.Idle;
			// 滑空.
			else birdState = BirdState.Flying;
		}*/
	private void UpdateBirdState()
	{
if (birdState == BirdState.Landing)
{
	if (ascend)
	{
		rb.WakeUp(); // Sleep解除

		birdState = BirdState.Flying;
		rb.velocity = Vector3.up * ascendSpeed;
	}
	return;
}


		// 上昇時
		if (ascend) birdState = BirdState.Ascending;
		// 下降時
		else if (descend) birdState = BirdState.Descending;
		// Debug Idle
		else if (moveInput == Vector2.zero && moveType == MoveType.Debug) birdState = BirdState.Idle;
		// 滑空
		else birdState = BirdState.Flying;
	}


	// 着地中に動かないようにする.
	void HandleLanding()
	{
		rb.velocity = Vector3.zero;      // 完全停止
		rb.angularVelocity = Vector3.zero;     // 回転も止める

		// 入力を無効化（誤操作防止）
		moveInput = Vector2.zero;
		//ascend = false;
		descend = false;
	}

	/// <summary>
	/// UIの更新.
	/// </summary>
	private void UpdateUI()
	{
		// 安全のため、範囲外はクランプ
		int index = Mathf.Clamp(correntLife, 0, LifeSprite.Length - 1);

		// UI画像を差し替え
		ImageLife.sprite = LifeSprite[index];
	}

	private void DropStone()
	{
		if (dropStone == true)  // 入力があったら.
		{
			if (isStone == true)	// 石を持っていたら.
			{	
				// 石を生成する.生成したのと当たって取得判定にさせないためにofesetあり.
				Instantiate(SmollStonePrefab, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), Quaternion.identity);
				Debug.Log("DropStone");
				isStone = false;	// 石を持っていない状態にする.
			}


		}
	}

	void EnterLanding()
	{
		// 状態変更
		birdState = BirdState.Landing;

		// 完全停止
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		// 傾きをリセット（Y回転だけ残す）
		rb.rotation = Quaternion.Euler(
			0f,
			rb.rotation.eulerAngles.y,
			0f
		);

		// 物理のブレ防止
		rb.Sleep();
	}

	/*	
		/// <summary>
		/// 地面法線に合わせる
		/// </summary>
		/// <param name="groundNormal"></param>
		void AlignToGround(Vector3 groundNormal)
		{
			// 現在の前方向を地面に沿わせる
			Vector3 forward = Vector3.ProjectOnPlane(transform.forward, groundNormal);

			if (forward.sqrMagnitude < 0.001f)
				return;

			Quaternion targetRot = Quaternion.LookRotation(forward, groundNormal);

			rb.rotation = targetRot;
		}
	*/
	/// <summary>
	/// 地面法線に合わせる(急斜面除外)
	/// </summary>
	/// <param name="groundNormal"></param>
	void AlignToGround(Vector3 groundNormal)
	{
		float slopeAngle = Vector3.Angle(groundNormal, Vector3.up);

		if (slopeAngle > 45f) return; // 急すぎる斜面は無視

		Vector3 forward = Vector3.ProjectOnPlane(transform.forward, groundNormal);
		Quaternion targetRot = Quaternion.LookRotation(forward, groundNormal);

		rb.rotation = targetRot;
	}



	// 関数終わり
	#endregion

	#region Collision
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Ground"))
		{
			Debug.Log("着地");
			ContactPoint contact = collision.contacts[0];
			AlignToGround(contact.normal);
			EnterLanding();
		}

		// Item.
		if (collision.collider.CompareTag("Branches"))
		{
			branches.current++; // 加算.
			Debug.Log("Get:木枝");
		}
		if (collision.collider.CompareTag("Hangar"))
		{
			hangar.current++;   // 加算.
			Debug.Log("Get:ハンガー");
		}
		if (collision.collider.CompareTag("Cotton"))
		{
			cotton.current++;   //加算.
			Debug.Log("Get:綿");
		}
		if (collision.collider.CompareTag("SmollStone"))
		{
			isStone = true;	// 石を持ってる状態にする.
			Debug.Log("Get:石");
		}

	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.collider.CompareTag("Ground"))
		{
			Debug.Log("離陸");
		}



		if (collision.gameObject.tag == "Item")
		{
			
		}

	}
	#endregion
}
