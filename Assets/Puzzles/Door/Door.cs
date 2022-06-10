using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    List<Button> buttons = new List<Button>();
    Animator animator = null;
    Collider2D hitbox = null;
    List<GameObject> objectsOpening = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Open", false);

        hitbox = GetComponent<Collider2D>();
        hitbox.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        bool open = objectsOpening.Count > 0;
        if (animator != null) {
            animator.SetBool("Open", open);
        }
        if (hitbox != null) {
            hitbox.enabled = !open;
        }
    }

    public void addObjectOpening(GameObject obj)
    {
        objectsOpening.Add(obj);
    }

    public void removeObjectOpening(GameObject obj)
    {
        objectsOpening.Remove(obj);
    }

    public List<Button> getButtons() {
        return buttons;
    }

    public void addButton(Button b) {
        buttons.Add(b);
    }
}
