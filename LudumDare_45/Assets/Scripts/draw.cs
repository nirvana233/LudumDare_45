using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class draw : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject speedLinePrefab;
    LineRenderer currentLineR;
    EdgeCollider2D currentLineCol;
    public PlayerStats stats;
    BallBehaviour ball;
    


    // Update is called once per frame
    void LateUpdate()
    {        
        if(Input.GetButtonDown("Fire1")&&stats.GetInk()>0&&!TestUI()){
            StartLine();
        }
        if(Input.GetButton("Fire1")&&stats.GetInk()>0){
            if(currentLineR!= null){
                DrawLine();
            }
        }
        else if(currentLineR != null){
            StopLine();
        }
        
    }

    public void AddRedInk(float amountInk){
        stats.AddRedInk(amountInk);
        if(currentLineR != null){
            StopLine();
            StartLine();
        }
    }


    private void StartLine(){
        //红墨水
        if(stats.GetRedInk()>0){
            GameObject line = GameObject.Instantiate(speedLinePrefab,Vector3.zero,Quaternion.identity);
            currentLineR = line.GetComponent<LineRenderer>();
            currentLineCol = line.GetComponent<EdgeCollider2D>();
        }
        //正常的墨水
        else{
            GameObject line = GameObject.Instantiate(linePrefab,Vector3.zero,Quaternion.identity);
            currentLineR = line.GetComponent<LineRenderer>();
            currentLineCol = line.GetComponent<EdgeCollider2D>();
        }
        DrawLine();
    }

    private void DrawLine(){
        //z是-10，相机的z，2d项目没用
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentLineR.positionCount++;
            //增加线渲染器的点
            currentLineR.SetPosition(currentLineR.positionCount-1,mousePos);
            Vector2[] points= new Vector2[currentLineR.positionCount];
            //得到线渲染器的Vector2的点给碰撞器用
            for(int i= 0; i<currentLineR.positionCount;i++){
                points[i]=(Vector2)currentLineR.GetPosition(i);
            }
            currentLineCol.points = points;
            if(currentLineR.positionCount>1){
                //计算墨水消耗
                float distance = Vector2.Distance(currentLineR.GetPosition(currentLineR.positionCount-1),currentLineR.GetPosition(currentLineR.positionCount-2));
                stats.AddInk(-distance);
                if(stats.GetRedInk() > 0){
                    stats.AddRedInk(-distance);
                    if(stats.GetRedInk()<=0){
                        StopLine();
                        StartLine();
                        stats.SetRedInk(0);
                    }
                }
            }
    }
    private void StopLine(){
        currentLineR.GetComponent<desolve>().startTimer();
        currentLineR = null;
        currentLineCol = null;
        stats.GetPlayer().GetComponent<BallBehaviour>().startGame();
    }
    bool TestUI(){
        return EventSystem.current.IsPointerOverGameObject();
    }
}
