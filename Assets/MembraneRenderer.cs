using UnityEngine;
using System.Collections.Generic;
using UnityEngine.U2D;
using UnityEngine.Rendering;


public class MembraneRenderer : MonoBehaviour
{
   
    private List<VerletPoint> membranePoints;
    public SpriteShapeController spriteShapeController;
    public Texture2D membraneTexture;
    public float textureScale = 1f;
    private bool _ready;

    public Sprite HealthyMembrane;
    public Sprite DamagedMembrane;
    public Sprite CriticalMembrane;
    

    public void BeginRenderingMembrane()
    {
      
        membranePoints = GetComponent<CellController>().GetVerletPoints();
        spriteShapeController = GetComponent<SpriteShapeController>();

        // Assign texture to the SpriteShape
        var material = new Material(Shader.Find("Sprites/Default"));
        material.mainTexture = membraneTexture;
        spriteShapeController.spriteShapeRenderer.sharedMaterial = material;
        _ready = true;
        ReflectHealth(CellController.MAX_MEMBRANE_HEALTH);
    }

    public void ReflectHealth(int currentHealth)
    {
        var profile = spriteShapeController.spriteShape;
        var currentSprite = HealthyMembrane;
        if (currentHealth <= 0)
            currentSprite = null;
        else if (currentHealth < CellController.MAX_MEMBRANE_HEALTH * 0.33f)
            currentSprite = CriticalMembrane;
        else if (currentHealth < CellController.MAX_MEMBRANE_HEALTH * 0.66f)
            currentSprite = DamagedMembrane;

        
        if (profile.angleRanges[0].sprites[0] == currentSprite)
            return;

        foreach (var range in profile.angleRanges)
        {
            
            if (range.sprites.Count > 0)
            {
                range.sprites[0] = currentSprite;
            }
        }


    }

    void LateUpdate()
    {
        if (_ready)
        {
            UpdateShape();
           
            for (int i = 0; i < membranePoints.Count; i++)
            {
                
            }
        }
        
    }

    void UpdateShape()
    {

        if (membranePoints == null || membranePoints.Count < 3) return;

        var spline = spriteShapeController.spline;
        spline.Clear();

        float minDistance = 0.01f; // Adjust as needed to avoid "too close" errors
        Vector2 lastValidPoint = membranePoints[0].position;
        int splineIndex = 0;

        for (int i = 0; i < membranePoints.Count; i++)
        {
            Vector2 point = membranePoints[i].position;

            if (Vector2.Distance(point, lastValidPoint) > minDistance)
            {
                spline.InsertPointAt(splineIndex, point);
                spline.SetTangentMode(splineIndex, ShapeTangentMode.Continuous); // Smooth curves
                lastValidPoint = point;
                splineIndex++;
            }
              
        }
        if (splineIndex > 1 && Vector2.Distance(membranePoints[0].position, lastValidPoint) > minDistance)
        {
            // Close the loop to form a closed shape
            spline.InsertPointAt(splineIndex, membranePoints[0].position);
            spline.SetTangentMode(splineIndex, ShapeTangentMode.Continuous);
        }
        

        // Update the edge collider to match the shape
        UpdateCollider();
    }

    void UpdateCollider()
    {
        var edgeCollider = GetComponent<EdgeCollider2D>();
        Vector2[] colliderPoints = new Vector2[membranePoints.Count + 1];

        for (int i = 0; i < membranePoints.Count; i++)
        {
            colliderPoints[i] = membranePoints[i].position;
        }
        colliderPoints[membranePoints.Count] = membranePoints[0].position; // Close the loop

        edgeCollider.points = colliderPoints;
    }


}
