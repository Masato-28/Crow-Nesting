using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Camera_1 : MonoBehaviour
{
	[Header("Target")]
	[SerializeField] Transform target;         // プレイヤー
	[SerializeField] Transform cameraHolder;   // 空のオブジェクト（ターゲットの位置へ追従）

	[Header("Settings")]
	[SerializeField] float rotateSpeed = 150f;
	[SerializeField] float minPitch = -40f;
	[SerializeField] float maxPitch = 70f;
	[SerializeField] float followDistance = 1.5f;

	[Header("Idle Rotation")]
	[SerializeField] float idleYaw = 0f;
	[SerializeField] float idlePitch = 10f;
	[SerializeField] float returnSpeed = 5f;


	private float yaw;
	private float pitch;

	// Input System
	private InputSystem_Actions.GameplayActions actions;
	private Vector2 lookInput;
	private bool cameraHold;

	/*private void Awake()
	{
		if (InputManager.Instance == null)
		{
			Debug.LogError("[C]InputManager がシーンに存在しません");
			enabled = false;
			return;
		}
		actions = InputManager.Instance.Actions;
	}*/

	private void Awake()
	{
		if (InputManager.Instance == null)
		{
			//Debug.LogError("[C]InputManager がシーンに存在しません");
			enabled = false;
			return;
		}

		actions = InputManager.Instance.Actions.Gameplay;
	}



	private void Update()
	{
		// 入力処理（右クリック押してる間だけ回す）
		cameraHold = actions.CameraHold.IsPressed();

		
		if (cameraHold)
		{
			lookInput = actions.Look.ReadValue<Vector2>();

			yaw += lookInput.x * rotateSpeed * Time.deltaTime;
			pitch -= lookInput.y * rotateSpeed * Time.deltaTime;
			pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
		}
		else
		{
			yaw = Mathf.LerpAngle(yaw, idleYaw, Time.deltaTime * returnSpeed);
			pitch = Mathf.Lerp(pitch, idlePitch, Time.deltaTime * returnSpeed);
		}

		// プレイヤー回転（Yawのみ）
		//target.rotation = Quaternion.Euler(0f, yaw, 0f);

		// Pivot
		cameraHolder.position = target.position;
		cameraHolder.rotation = Quaternion.Euler(pitch, target.eulerAngles.y, 0f);

		/*		if (cameraHold)
				{
					lookInput = controls.Gameplay.Look.ReadValue<Vector2>();

					yaw += lookInput.x * rotateSpeed * Time.deltaTime;
					yaw = Mathf.Repeat(yaw + 180f, 360f) - 180f;
					pitch -= lookInput.y * rotateSpeed * Time.deltaTime;
					pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
				}
				else
				{
					// pitch/yaw を戻す.
					yaw = Mathf.LerpAngle(yaw, idleYaw, Time.deltaTime * returnSpeed);
					pitch = Mathf.Lerp(pitch, idlePitch, Time.deltaTime * returnSpeed);
				}

				// cameraHolder をターゲット位置へ
				cameraHolder.position = target.position;

				// target を中心に回転
				cameraHolder.rotation = Quaternion.Euler(pitch, yaw, 0f);

				// プレイヤーをカメラのYawに合わせる
				target.rotation = Quaternion.Euler(0f, yaw, 0f);*/

	}

	private void LateUpdate()
	{
		// カメラを cameraHolder の位置・回転に追従
		transform.position = cameraHolder.position + cameraHolder.forward * -followDistance; // 距離は好きに調整
		transform.LookAt(cameraHolder);
	}
}