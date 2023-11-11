using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Transform player;
    public LayerMask doorLayer;

    private float patrolSpeed = 3f;
    private float chaseSpeed = 3f;
    private float detectionRadius = 7f;

    private NavMeshAgent agent;
    private bool playerDetected = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(RandomPatrol());
    }

    IEnumerator RandomPatrol()
    {
        while (true)
        {
            // Generate a random point on the NavMesh within the detection radius
            Vector3 randomPoint = RandomNavMeshPoint(transform.position, detectionRadius, -1);

            // Check if the player is within the detection radius
            if (!playerDetected && Vector3.Distance(transform.position, player.position) < detectionRadius)
            {
                // Check if there's no door between the AI and the player
                if (!IsDoorBetween())
                {
                    StopAllCoroutines();
                    StartCoroutine(ChasePlayer());
                    yield break; // Exit the patrol coroutine
                }
            }

            // Move to the random point
            agent.speed = patrolSpeed;
            agent.SetDestination(randomPoint);

            // Wait for a short duration before the next random patrol
            yield return new WaitForSeconds(Random.Range(2f, 5f));

            // Reset playerDetected state after completing patrol
            playerDetected = false;
        }
    }

    IEnumerator ChasePlayer()
    {
        playerDetected = true;

        // Chase the player
        while (Vector3.Distance(transform.position, player.position) > 2f)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
            yield return null;
        }

        // Player caught, handle losing logic here
        Debug.Log("Player caught!");

        // Stop AI and player movement
        agent.isStopped = true;
        player.GetComponent<NavMeshAgent>().isStopped = true;
    }

    bool IsDoorBetween()
    {
        // Check if there's a door between the AI and the player
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.position - transform.position, out hit, detectionRadius, doorLayer))
        {
            return hit.collider.CompareTag("Door");
        }

        return false;
    }

    Vector3 RandomNavMeshPoint(Vector3 origin, float radius, int areaMask)
    {
        // Generate a random point on the NavMesh
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, areaMask);

        return hit.position;
    }
}