using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    SpriteRenderer sR;
    // Start is called before the first frame update
    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
    }
    //void Update()
    //{
    //    Vector2 parentPos = transform.position-transform.localPosition;
    //    transform.localPosition= new Vector3(-parentPos.x%transform.localScale.x,-parentPos.y%transform.localScale.y,10);
    //}
    /*
    相机移动多少，子物体（背景）就应该反向移动LocalPos（%之后的）多少，以保持位置不变，球在向前滚动。但一直滚动下去，背景肯定不够用。
    并且，背景是平铺的。所以，这时只要滚动到平铺的距离时，直接把背景拉回去（也就是取模），游戏画面看着是一样的。
     */
    private void LateUpdate()
    {
        Vector2 parentPos = transform.position - transform.localPosition;
        transform.localPosition = new Vector3(-parentPos.x % transform.localScale.x, -parentPos.y % transform.localScale.y, 10);
    }
}
