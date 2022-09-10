using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // 移動階梯以產生向下效果
    void Update()
    {
        transform.Translate(0, moveSpeed*Time.deltaTime,0);

        // 刪掉超出地圖的梯，並呼叫產生階梯
        if (transform.position.y > 5.36f){

            Destroy(gameObject);

            transform.parent.GetComponent<FloorManager>().SpawnFloor();
        }

    }
}
