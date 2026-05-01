using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyScan : MonoBehaviour
{
    [SerializeField] private float scanInterval = 0.1f;

    public PlayerController closestEnemy { get; set; }
    public Vector2 DirectionToEnemy {  get; set; }
    public float DistanceToEnemy {  get; set; }

    private PlayerController player;
    private float scanTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerController>();        
    }

    // Update is called once per frame
    void Update()
    {
        scanTimer -= Time.deltaTime;

        if(scanTimer <= 0)
        {
            Scan();
            scanTimer = scanInterval;
        }
        else if(closestEnemy != null)
        {
            RefreshDirection();
        }
    }

    private void Scan()
    {
        PlayerController[] enemies = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        PlayerController closest = null;
        float closestDistance = float.MaxValue;

        foreach (var p in enemies) {

            if (p == player) continue;

            float distance = Vector2.Distance(transform.position, p.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closest = p;
            }

        }

        closestEnemy = closest;
        DistanceToEnemy = closestDistance;
        RefreshDirection();
    }

    private void RefreshDirection()
    {
        if (closestEnemy == null) return;

        Vector2 delta = (Vector2)closestEnemy.transform.position - (Vector2)transform.position;
        DirectionToEnemy = delta.sqrMagnitude > 0.0001f ? delta.normalized : Vector2.right;
        DistanceToEnemy = delta.magnitude;

    }
}
