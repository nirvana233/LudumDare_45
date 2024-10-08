using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class desolve : MonoBehaviour
{
    public float timeSpan=0;
    LineRenderer l;
    EdgeCollider2D ed;
    public float desolveTime = 1;
    public Color col;


    public void startTimer(){
        l= GetComponent<LineRenderer>();
        ed = GetComponent<EdgeCollider2D>();
        StartCoroutine(timer());

    }
    
    void Desolve(){
        float lineLength = 0;
        for(int i = 0; i<l.positionCount-1;i++){
            lineLength+=Vector2.Distance(l.GetPosition(i),l.GetPosition(i+1));
        }
        //float desolveTime = lineLength/desolveSpeed;
        
        StartCoroutine(DesolveAnim(desolveTime, lineLength));
    }
    
    IEnumerator DesolveAnim(float desolveTime, float lineLength){
        float t= 0;
        bool calculated= false;
        float nextLength= 0;
        float deletedPath = 0;
        while(t<desolveTime){
            t+= Time.deltaTime;
            // l.colorGradient.alphaKeys[0].time = t/desolveTime+0.1f;
            // l.colorGradient.alphaKeys[1].time = t/desolveTime;
            
            // l.colorGradient.alphaKeys[1].alpha = Mathf.Clamp01(t/(desolveTime/10))*255;
            //这部分代码逐步减少 LineRenderer 的透明度，使其在 desolveTime 时间内逐渐“消失”
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(col, 0.0f), new GradientColorKey(col, 1.0f) },
                //随时间推移，Alpha为0的位置越来越靠近1，（t/desolveTime-0.5f/lineLength）是让尾端多少带一点透明渐变
                new GradientAlphaKey[] { new GradientAlphaKey(0, t/desolveTime-0.5f/lineLength), new GradientAlphaKey(1, t/desolveTime) }

            );
            l.colorGradient = gradient;
            float length = t/desolveTime * lineLength+0.5f/lineLength;//当前应该消失的长度 length
            if(!calculated){
                nextLength = Vector2.Distance(ed.points[0],ed.points[1]);
            }
            //当前应该消失的长度 length > 下一段的长度加上已经删除的路径长度，就移除碰撞器的点
            if (length>nextLength+deletedPath){
                deletedPath = nextLength+deletedPath;
                nextLength = 0;
                calculated = false;
                Vector2[] points = ed.points;
                RemoveAt(ref points,0);
                ed.points = points;
            }
            yield return null;
        }
        Destroy(gameObject);
    }
    IEnumerator timer(){
        yield return new WaitForSeconds(timeSpan);
        Desolve();
    }
     public static void RemoveAt<T>(ref T[] arr, int index)
    {
        for (int a = index; a < arr.Length - 1; a++)
        {
            // moving elements downwards, to fill the gap at [index]
            arr[a] = arr[a + 1];
        }
        // finally, let's decrement Array's size by one
        Array.Resize(ref arr, arr.Length - 1);
    }
}
/*
 * DesolveAnim函数解释
 * 
这个 `DesolveAnim` 函数是一个协程（Coroutine），它在一段时间内逐步改变 `LineRenderer` 的透明度，并移除 `EdgeCollider2D` 的点，从而实现游戏对象的“消失”效果。以下是对这个函数逐行的详细解释：

```csharpIEnumerator DesolveAnim(float desolveTime, float lineLength) {
 float t =0;
 bool calculated = false;
 float nextLength =0;
 float deletedPath =0;
 while (t < desolveTime) {
 t += Time.deltaTime;
```
- `desolveTime`：消失过程的总时间。
- `lineLength`：`LineRenderer` 的总长度。
- `t`：当前消失的进度时间。
- `calculated`：标志是否已经计算了当前段的长度。
- `nextLength`：下一段的长度。
- `deletedPath`：已经删除的路径长度。

这部分代码初始化了一些变量，并开始一个 `while` 循环，该循环将在 `t` 小于 `desolveTime` 时执行。

```csharp Gradient gradient = new Gradient();
 gradient.SetKeys(
 new GradientColorKey[] { new GradientColorKey(col,0.0f), new GradientColorKey(col,1.0f) },
 new GradientAlphaKey[] { new GradientAlphaKey(0, t / desolveTime -0.5f / lineLength), new GradientAlphaKey(1, t / desolveTime) }
 );
 l.colorGradient = gradient;
```
- 创建一个新的 `Gradient` 对象。
- 设置颜色和透明度的关键帧 (`GradientColorKey` 和 `GradientAlphaKey`)。
 -颜色关键帧：从 `col`颜色开始，到 `col`颜色结束。
 -透明度关键帧：从 `(0, t / desolveTime -0.5f / lineLength)` 到 `(1, t / desolveTime)`。
- 将新的 `Gradient` 分配给 `LineRenderer` 的 `colorGradient` 属性。

这部分代码逐步减少 `LineRenderer` 的透明度，使其在 `desolveTime` 时间内逐渐“消失”。

```csharp float length = t / desolveTime * lineLength +0.5f / lineLength;
 if (!calculated) {
 nextLength = Vector2.Distance(ed.points[0], ed.points[1]);
 calculated = true;
 }
```
-计算当前应该消失的长度 `length`。
- 如果尚未计算当前段的长度（`calculated` 为 `false`），则计算 `EdgeCollider2D` 的第一个点与第二个点之间的距离，并将其存储在 `nextLength` 中。然后将 `calculated` 设置为 `true`。

```csharp if (length > nextLength + deletedPath) {
 deletedPath = nextLength + deletedPath;
 nextLength =0;
 calculated = false;
 Vector2[] points = ed.points;
 RemoveAt(ref points,0);
 ed.points = points;
 }
 yield return null;
 }
 Destroy(gameObject);
}
```
- 如果当前长度 `length` 超过了下一段的长度加上已经删除的路径长度：
 - 更新 `deletedPath`以包括 `nextLength`。
 - 将 `nextLength` 重置为 `0`，并将 `calculated` 设置为 `false`。
 - 获取 `EdgeCollider2D` 的点数组 `points`。
 - 调用 `RemoveAt` 方法从数组中移除第一个点。
 - 将更新后的点数组重新赋值给 `EdgeCollider2D` 的 `points` 属性。
- 使用 `yield return null`使协程在下一帧继续执行。
- 当 `t` 达到或超过 `desolveTime` 时，退出 `while` 循环并销毁游戏对象。

### 总结这个协程逐步减少 `LineRenderer` 的透明度，同时逐段移除 `EdgeCollider2D` 的碰撞点，从而实现游戏对象的“消失”效果。其核心机制是在一段时间内（`desolveTime`）根据当前时间进度 `t` 调整 `LineRenderer` 的渐变颜色，并根据计算的长度逐段移除 `EdgeCollider2D` 的碰撞点。
 */