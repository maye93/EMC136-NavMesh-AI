using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;

    private float playerSpeed = 5f; // Adjust the speed as needed

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial speed
        agent.speed = playerSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the clicked object has a collider and is not on the "Door" layer
                if (hit.collider != null && hit.collider.gameObject.layer != LayerMask.NameToLayer("Door"))
                {
                    agent.SetDestination(hit.point);
                }
                // You can add additional conditions or behaviors for objects on the "Door" layer if needed.
            }
        }
    }
}