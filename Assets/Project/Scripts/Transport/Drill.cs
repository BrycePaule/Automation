using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    [SerializeField] private int framesPerSprite;
    [SerializeField] private Sprite[] sprites;

    private SpriteRenderer sr;
    private int counter;
    private int currSprite;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        counter = 0;
        currSprite = 0;
    }

    private void Update()
    {
        counter += 1;

        if (counter >= framesPerSprite)
        {
            counter = 0;
            currSprite += 1;
            if (currSprite >= sprites.Length) { currSprite = 0; }

            sr.sprite = sprites[currSprite];
        }
    }
}
