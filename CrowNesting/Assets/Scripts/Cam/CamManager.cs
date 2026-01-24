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
	//[SerializeField] float idleReturnSpeed = 5f;
	//[SerializeField] float idleDelay = 0.2f;

	[Header("Input")]
	[SerializeField] InputActionReference lookAction;
	[SerializeField] InputActionReference rotateButton; // 右クリック
	[SerializeField] InputActionReference resetCameraAction;

	[Header("Reset Motion")]
	[SerializeField] float resetDuration = 0.15f;

	bool isResetting = false;
	float resetTimer = 0f;
	float startPitch;
	float startYaw;


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
		resetCameraAction.action.Enable();
	}

	private void OnDisable()
	{
		lookAction.action.Disable();
		rotateButton.action.Disable();
		resetCameraAction.action.Disable();
	}


	private void Update()
	{
		Vector2 look = lookAction.action.ReadValue<Vector2>();
		bool isMouse = Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero;
		bool canRotate = true;

		if (isMouse)
		{
			canRotate = rotateButton.action.IsPressed();
		}

		if (canRotate && look.sqrMagnitude > 0.001f)
		{
			yaw += look.x * rotateSpeed * Time.deltaTime;
			pitch -= look.y * rotateSpeed * Time.deltaTime;
			pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
		}

		if (resetCameraAction.action.WasPressedThisFrame())
		{
			isResetting = true;
			resetTimer = 0f;

			startPitch = pitch;
			startYaw = yaw;
		}

		if (isResetting)
{
	resetTimer += Time.deltaTime;
	float t = resetTimer / resetDuration;

	// 0→1 をなめらかに
	t = Mathf.SmoothStep(0f, 1f, t);

	pitch = Mathf.Lerp(startPitch, idlePitch, t);
	yaw = Mathf.Lerp(startYaw, 0f, t);

	if (t >= 1f)
	{
		pitch = idlePitch;
		yaw = 0f;
		isResetting = false;
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
