using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    //camera 添加 scripts，声明 transform 属性， 绑定 player
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //设置摄像头，跟踪狐狸位置
        transform.position = new Vector3(player.position.x, 0, -10f);
    }
}
