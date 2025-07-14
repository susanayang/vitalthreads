using UnityEngine;

public class BarController : MonoBehaviour
{
    // 定义4种方形的预制体
    public GameObject[] squarePrefabs;

    // 定义5个位置的游戏对象
    public GameObject position0;
    public GameObject position1; //(-20.5, 4.5) = first
    public GameObject position2; //(-20.5, 1.5) = second
    public GameObject position3; //(-20.5, -1.5)
    public GameObject position4; //(-20.5, -4.5)

    // 定义一个数组来存储位置
    public GameObject[] positions;
    public GameObject[] squaresOnBar;

    // 定义一个随机数生成器
    private System.Random random;

    // 定义下落速度
    public float dropSpeed = 0.1f;

    // 在开始时初始化数组和随机数生成器
    void Start()
    {
        squaresOnBar = new GameObject[5];
        positions = new GameObject[] { position0, position1, position2, position3, position4 };
        random = new System.Random();
        for (int i = 0; i < positions.Length; i++)
        {
            // 随机选择一种方形的预制体
            int index = random.Next(0, 4);

            // 在该位置实例化一个方形
            squaresOnBar[i] = (GameObject)Instantiate(squarePrefabs[index], positions[i].transform.position, Quaternion.identity);
            // 生成时随机旋转
            float rotateAngle = -90f;
            index = random.Next(0, 4);
            squaresOnBar[i].transform.Rotate(Vector3.forward * rotateAngle * index);
        }
    }

    private void Update()
    {
        // 下落动画
        for(int i = squaresOnBar.Length - 1; i > 0; --i)
        {
            Vector3 startPosition = squaresOnBar[i].transform.position;
            Vector3 tagetPosition = positions[i].transform.position;
            squaresOnBar[i].transform.position = Vector3.MoveTowards(startPosition, tagetPosition, dropSpeed);
        }
    }

    //在放置一个后生成一个新方形
    public void generateNewSquare()
    {
        int index = random.Next(0, 4);
        squaresOnBar[0] = (GameObject)Instantiate(squarePrefabs[index], position0.transform.position, Quaternion.identity);
        // 生成时随机旋转
        float rotateAngle = -90f;
        index = random.Next(0, 4);
        squaresOnBar[0].transform.Rotate(Vector3.forward * rotateAngle * index);
    }

    //checks if square is last square
    public bool lastPosition(Vector2 squarePosition)
    {
        Vector2 p4 = position4.transform.position;
        if (squarePosition == p4){
            return true;
        }
        return false;
    }

    //drop squares on left bar   
    public void dropSquares()
    {
        for (int i = squaresOnBar.Length-1; i > 0 ; --i)
        {
            squaresOnBar[i] = squaresOnBar[i-1];
            //squaresOnBar[i].transform.position = positions[i].transform.position;
        }
    }
}
