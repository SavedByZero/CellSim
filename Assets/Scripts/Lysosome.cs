using UnityEngine;

public class Lysosome : CellOrganelle
{
    public float digestionPower = 10f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Invader")) // Assume invaders have this tag
        {
            Destroy(other.gameObject);
        }
    }
}
