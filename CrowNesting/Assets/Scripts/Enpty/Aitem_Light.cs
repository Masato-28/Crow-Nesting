using UnityEngine;

public class Aitem_Light : MonoBehaviour
{
	public Transform player;
	public float showDistance = 3f;
	public GameObject glowObject;


	private void Start()
	{
		

	}

	void Update()
	{
		if (player == null || glowObject == null) return;

		float d = Vector3.Distance(player.position, transform.position);
		glowObject.SetActive(d <= showDistance);
	}
}
