using UnityEngine;

public class InjectorVirus : MonoBehaviour, IEnemyAgent
{
    public float speed = 1f;
    public int Children = 1;
    public int Damage = 10;
    public RNA rnaPrefab;
    private bool injected = false;
    private VirusState _state;
    public Vector3 Position => transform.position;

    public CellController Target;

    void Start()
    {
        if (Target == null)
            Target = GameObject.FindFirstObjectByType<CellController>();
        float dist = Vector2.Distance(Target.transform.position, this.transform.position);
        if (dist < 1f)
            Flee();
    }

    public void Attack()
    {
        _state = VirusState.ATTACKING;
    }

    public void Flee()
    {
        _state = VirusState.FLEEING;
    }

    void Update()
    {
        if (_state == VirusState.ATTACKING)
            MoveTowardsCell();
        else if (_state == VirusState.FLEEING)
            EscapeCell();

            
    }

    void MoveTowardsCell()
    {
        // Move toward cell center or membrane (basic example)
        Vector2 direction = ((Vector2)Target.transform.position - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void EscapeCell()
    {
        Vector2 direction = (Vector2.right + (Vector2.up*Random.Range(-1,1)) ) - ((Vector2)Target.transform.position);
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       
       // if (injected) return;
        if (other is EdgeCollider2D)
        {
            Target.DamageMembrane(Damage);
            FXManager.Instance.PlayFX("Burst", this.transform.position);
             if (_state != VirusState.FLEEING)
            {
                InjectRNA(other.gameObject);
                injected = true;
                Destroy(gameObject); // virus coat discarded
            }
        }
        else if (other.CompareTag("slicer"))
        {
            other.GetComponent<SlicerEnzyme>().TakeDamage(100);
            FXManager.Instance.PlayFX("burst", this.transform.position);
            Destroy(gameObject);
        }
       
    }

    void InjectRNA(GameObject target)
    {
        target.GetComponentInChildren<Nucleus>().Hijack(this, Children);
        //Instantiate(rnaPrefab, transform.position, Quaternion.identity);
    }
}

public enum VirusState
{
    WAITING,
    ATTACKING,
    FLEEING
}
