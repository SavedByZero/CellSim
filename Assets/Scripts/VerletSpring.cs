using UnityEngine;

public class VerletSpring
{
    public VerletPoint pointA;
    public VerletPoint pointB;
    public float restLength;
    public float stiffness;

    public VerletSpring(VerletPoint a, VerletPoint b, float stiffness)
    {
        pointA = a;
        pointB = b;
        this.restLength = Vector2.Distance(a.position, b.position);
        this.stiffness = stiffness;
    }

    public Vector2 ApplyForce()
    {
        Vector2 delta = pointB.position - pointA.position;
        float distance = delta.magnitude;
        float forceMagnitude = (distance - restLength) * stiffness;
        Vector2 force = delta.normalized * forceMagnitude;

        pointA.AddForce(force);
        pointB.AddForce(-force);
        return force;
    }
}
