using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twinkle : MonoBehaviour
{
    int its = 3;
    int it = 0;

    public float speed = 0.01f;

    SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (it < its)
        {
            if (sprite.color.a > 0) //当还没达到目标透明度
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - speed);
            }
            else
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
                ++it;
            }
        }
        else
        {
            if (sprite.color.a > 0) //当还没达到目标透明度
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - speed);
            }
        }
    }
}
