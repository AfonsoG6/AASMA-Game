using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] Door DOOR_TO_OPEN;
    [SerializeField] Sprite SPRITE_BASE;
    [SerializeField] Sprite SPRITE_PRESSED;

    SpriteRenderer spriteRenderer = null;

    private int pressing = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = SPRITE_BASE;
    }

    // Update is called once per frame
    void Update()
    {
        if (pressing > 0 && spriteRenderer != null) {
            spriteRenderer.sprite = SPRITE_PRESSED;
        }
        else {
            spriteRenderer.sprite = SPRITE_BASE;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Box") {
            DOOR_TO_OPEN.addObjectOpening(gameObject);
            pressing++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Box") {
            DOOR_TO_OPEN.removeObjectOpening(gameObject);
            pressing--;
        }
    }
}
