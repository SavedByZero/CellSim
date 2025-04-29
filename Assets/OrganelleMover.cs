using UnityEngine;

public class OrganelleMover : MonoBehaviour
{
    private CellOrganelle[] organelles; // Assign all organelles in the Inspector
    private Cytoskeleton cytoskeleton;
    private PseudopodController pseudopodController;

    void Start()
    {
        cytoskeleton = GetComponentInChildren<Cytoskeleton>();
        organelles = GetComponentsInChildren<CellOrganelle>();
        pseudopodController = GetComponentInChildren<PseudopodController>();
        if (organelles.Length == 0)
        {
            Debug.LogWarning("No organelles assigned to OrganelleMover on " + gameObject.name);
        }
        if (pseudopodController == null)
        {
            Debug.Log("Warning: no pseudopod controller detected. Organelles will not move without it.");
        }
        else
        {
            pseudopodController.onMovingInDirection += MoveOrganelles;
        }
    }

    /// <summary>
    /// Moves all organelles in the given direction with the specified force.
    /// Called by the PseudopodController.
    /// </summary>
    public void MoveOrganelles(Vector2 force)
    {
        if (cytoskeleton == null)
        {
            Debug.LogError("No cytoskeleton to move!");
            return;
        }
        cytoskeleton.Rb2D.AddForce(force * Time.deltaTime);
        /*foreach (var organelle in organelles)
        {
            if (organelle != null)
            {
               // Vector2 movement = direction.normalized * force.magnitude * Time.deltaTime;
                organelle.Rb2D.AddForce(force * Time.deltaTime);
            }
        }*/
    }
}
