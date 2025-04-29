using UnityEngine;

public class VerletPoint
{
    public Vector2 position;
    public Vector2 previousPosition;
    public Vector2 acceleration;

    public VerletPoint(Vector2 initialPosition)
    {
        position = initialPosition;
        previousPosition = initialPosition;
        acceleration = Vector2.zero;
    }

    public void UpdatePosition(float deltaTime)
    {
        // Verlet Integration
        Vector2 velocity = position - previousPosition;
        previousPosition = position;
        position += velocity + acceleration * (deltaTime * deltaTime);
        acceleration = Vector2.zero; // Reset acceleration after each step
    }

    public void AddForce(Vector2 force)
    {
        acceleration += force;
    }
}
