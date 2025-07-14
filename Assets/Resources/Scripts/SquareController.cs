using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class SquareController : MonoBehaviour
{
    //自定义网格边框
    // 起始点(-7,-1)，-7<=x<=3,-5<=y<=4
    public double topBorder; 
    public double bottomBorder;
    public double leftBorder;
    public double rightBorder;

    public static bool levelEnd = false;
    
    //selected puzzle piece, null if no puzzle piece is selected
    private GameObject selectedObject;
    // 储存原位置
    Vector2 originalPosition;
    // 判断是否可移动
    private bool allowMove = true;
    // 定义旋转角度
    int rotateAngle = 90;
    // 定义旋转速度，须为90的因子
    public int rotateSpeed = 2;
    // 是否继续旋转
    bool isRotate = false;
    //记录已经旋转的角度
    int rotateCount = 0; 
    // 存储 barController 的引用
    private BarController bc;

    // 用来存储所有的square
    public static List<SquareController> squares = new List<SquareController>();
    // 用来存储所有的circle
    public static List<Transform> circles = new List<Transform>();
    // 用来获取自身的矩形碰撞器
    private BoxCollider2D boxCollider;
    // 用来获取自身的矩形变换组件
    private RectTransform rectTransform;
    // 用来获取自身的子物体circle
    private Transform[] childCircles;

    // 判断是否达到目标点，存储目标点
    GameObject[] targets;

    GameObject endP;

    void Start()
    {
        // 获取 BarController 的引用
        bc = GameObject.Find("Bar").GetComponent<BarController>();

        // 获取目标点
        targets = GameObject.FindGameObjectsWithTag("Target");

        endP = GameObject.Find("EndPoint");

        // 将自身添加到squares列表中
        squares.Add(this);

        // 获取自身的组件
        boxCollider = GetComponent<BoxCollider2D>();
        rectTransform = GetComponent<RectTransform>();
        //Debug.Log(rectTransform.sizeDelta.x);

        // 获取子物体circle，并添加到circles列表中
        childCircles = GetComponentsInChildren<Transform>();
        foreach (var circle in childCircles)
            if (circle != transform)
                circles.Add(circle);
    }

    private void Update()
    {
        //left click
        if (Input.GetMouseButtonDown(0) && allowMove)
        {
            if (selectedObject == null)
            {
                //calls function, casts ray from camera to mouse position
                RaycastHit2D hit = CastRay();
                //pick up object
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    originalPosition = gameObject.transform.position;
                    if (bc.lastPosition(originalPosition))
                    {
                        selectedObject = gameObject;
                    }
                }
            }
            else
            {
                //drop object
                // 放置在整数坐标上，方便对齐
                Vector2 roundedPosition = new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x),
                                                                                               Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
                selectedObject.transform.position = roundedPosition;
                if (canPlace())
                {
                    selectedObject = null;
                    // 放置过一次后将无法再移动
                    allowMove = false;

                    bc.dropSquares();
                    // 调用 BarController 的方法在第一个位置生成新的 square
                    bc.generateNewSquare();

                    // 放置后判断是否到达目标点
                    foreach(var circle in childCircles)
                    {
                        foreach(var target in targets)
                        {
                            if(Mathf.Round(circle.transform.position.x) == Mathf.Round(target.transform.position.x) 
                                && Mathf.Round(circle.transform.position.y) == Mathf.Round(target.transform.position.y))
                            {
                                TargetController tc = target.GetComponent<TargetController>();
                                tc.change();
                            }
                            if (Mathf.Round(circle.transform.position.x) == Mathf.Round(endP.transform.position.x)
                                && Mathf.Round(circle.transform.position.y) == Mathf.Round(endP.transform.position.y))
                            {
                                levelEnd = true;
                            }
                        }
                    }
                }
                else
                {
                    // 恢复原位
                    selectedObject.transform.position = originalPosition;
                    selectedObject = null;
                }
            }
        }

        // 如果鼠标右键被按下
        if (Input.GetMouseButtonDown(1) && allowMove && bc.lastPosition(originalPosition) && isRotate==false)
        {
            RaycastHit2D hit = CastRay();
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // 以 z 轴为中心，顺时针旋转方形指定的角度
                //transform.Rotate(Vector3.forward * rotateAngle);
                rotateCount = 0;
                isRotate = true;
            }
        }

        if (isRotate)
        {
            if (rotateCount < rotateAngle) //当还没达到目标角度时
            {
                rotateCount += rotateSpeed; //增加已经旋转的角度
                transform.Rotate(Vector3.forward * rotateSpeed); //插值当前旋转和目标旋转
            }
            else
            {
                isRotate = false;
            }
        }

        if (selectedObject != null)
        {
            //drag object
            selectedObject.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                                                                                             Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        }
    }

    //function that casts ray from camera to mouse position
    private RaycastHit2D CastRay()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, ray.direction);
        return hit;
    }

    // 判断放置位置是否合法
    private bool canPlace()
    {
        // 不能将方形放置在网格外
        double x = Math.Round(transform.position.x);
        double y = Math.Round(transform.position.y);
        if(x < leftBorder || x > rightBorder || y < bottomBorder || y > topBorder)
        {
            return false;
        }

        // 遍历所有的square
        foreach (var square in squares)
        {
            // 如果不是自身
            if (square != this && square != null)
            {
                // 检测自身的矩形是否与其他square的矩形重叠
                Vector2 thisP = gameObject.transform.position;
                Vector2 otherP = square.transform.position;
                if (Vector2.Distance(thisP, otherP) < 2 * rectTransform.sizeDelta.x)
                    return false;
            }
        }

        // 遍历自身的子物体circle
        foreach (var circle in childCircles)
        {
            // 如果不是自身
            if (circle != transform)
            {
                // 获取circle的世界坐标和半径
                Vector2 circlePosition = circle.position;
                // 遍历所有的circle
                foreach (var otherCircle in circles)
                {
                    // 如果不是自身的子物体circle
                    if (!childCircles.Contains(otherCircle) && otherCircle != null)
                    {
                        // 获取otherCircle的世界坐标和半径
                        Vector2 otherCirclePosition = otherCircle.position;
                        // 检测两个circle是否重合
                        if (circlePosition == otherCirclePosition)
                            return true;
                    }
                }
            }
        }
        // 如果没有重叠或相交，则返回false
        return false;
    }
}