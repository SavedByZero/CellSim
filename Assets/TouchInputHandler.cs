using UnityEngine;
using System.Collections.Generic;

public class TouchInputHandler : MonoBehaviour
{
    public float dragForce = 10f;

    private Camera mainCamera;
    private List<VerletPoint> membranePoints;

    void Start()
    {
        mainCamera = Camera.main;
        
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        { 
            if (membranePoints == null)
            {
                membranePoints = GetComponentInChildren<CellController>().GetVerletPoints();
            }
            Vector2 targetPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            ApplyDragForce(targetPos);
        }
    }

    void ApplyDragForce(Vector2 targetPos)
    {
        foreach (var point in membranePoints)
        {
            Vector2 direction = targetPos - point.position;
            point.AddForce(direction * dragForce * Time.deltaTime);
        }
    }
}
