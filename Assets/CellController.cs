using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class CellController : MonoBehaviour
{
   
    public float springStiffness = 150f;
    public float pressureStrength = 5f;
    public float Radius = 1f;

    private List<VerletPoint> points;
    private List<VerletSpring> springs;
    private Mesh mesh;
    private MembraneRenderer membraneRenderer;
    private Cytoplasm cytoplasm;
    public int MembraneHealth = 100;
    public static int MAX_MEMBRANE_HEALTH = 100;
    public int pointCount = 20;
    public float damping = 0.98f;
   // public float polymerizationForce = 10f;
    public float contractionForce = 5f;
    private PseudopodController pseudopodController;

    public int ATP = 1000;
   
    public float totalProteins = 50f;
    public float totalLipids = 50f;
    public float totalToxins = 0f;
    public Transform membraneTransform;


    private List<CellOrganelle> organelles = new List<CellOrganelle>();
    private float ribosomeCount;
    private float fluidViscosity;

    public delegate void UpdateATP(int newAmount);
    public UpdateATP onUpdateATP;

    public delegate void UpdateGoodie(Goodie g);
    public UpdateGoodie onUpdateGoodie;

    public delegate void UpdateGlucose(int newAmount);
    public UpdateGlucose onUpdateGlucose;

    public delegate void UpdateNA(int newAmount);
    public UpdateNA onUpdateNA;

    public delegate void SignalGameOver(string reason);
    public SignalGameOver onSignalGameOver;

    private Mitochondrion mitochondrion;
    

    void Awake()
    {
        if (MAX_MEMBRANE_HEALTH < MembraneHealth)
            MAX_MEMBRANE_HEALTH = MembraneHealth;
        GameData.ATP = ATP;
        organelles.AddRange(FindObjectsByType<CellOrganelle>(FindObjectsSortMode.None));
        InitializeCell();
        pseudopodController = GetComponent<PseudopodController>();
        if (pseudopodController != null)
        {
            pseudopodController.points = points;
        }
        membraneRenderer = GetComponent<MembraneRenderer>();
        membraneRenderer.BeginRenderingMembrane();
        cytoplasm = GetComponentInChildren<Cytoplasm>();
        cytoplasm.onUpdateATP += delegate (int atp) { onUpdateATP?.Invoke(atp); };
        cytoplasm.onUpdateGlucose += delegate (int g) { onUpdateGlucose?.Invoke(g); };
       
        mitochondrion = GetComponentInChildren<Mitochondrion>();
        mitochondrion.onChangeEnergyGeneration += delegate (int newAmt) { cytoplasm.ProductionAmt = newAmt; };
        //initialArea = CalculateCurrentArea(); // Cache the starting area
    }

    void Start()
    {
        onUpdateATP?.Invoke(GameData.ATP);   
    }

    public void DamageMembrane(int amount)
    {
        MembraneHealth -= amount;
        membraneRenderer.ReflectHealth(MembraneHealth);

        if (MembraneHealth <= 0)
        {
            CollapseOrganelles();
            onSignalGameOver?.Invoke("lysis");
        }
    }

    public void CollapseOrganelles()
    {
        CellOrganelle[] organelles = GetComponentsInChildren<CellOrganelle>();
        for(int i=0; i < organelles.Length; i++)
        {
            organelles[i].TakeDamage(organelles[i].maxHealth);
        }
    }

   

    public void AddATP(int amount)
    {
       GameData.ATP += amount;
    }

    public void AddProteins(float amount)
    {
        totalProteins += amount;
    }

    public void AddLipids(float amount)
    {
        totalLipids += amount;
    }

    public void RemoveToxins(float amount)
    {
        totalToxins = Mathf.Max(0, totalToxins - amount);
    }

    public void StabilizeCell(float factor)
    {
        // Placeholder function for now
    }

    public void InitiateCellDivision()
    {
        Debug.Log("Cell division initiated!");
    }

    /// <summary>
    /// Increases the ribosome count to simulate ribosome production by the nucleolus.
    /// </summary>
    public void AddRibosomes(float amount)
    {
        ribosomeCount += amount;
        Debug.Log($"Ribosome count increased: {ribosomeCount}");
    }

    /// <summary>
    /// Adjusts cytoplasm viscosity to affect cell movement and internal flow dynamics.
    /// </summary>
    public void ModifyFluidViscosity(float newViscosity)
    {
        fluidViscosity = Mathf.Clamp(newViscosity, 0.1f, 2.0f); // Prevent extreme values
        Debug.Log($"Fluid viscosity updated: {fluidViscosity}");
    }

    /// <summary>
    /// Responds to environmental signals received by the ECM, adapting the cell's behavior.
    /// </summary>
    public void ReactToEnvironment(string signalType)
    {
        //lastEnvironmentalSignal = signalType;

        switch (signalType)
        {
            case "NutrientSurge":
                Debug.Log("Cell detects a nutrient surge! Increasing metabolism.");
                // Potentially boost ATP production or movement speed
                break;
            case "ToxinPresence":
                Debug.Log("Toxins detected! Activating defensive responses.");
                // Could trigger lysosome activity to neutralize toxins
                break;
            case "SignalingMolecule":
                Debug.Log("Receiving a chemical signal. Adjusting behavior.");
                // Might trigger changes in cell movement or protein synthesis
                break;
            default:
                Debug.Log("Unknown environmental signal.");
                break;
        }
    }


    void OnMouseUp()
    {
        /*float relaxation = 0.95f;
        foreach (var point in points)
        {
            point.previousPosition = Vector2.Lerp(point.previousPosition, point.position, relaxation);
        }*/
    }

    void InitializeCell()
    {
        points = new List<VerletPoint>();
        springs = new List<VerletSpring>();

        // Create membrane points in a circle
        
        for (int i = 0; i < pointCount; i++)
        {
            float angle = i * Mathf.PI * 2 / pointCount;
            Vector2 pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Radius;
            points.Add(new VerletPoint(pos));
        }

        // Create edge springs
        for (int i = 0; i < pointCount; i++)
        {
            int next = (i + 1) % pointCount;
            springs.Add(new VerletSpring(points[i], points[next], springStiffness));
        }
       

        // Add cross-linking springs for structural integrity
        for (int i = 0; i < pointCount; i++)
        {
            int next = (i + 2) % pointCount;
            springs.Add(new VerletSpring(points[i], points[next], springStiffness * 0.5f));
        }

        // Set up mesh
        //mesh = new Mesh();
        //GetComponent<MeshFilter>().mesh = mesh;
    }

    void FixedUpdate()
    {
        Vector2 force = Vector2.zero;
        foreach (var spring in springs)
            force = spring.ApplyForce();

       // foreach (var organelle in organelles)
         //   organelle.Rb2D.AddForce(force);
        // Apply internal pressure
        ApplyInternalPressure();

        // Update positions using Verlet integration
        foreach (var point in points)
            point.UpdatePosition(Time.fixedDeltaTime);

        // Update mesh for rendering
        //UpdateMesh();

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int distance = (int)Vector2.Distance(targetPos, this.transform.position);
            int cost = distance*10;
            if (GameData.ATP > cost)
            {
                pseudopodController.MakePseudopod(targetPos, distance);
                GameData.ATP -= cost;
                onUpdateATP?.Invoke(GameData.ATP);
            }
        }

    }

    public List<VerletPoint> GetVerletPoints()
    {
        return points;
    }

    void ApplyInternalPressure()
    {
        Vector2 center = GetCellCenter();
        float currentArea = CalculateArea();
        float targetArea = Mathf.PI * 1f * 1f;

        // Scale down the correction force and apply damping
        float pressureForce = (targetArea - currentArea) * pressureStrength * 0.1f; // Apply only 10% of the correction per frame

        //float damping = 0.95f; // Reduces feedback loop tension

        foreach (var point in points)
        {
            Vector2 direction = (point.position - center).normalized;
            point.AddForce(direction * pressureForce);

            // Apply damping to soften the reaction
            point.previousPosition += (point.position - point.previousPosition) * (1 - damping);
        }
        /*foreach (var organelle in organelles)
        {
            Vector2 direction = ((Vector2)organelle.transform.position - center).normalized;
            organelle.Rb2D.AddForce(direction * pressureForce);
        }*/
            
    }

    Vector2 GetCellCenter()
    {
        Vector2 sum = Vector2.zero;
        foreach (var point in points)
            sum += point.position;

        return sum / points.Count;
    }

    float CalculateArea()
    {
        float area = 0;
        for (int i = 0; i < points.Count; i++)
        {
            int next = (i + 1) % points.Count;
            area += points[i].position.x * points[next].position.y;
            area -= points[next].position.x * points[i].position.y;
        }
        return Mathf.Abs(area) * 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("goodie"))
        {
            Goodie g = collision.gameObject.GetComponent<Goodie>();
            switch (g.Type)
            {
                case "g":
                    GameData.Glucose += g.amount;
                    break;
                case "na":
                    GameData.NA += g.amount;
                    break;
                case "aa":
                    GameData.AA += g.amount;
                    break;
            }

            onUpdateGoodie?.Invoke(g);

            collision.gameObject.SetActive(false);
        }
    }

    




}
