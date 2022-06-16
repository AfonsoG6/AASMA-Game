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

    public bool isOpen() {
        return objectsOpening.Count > 0;
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
    
    // Optimization
    public List<Button> getReachableButtons(Vector3 agentPosition) {
        Vector3 doorPosition = gameObject.transform.position;
        List<Button> reachableButtons = new List<Button>();
        foreach (Button button in buttons) {
            Vector3 buttonPosition = button.gameObject.transform.position;
            if ((agentPosition.x <= doorPosition.x && buttonPosition.x <= doorPosition.x) ||
                (agentPosition.x >= doorPosition.x && buttonPosition.x >= doorPosition.x)) {
                reachableButtons.Add(button);
            }
        }
        return reachableButtons;
    }

    public bool areBothPlayersOnSameSide() {
        Vector3 position1 = GameObject.Find("Player1").transform.position;
        Vector3 position2 = GameObject.Find("Player2").transform.position;
        Vector3 doorPosition = transform.position;
        return (position1.x <= doorPosition.x && position2.x <= doorPosition.x) ||
                (position1.x >= doorPosition.x && position2.x >= doorPosition.x);
    }

    public void addButton(Button b) {
        buttons.Add(b);
    }
}
