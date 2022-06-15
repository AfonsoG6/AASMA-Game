using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] Door DOOR_TO_OPEN;
    [SerializeField] Sprite SPRITE_BASE;
    [SerializeField] Sprite SPRITE_PRESSED;

    SpriteRenderer spriteRenderer = null;

    private int playersPressing = 0;
    private int boxesPressing = 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = SPRITE_BASE;
        DOOR_TO_OPEN.addButton(this);
        GameObject LinePrefab = Resources.Load<GameObject>("Puzzles/LineRenderer");
        LineRenderer line = Instantiate(LinePrefab, transform.position, Quaternion.identity).GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
        line.SetPosition(1, DOOR_TO_OPEN.transform.position);
        Color randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        line.startColor = randomColor;
        line.endColor = randomColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed() && spriteRenderer != null) {
            spriteRenderer.sprite = SPRITE_PRESSED;
        }
        else {
            spriteRenderer.sprite = SPRITE_BASE;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            DOOR_TO_OPEN.addObjectOpening(gameObject);
            playersPressing++;
        }
        else if (other.gameObject.tag == "BoxPhysics") {
            DOOR_TO_OPEN.addObjectOpening(gameObject);
            boxesPressing++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            DOOR_TO_OPEN.removeObjectOpening(gameObject);
            playersPressing--;
        }
        if (other.gameObject.tag == "BoxPhysics") {
            DOOR_TO_OPEN.removeObjectOpening(gameObject);
            boxesPressing--;
        }
    }

    public Door getDoor()
    {
        return DOOR_TO_OPEN;
    }

    public bool pressed() {
        return playersPressing+boxesPressing > 0;
    }

    public bool pressedByBox() {
        return boxesPressing > 0;
    }

    public bool pressedByPlayer() {
        return playersPressing > 0;
    }
}
