using UnityEngine;
using UnityEngine.InputSystem;

public class CamManager : MonoBehaviour
{
	[Header("Target")]
	[SerializeField] Transform target;
	[SerializeField] Transform cameraHolder;

	[Header("Distance")]
	[SerializeField] float followDistance = 1.5f;

	[Header("Rotation")]
	[SerializeField] float rotateSpeed = 120f;

	[Header("Pitch Limit")]
	[SerializeField] float minPitch = -30f;
	[SerializeField] float maxPitch = 45f;

	[Header("Idle Rotation")]
	[SerializeField] float idlePitch = 10f;
	[SerializeField] float idleReturnSpeed = 5f;
	[SerializeField] float idleDelay = 0.2f;

	[Header("Input")]
	[SerializeField] InputActionReference lookAction;
	[SerializeField] InputActionReference rotateButton; // 右クリック

	private float pitch;
	private float yaw;
	private float idleTimer;

	private void Start()
	{
		pitch = idlePitch;
		yaw = 0f;
	}

	private void OnEnable()
	{
		lookAction.action.Enable();
		rotateButton.action.Enable();
	}

	private void OnDisable()
	{
		lookAction.action.Disable();
		rotateButton.action.Disable();
	}

	private void Update()
	{
		Vector2 look = lookAction.action.ReadValue<Vector2>();

		bool isMouse = Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero;
		bool canRotate = true;

		// マウスの場合：右クリック中のみ回転
		if (isMouse)
		{
			canRotate = rotateButton.action.IsPressed();
		}

		if (canRotate && look.sqrMagnitude > 0.001f)
		{
			yaw += look.x * rotateSpeed * Time.deltaTime;
			pitch -= look.y * rotateSpeed * Time.deltaTime;
			pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

			idleTimer = 0f;
		}
		else
		{
			idleTimer += Time.deltaTime;

			if (idleTimer > idleDelay)
			{
				pitch = Mathf.Lerp(pitch, idlePitch, idleReturnSpeed * Time.deltaTime);
				yaw = Mathf.Lerp(yaw, 0f, idleReturnSpeed * Time.deltaTime);
			}
		}

		cameraHolder.position = target.position;
		cameraHolder.rotation =
			Quaternion.Euler(pitch, target.eulerAngles.y + yaw, 0f);
	}

	private void LateUpdate()
	{
		transform.position =
			cameraHolder.position - cameraHolder.forward * followDistance;

		transform.LookAt(cameraHolder);
	}
}
