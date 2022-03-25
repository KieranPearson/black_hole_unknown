using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TiltSpriteOnMovement : MonoBehaviour
{
    [SerializeField] private int timeTillIdle;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite strafeLeftSprite;
    [SerializeField] private Sprite strafeRightSprite;
    
    private SpriteRenderer spriteRenderer;

    private enum direction {LEFT, NONE, RIGHT};
    private direction tiltDirection = direction.NONE;

    private float timeOfLastCmd = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        CheckForIdle();
    }

    private void CheckForIdle()
    {
        if (tiltDirection == direction.NONE) return;
        if ((Time.time - timeOfLastCmd) >= timeTillIdle)
        {
            tiltDirection = direction.NONE;
            spriteRenderer.sprite = normalSprite;
        }
    }

    public void TiltSpriteLeft()
    {
        tiltDirection = direction.LEFT;
        spriteRenderer.sprite = strafeLeftSprite;
        timeOfLastCmd = Time.time;
    }

    public void TiltSpriteRight()
    {
        tiltDirection = direction.RIGHT;
        spriteRenderer.sprite = strafeRightSprite;
        timeOfLastCmd = Time.time;
    }
}
