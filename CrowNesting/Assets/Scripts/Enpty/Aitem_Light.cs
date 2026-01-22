using UnityEngine;

public class Aitem_Light : MonoBehaviour
{



	private void Start()
	{
		

	}

	void Update()
	{
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			GameObject.Destroy(this);
		}
	}
}
