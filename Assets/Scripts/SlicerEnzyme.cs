using System.Security.Cryptography;
using UnityEngine;
using DG.Tweening;


// SlicerEnzyme.cs
public class SlicerEnzyme : CellOrganelle
{
    public float sliceCooldown = 3f;
    private float timer;

    private bool _chasing;
    private InjectorVirus _target;


    protected override void Start()
    {
        base.Start();
        timer = sliceCooldown * Random.value;
        transform.DOBlendableLocalMoveBy(Vector3.one * Random.Range(-5, 5), 2);
    }

    void Update()
    {
        if (_chasing)
        {
            if (_target == null)
            {
                _chasing = false;
            }
            else
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, _target.Position, 0.04f);
            }
           
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                SliceNearbyRNA();
                timer = sliceCooldown;
            }
        }
    }

    void SliceNearbyRNA()
    {
        var rnas = FindObjectsByType<InjectorVirus>(FindObjectsSortMode.None);
        foreach (var rna in rnas)
        {
            if (Vector2.Distance(transform.position, rna.Position) < 1f)
            {
                _target = rna;
                _chasing = true;
                //
                //Debug.Log("Slicer enzyme destroyed RNA.");
                break;
            }
        }
    }
}
