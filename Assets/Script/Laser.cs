using UnityEngine;
using UnityEngine.InputSystem;

public class Laser : MonoBehaviour
{
    public LineRenderer line;

    public float maxLength = 10f;
    public LayerMask blockMask;

    private bool playerNear;
    private bool laserOn;

    void Awake()
    {
        if (line == null) line = GetComponent<LineRenderer>();
        line.enabled = false;
        line.positionCount = 2;
    }

    void Update()
    {
        if (playerNear && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            ToggleLaser();

        if (laserOn)
            UpdateLaser();
    }

    void ToggleLaser()
    {
        laserOn = !laserOn;
        line.enabled = laserOn;
        if (laserOn) UpdateLaser();
    }

    void UpdateLaser()
    {
        Vector2 start = transform.position;
        Vector2 dir = (Vector2)transform.right.normalized;

        Debug.DrawRay(start, dir * maxLength, Color.red); // tik debug'ui

        RaycastHit2D hit = Physics2D.Raycast(start, dir, maxLength, blockMask);

        Vector2 end = (hit.collider != null) ? hit.point : start + dir * maxLength;

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerNear = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerNear = false;
    }
}
