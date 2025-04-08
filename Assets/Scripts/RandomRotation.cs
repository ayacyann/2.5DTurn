using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            // 计算随机Y旋转
            float randomYRotation = Random.Range(0, 360);
            
            // 应用旋转(只改变Y轴)
            child.localRotation = Quaternion.Euler(0f, randomYRotation, 0f);
        }
    }
}
