using UnityEngine;

public class LaserTriggerMiniGame : MonoBehaviour
{
    [Header("Laser")]
    public Transform origin;             // iš kur šauna lazeris
    public Vector2 direction = Vector2.right;
    public float distance = 50f;
    public LayerMask hitMask;

    [Header("MiniGame")]
    public MiniGameManager miniGame;     // priskirk per Inspector

    private bool started;

    void Update()
    {
        if (started) return;

        Vector2 o = origin ? (Vector2)origin.position : (Vector2)transform.position;
        Vector2 dir = direction.normalized;

        RaycastHit2D hit = Physics2D.Raycast(o, dir, distance, hitMask);

        if (hit.collider != null)
        {
            // jei pataikom į Lens (tag'u arba komponentu)
            if (hit.collider.CompareTag("Lens"))
            {
                started = true;
                miniGame.Open(); // paleidžiam mini žaidimą
            }
        }
    }
}
