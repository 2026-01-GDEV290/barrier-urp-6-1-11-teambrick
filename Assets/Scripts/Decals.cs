using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;

public class Decals : MonoBehaviour
{
    IObjectPool<DecalProjector> decalPool;

    public Material decalMaterial;
    public LayerMask decalLayers = -1;

    public Vector3 decalSize = new Vector3(0.5f, 0.5f, 0.5f);
    public float decalLifetime = 5f;

    public float fadeDuration = 5f;
    Camera cam;

    
    void Start()
    {
        cam = Camera.main;
        decalPool = new ObjectPool<DecalProjector>(
            createFunc: () =>
            {
                GameObject go = new GameObject("DecalProjector");
                DecalProjector dp = go.AddComponent<DecalProjector>();
                dp.material = null;
                dp.fadeFactor = 1f;
                dp.fadeScale = 0.95f;
                return dp;
            }, 
            actionOnGet: dp => dp.gameObject.SetActive(true),
            actionOnRelease: dp => dp.gameObject.SetActive(false),
            actionOnDestroy: dp => Destroy(dp.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );
    }

    public void SpawnDecal(Vector3 hitPoint, Vector3 hitNormal, float separation = 1f, Material decalMaterialOverride = null)
    {
        Debug.Log("Spawning decal at point: " + hitPoint + " with normal: " + hitNormal + " and separation: " + separation);
        DecalProjector decal = decalPool.Get();

        decal.transform.position = hitPoint + hitNormal * separation; // *0.25f;  // *0.01f;
        decal.material = decalMaterialOverride ?? decalMaterial;

        Quaternion normalRotation = Quaternion.LookRotation(-hitNormal, Vector3.up);
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        decal.transform.rotation = normalRotation * randomRotation;
        decal.size = decalSize;


        StartCoroutine(FadeAndReleaseDecal(decal, fadeDuration));
    }

    public void SpawnDecal(Vector3 hitPoint, Vector3 hitNormal, Transform parentObject, float separation = 1f, Material decalMaterialOverride = null)
    {
        Debug.Log("Spawning decal at point: " + hitPoint + " with normal: " + hitNormal + " and separation: " + separation);
        DecalProjector decal = decalPool.Get();

        decal.transform.position = hitPoint + hitNormal * separation;
        decal.material = decalMaterialOverride ?? decalMaterial;

        Quaternion normalRotation = Quaternion.LookRotation(-hitNormal, Vector3.up);
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        decal.transform.rotation = normalRotation * randomRotation;
        decal.size = decalSize;

        // Attach decal to the hit object
        if (parentObject != null)
        {
            decal.transform.SetParent(parentObject);
        }

        StartCoroutine(FadeAndReleaseDecal(decal, fadeDuration));
    }

    public void SpawnDecal(RaycastHit hit, Material decalMaterialOverride = null)
    {
        DecalProjector decal = decalPool.Get();
        decal.transform.position = hit.point + hit.normal * 0.01f; //0.25f;  //0.01f;
        decal.material = decalMaterialOverride ?? decalMaterial;
        Quaternion normalRotation = Quaternion.LookRotation(-hit.normal, Vector3.up);
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        decal.transform.rotation = normalRotation * randomRotation;
        decal.size = decalSize;


        StartCoroutine(FadeAndReleaseDecal(decal, fadeDuration));
    }

    IEnumerator FadeAndReleaseDecal(DecalProjector decal, float duration)
    {
        float time = 0f;
        float initialFade = decal.fadeFactor;
        while (time < duration)
        {
            if (decal == null) yield break;

            time += Time.deltaTime;
            float t = time / duration;
            decal.fadeFactor = Mathf.Lerp(initialFade, 0f, t);
            yield return null;
        }
        if (decal != null)
        {
            decal.fadeFactor = initialFade;
            decalPool.Release(decal);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
