using System.Collections;
using UnityEngine;

public class DoorMovement : MonoBehaviour
{
    public bool isHorizontal = true;

    private float moveDistance = 3f;
    private float moveSpeed = 2f;

    private Vector3 initialPosition;

    void Start()
    {
        // Save the initial position of the door
        initialPosition = transform.position;

        // Start the movement coroutine
        StartCoroutine(MoveDoor());
    }

    IEnumerator MoveDoor()
    {
        while (true)
        {
            // Move left after 2 seconds
            yield return new WaitForSeconds(2f);

            Vector3 targetPosition;
            if (isHorizontal)
                targetPosition = initialPosition - new Vector3(moveDistance, 0f, 0f);
            else
                targetPosition = initialPosition - new Vector3(0f, 0f, moveDistance);

            StartCoroutine(MoveTo(targetPosition));

            // Wait for 3 seconds
            yield return new WaitForSeconds(3f);

            // Move back to the original position
            StartCoroutine(MoveTo(initialPosition));
        }
    }

    IEnumerator MoveTo(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            // Move the door towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}