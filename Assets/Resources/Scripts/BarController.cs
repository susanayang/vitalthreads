using UnityEngine;

public class BarController : MonoBehaviour
{
    // ����4�ַ��ε�Ԥ����
    public GameObject[] squarePrefabs;

    // ����5��λ�õ���Ϸ����
    public GameObject position0;
    public GameObject position1; //(-20.5, 4.5) = first
    public GameObject position2; //(-20.5, 1.5) = second
    public GameObject position3; //(-20.5, -1.5)
    public GameObject position4; //(-20.5, -4.5)

    // ����һ���������洢λ��
    public GameObject[] positions;
    public GameObject[] squaresOnBar;

    // ����һ�������������
    private System.Random random;

    // ���������ٶ�
    public float dropSpeed = 0.1f;

    // �ڿ�ʼʱ��ʼ������������������
    void Start()
    {
        squaresOnBar = new GameObject[5];
        positions = new GameObject[] { position0, position1, position2, position3, position4 };
        random = new System.Random();
        for (int i = 0; i < positions.Length; i++)
        {
            // ���ѡ��һ�ַ��ε�Ԥ����
            int index = random.Next(0, 4);

            // �ڸ�λ��ʵ����һ������
            squaresOnBar[i] = (GameObject)Instantiate(squarePrefabs[index], positions[i].transform.position, Quaternion.identity);
            // ����ʱ�����ת
            float rotateAngle = -90f;
            index = random.Next(0, 4);
            squaresOnBar[i].transform.Rotate(Vector3.forward * rotateAngle * index);
        }
    }

    private void Update()
    {
        // ���䶯��
        for(int i = squaresOnBar.Length - 1; i > 0; --i)
        {
            Vector3 startPosition = squaresOnBar[i].transform.position;
            Vector3 tagetPosition = positions[i].transform.position;
            squaresOnBar[i].transform.position = Vector3.MoveTowards(startPosition, tagetPosition, dropSpeed);
        }
    }

    //�ڷ���һ��������һ���·���
    public void generateNewSquare()
    {
        int index = random.Next(0, 4);
        squaresOnBar[0] = (GameObject)Instantiate(squarePrefabs[index], position0.transform.position, Quaternion.identity);
        // ����ʱ�����ת
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
