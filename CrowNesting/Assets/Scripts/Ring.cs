using UnityEngine;

public class Ring : MonoBehaviour
{
	private bool passed;

	private void OnTriggerEnter(Collider other)
	{
		if (passed) return;

		if (other.CompareTag("Player"))
		{
			passed = true;
			MiniGameManager.Instance.PassRing();
			gameObject.SetActive(false);
		}
	}
}
