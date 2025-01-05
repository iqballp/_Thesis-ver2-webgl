using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Vector3 mousePosition;
    public float moveSpeed = 0.1f;
    Rigidbody2D rb;
    Vector2 position;
    Vector3 rotation;

    private GameManager gm;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnMouseDrag()
    {
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
        rb.MovePosition(position);

        var desiredRotQ = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -26f);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime *10f);
    }

    private void OnMouseDown()
    {
       // Debug.Log("Touched");
        gm.addSelected(this.gameObject);
    }
}
