using UnityEngine;

public class TargetController : MonoBehaviour
{
    // 判断该点是否到达
    bool arrived = false;

    bool isChanging = false;

    public float speed = 0.005f;

    SpriteRenderer buildingSprite, fogSprite;

    void Start()
    {
        GameObject fog = transform.Find("Fog").gameObject;
        fogSprite = fog.GetComponent<SpriteRenderer>();
        GameObject building = transform.Find("Building").gameObject;
        buildingSprite = building.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isChanging)
        {
            if (fogSprite.color.a <= 0 && buildingSprite.color.a >= 100)
            {
                isChanging = false;
            }
            buildingSprite.color = new Color(buildingSprite.color.r, buildingSprite.color.g, buildingSprite.color.b, buildingSprite.color.a + speed);
            fogSprite.color = new Color(fogSprite.color.r, fogSprite.color.g, fogSprite.color.b, fogSprite.color.a - speed);
        }
    }

    // 改变是否到达的状态
    public void change()
    {
        if (!arrived)
        {
            arrived = true;
            isChanging = true;
        }
    }
}
