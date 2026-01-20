using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
	[Header("Target")]
	[SerializeField] Transform target;         // プレイヤー
	[SerializeField] Transform cameraHolder;   // 空のオブジェクト（ターゲットの位置へ追従）

	[Header("Settings")]
	[SerializeField] float followDistance = 1.5f;

	[Header("Idle Rotation")]
	[SerializeField] float idlePitch = 10f;

	private float pitch;

	private void Start()
	{
		pitch = idlePitch;
	}

	private void Update()
	{
		// Pivot
		cameraHolder.position = target.position;
		cameraHolder.rotation = Quaternion.Euler(pitch, target.eulerAngles.y, 0f);
	}

	private void LateUpdate()
	{
		// カメラを cameraHolder の位置・回転に追従
		// 距離は好きに調整
		transform.position = cameraHolder.position + cameraHolder.forward * -followDistance; 
		transform.LookAt(cameraHolder);
	}
}