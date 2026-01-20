using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmollStone : MonoBehaviour
{

    [SerializeField]private float destroyTime = 2.5f;
    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
        currentTime += Time.deltaTime;
        //Debug.Log(currentTime);
        if (destroyTime >= currentTime)
        {
            Destroy(this);
        }
	}

	private void OnTriggerEnter(Collider other)
	{
        Destroy(this);// âΩÇ©Ç∆ìñÇΩÇ¡ÇΩÇÁè¡Ç¶ÇÈ.
	}
}
