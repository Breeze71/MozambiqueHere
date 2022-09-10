using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{

    // 建立一個陣列存放兩種地形
    [SerializeField] GameObject[] floorPrefabs;

    public void SpawnFloor(){
        
        // 從陣列中挑選地形
        int r = Random.Range(0,floorPrefabs.Length);

        // 生成階梯，並將其放在 manager 資料夾中
        GameObject floor = Instantiate(floorPrefabs[r],transform);

        // 產生位置
        floor.transform.position = new Vector3(Random.Range(-4f,4f),-5.5f,0f);



    }

}
