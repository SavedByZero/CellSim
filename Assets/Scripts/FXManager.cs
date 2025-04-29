using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance { get; private set; }

    [System.Serializable]
    public class FXPrefabEntry
    {
        public string key;
        public GameObject prefab;
    }

    [Header("FX Prefabs")]
    public FXPrefabEntry[] fxPrefabs;

    private Dictionary<string, Queue<GameObject>> fxPool = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> fxPrefabLookup = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var entry in fxPrefabs)
        {
            fxPrefabLookup[entry.key] = entry.prefab;
            fxPool[entry.key] = new Queue<GameObject>();
        }
    }

    public void PlayFX(string key, Vector3 position)
    {
        if (!fxPrefabLookup.ContainsKey(key))
        {
            Debug.LogWarning($"FXManager: No FX prefab found for key '{key}'");
            return;
        }

        GameObject fxObject;
        if (fxPool[key].Count > 0)
        {
            fxObject = fxPool[key].Dequeue();
            fxObject.SetActive(true);
        }
        else
        {
            fxObject = Instantiate(fxPrefabLookup[key]);
        }

        fxObject.transform.position = position;
        fxObject.transform.SetParent(this.transform);

        var animator = fxObject.GetComponent<SpriteAnimator>();
        if (animator != null)
        {
       
            StartCoroutine(ReturnToPoolAfter(animator.Sprites.Length * animator.frameRate, key, fxObject));
        }
        else
        {
            Debug.LogError("No animator on FX! " + fxObject);
        }
    }

    private System.Collections.IEnumerator ReturnToPoolAfter(float seconds, string key, GameObject fxObject)
    {
        yield return new WaitForSeconds(seconds);
        fxObject.SetActive(false);
        fxPool[key].Enqueue(fxObject);
    }
}
