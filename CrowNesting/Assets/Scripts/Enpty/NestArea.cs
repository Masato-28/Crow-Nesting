using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestArea : MonoBehaviour
{
    [SerializeField] private Enpty_Bird enpty_Bird;

    private float AreaRadius;

    // Start is called before the first frame update
    void Start()
    {
        AreaRadius = enpty_Bird.radius * 2;
        transform.localScale = new Vector3(AreaRadius, AreaRadius, AreaRadius);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")) // プレイヤーが入ったら.
        {
            // ミニゲームを開始する.
            Debug.Log("鳥の巣_ミニゲーム開始？");
        }
	}
}
