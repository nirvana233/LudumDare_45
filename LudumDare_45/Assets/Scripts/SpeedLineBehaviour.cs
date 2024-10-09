using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLineBehaviour : MonoBehaviour
{
    public float speedPower;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionStay2D(Collision2D collisionInfo){
        if(collisionInfo.gameObject.tag == "Player"){
            //Vector2 direction = Rotate(collisionInfo.contacts[0].normal, 90);
            Vector2 normal = collisionInfo.contacts[0].normal;
            Vector2 direction = new Vector2(-normal.y, normal.x);//就是得到切线，可以代替Rotate方法
            //print($"Rotate(collisionInfo.contacts[0].normal, 90);   {Rotate(collisionInfo.contacts[0].normal, 90)}");
            //print($"direction  {direction}");
            collisionInfo.gameObject.GetComponent<BallBehaviour>().AddForce(direction.normalized*speedPower);
        }
    }
    //矩阵运算得到旋转后的向量
    private Vector2 Rotate(Vector2 v, float degrees) {
         float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
         float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

         float tx = v.x;
         float ty = v.y;
         v.x = (cos * tx) - (sin * ty);
         v.y = (sin * tx) + (cos * ty);
         return v;
     }
}
