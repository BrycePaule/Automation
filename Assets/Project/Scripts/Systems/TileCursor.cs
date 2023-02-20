using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCursor : MonoBehaviour
{

    public bool Visible;
    public Sprite Sprite;

    private SpriteRenderer spriteRenderer;

    [Header("References")]
    [SerializeField] private TSystemManager conveyorManager;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite;
    }

    private void Update()
    {
        if (Visible)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    public void UpdatePosition(Vector3 _worldPos)
    {
        transform.position = _worldPos;
    }
}
