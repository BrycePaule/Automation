using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimIterateSprites : MonoBehaviour
{
    [SerializeField] private int framesPerSprite;
    [SerializeField] private Sprite[] sprites;

    private SpriteRenderer sr;
    private int counter;
    private int selected;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        counter = 0;
        selected = 0;
    }

    private void Update()
    {
        counter += 1;

        if (counter >= framesPerSprite)
        {
            counter = 0;
            selected += 1;

            if (selected >= sprites.Length) { selected = 0; }

            sr.sprite = sprites[selected];
        }
    }
}
