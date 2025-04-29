using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudopodController : MonoBehaviour
{
    public List<VerletPoint> points;
    public float pseudopodForce = 5f;
    public float retractionForce = 3f;
    public float pseudopodInterval = 1f;
    private float _counter;

    private float nextPseudopodTime = 0f;
    private VerletPoint leadingPoint;
    private VerletPoint trailingPoint;
    private Vector2 direction;

    public float pseudopodTargetDistance = 1f;
    private bool _pseudopoding;
    private Coroutine _pseudopodCoroutine;

    public delegate void MovingInDirection(Vector2 force);
    public MovingInDirection onMovingInDirection;
    void Start()
    {
        //StartCoroutine(formPseudopod());
    }

    public void MakePseudopod(Vector2 point, int amount)
    {
        //don't do it if it's already happening. 
        if (_pseudopodCoroutine == null)
            _pseudopodCoroutine = StartCoroutine(formPseudopod(point, amount));
    }


    IEnumerator formPseudopod(Vector2 point, int amount = 1)
    {
        ExtendPseudopod(point);
        //float distance = Vector2.Distance(leadingPoint.position, GetCellCenter());
        direction = (leadingPoint.position - GetCellCenter()).normalized;
        //Debug.Log($"distance {distance} and direction {direction} ");
        amount *= 2; //offset

        while (_counter < amount)
        {
            //Debug.Log(_counter);
            _counter += 0.016f;
            leadingPoint.AddForce(direction * pseudopodForce);
            //trailingPoint.AddForce(direction * pseudopodForce);
            onMovingInDirection?.Invoke(direction * pseudopodForce);
            yield return null;

        }
        Vector2 contractionDirection = -direction;//(GetCellCenter() - trailingPoint.position).normalized;
        while (_counter < (amount*2))
        {
           // Debug.Log(_counter);
            _counter += 0.016f;
            leadingPoint.AddForce(contractionDirection * retractionForce);
            trailingPoint.AddForce(direction * retractionForce/2);  //drag the butt forward a little 
            onMovingInDirection?.Invoke(contractionDirection * (retractionForce));
            yield return null;
            
        }
        _counter = 0;
        _pseudopodCoroutine = null;
        onMovingInDirection?.Invoke(Vector2.zero);


    }

    void Update()
    {
       /* if (_pseudopoding)
        {
            if (_counter == 0)
            
            _startPseudopod = false;
        }
        _counter++;
        if (_counter >= pseudopodInterval)
        {
           

        }
        else
        {
            if (_counter == 0)
                ExtendPseudopod();
            if (leadingPoint != null && trailingPoint != null)
            {
                direction = (leadingPoint.position - GetCellCenter()).normalized;
                float distance = Vector2.Distance(leadingPoint.position, GetCellCenter());

                // Only extend if it's below the target distance
                if (distance < pseudopodTargetDistance)
                {
                    leadingPoint.AddForce(direction * pseudopodForce);
                }

                // Always retract (until the shape stabilizes)
               
            }
        }*/

        
    }

    void ExtendPseudopod(Vector2 mousePoint)
    {
        // Identify leading edge point based on distance and direction
        leadingPoint = GetLeadingEdgePoint(mousePoint);
        trailingPoint = GetOppositePoint(leadingPoint);
        /*if (leadingPoint != null)
        {
            // Find the trailing edge point on the opposite side of the cell
            
        }*/
    }

    VerletPoint GetLeadingEdgePoint(Vector2 mousePoint)
    {
        VerletPoint bestPoint = null;
        float minDistance = float.MaxValue;
        Vector2 center = GetCellCenter();

        // Choose the point furthest from the center in the direction of movement
        foreach (var point in points)
        {
            float distance = Vector2.Distance(mousePoint, point.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                bestPoint = point;
            }
        }
        return bestPoint;
    }

    VerletPoint GetOppositePoint(VerletPoint referencePoint)
    {
        VerletPoint bestPoint = null;
        float minDot = 1f; // Dot product range is -1 to 1
        Vector2 center = GetCellCenter();
        Vector2 refDirection = (referencePoint.position - center).normalized;

        foreach (var point in points)
        {
            if (point == referencePoint) continue;

            Vector2 direction = (point.position - center).normalized;
            float dot = Vector2.Dot(refDirection, direction);

            // We want the smallest dot product (most negative), which represents the most opposite direction
            if (dot < minDot)
            {
                minDot = dot;
                bestPoint = point;
            }
        }

        // Fallback: if no opposite point is found, pick the nearest point (prevents null reference)
        if (bestPoint == null)
        {
            float minDistance = float.MaxValue;
            foreach (var point in points)
            {
                if (point == referencePoint) continue;

                float distance = Vector2.Distance(point.position, center);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestPoint = point;
                }
            }
        }

        return bestPoint;
    }

    Vector2 GetCellCenter()
    {
        Vector2 center = Vector2.zero;
        foreach (var point in points)
        {
            center += point.position;
        }
        return center / points.Count;
    }
}
