using UnityEngine;
using System.Collections.Generic;

public class CellPhysicsManager : MonoBehaviour
{
   // public float internalPressure = 10f;
    public float centeringForce = 0.5f;
    public float volumePreservationForce = 5f;
    public float radialForce = 2f; // Increase if it’s too loose

    private List<VerletPoint> membranePoints;
    private float targetArea;
    //public float pressureStrength = 10f;

    void Start()
    {
        if (membranePoints == null)
            membranePoints = GetComponent<CellController>().GetVerletPoints();
        targetArea = CalculateArea();
    }
    /*
    void ApplyInternalPressure()
    {
        float currentArea = CalculateArea();
        float pressureForce = (targetArea - currentArea) * pressureStrength;

        Vector2 center = GetCellCenter();

        foreach (var point in membranePoints)
        {
            Vector2 direction = (point.position - center).normalized;
            point.AddForce(direction * pressureForce * Time.fixedDeltaTime);
        }
    }*/

    float CalculateArea()
    {
        float area = 0;
        int count = membranePoints.Count;

        for (int i = 0; i < count; i++)
        {
            int next = (i + 1) % count;
            area += membranePoints[i].position.x * membranePoints[next].position.y;
            area -= membranePoints[next].position.x * membranePoints[i].position.y;
        }

        return Mathf.Abs(area) * 0.5f;
    }

    void ApplyRadialRestoration()
    {
        Vector2 center = GetCellCenter();
        float targetRadius = Mathf.Sqrt(targetArea / Mathf.PI); // Target radius for circular shape

        foreach (var point in membranePoints)
        {
            Vector2 direction = (point.position - center).normalized;
            float distance = Vector2.Distance(point.position, center);

            // If it's too far or too close, apply a correction force
            float correction = (targetRadius - distance) * radialForce;
            point.AddForce(direction * correction * Time.fixedDeltaTime);
        }
    }

    void FixedUpdate()
    {
        //ApplyInternalPressure();
        ApplyCenteringForce();
        ApplyVolumePreservation();
        ApplyRadialRestoration(); // New circular correction force
    }

   

    void ApplyCenteringForce()
    {
        Vector2 center = GetCellCenter();

        foreach (var point in membranePoints)
        {
            Vector2 directionToCenter = center - point.position;
            point.AddForce(directionToCenter * centeringForce * Time.fixedDeltaTime);
        }
    }

    void ApplyVolumePreservation()
    {
        float currentArea = CalculateArea();
        float areaDifference = targetArea - currentArea;

        Vector2 center = GetCellCenter();

        foreach (var point in membranePoints)
        {
            Vector2 direction = (point.position - center).normalized;
            Vector2 volumeCorrection = direction * areaDifference * volumePreservationForce * Time.fixedDeltaTime;
            point.AddForce(volumeCorrection);
        }
    }

   

    Vector2 GetCellCenter()
    {
        Vector2 center = Vector2.zero;
        foreach (var point in membranePoints)
        {
            center += point.position;
        }
        return center / membranePoints.Count;
    }
}
