using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class DoorMovement : MonoBehaviour
{
    public bool isHorizontal = true;
    private float moveDistance = 2.5f;
    private float moveSpeed = 2f;
    private Vector3 initialPosition;
    private NavMeshObstacle navMeshObstacle;

    void Start()
    {
        initialPosition = transform.position;
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        StartCoroutine(MoveDoorContinuously());
    }

    IEnumerator MoveDoorContinuously()
    {
        while (true)
        {
            StartCoroutine(MoveDoor(true)); // Close the door
            yield return new WaitForSeconds(1.5f); // Wait for 3 seconds
            StartCoroutine(MoveDoor(false));
            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator MoveDoor(bool close)
    {
        Vector3 targetPosition = close ? initialPosition : initialPosition - new Vector3(0f, moveDistance, 0f);

        // Enable the NavMeshObstacle to block the NavMeshAgent
        navMeshObstacle.enabled = close;

        StartCoroutine(MoveTo(targetPosition));

        // Wait until the door reaches its target position
        yield return new WaitUntil(() => Vector3.Distance(transform.position, targetPosition) < 0.01f);

        // Disable the NavMeshObstacle to allow the NavMeshAgent to pass only when opening
        if (!close)
        {
            navMeshObstacle.enabled = false;
        }
    }

    IEnumerator MoveTo(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}