using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] private Transform seeThroTarget;
    [SerializeField] private LayerMask seeThroLayer;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private List<RaycastHit> lastHitObjects = new();
    void Update()
    {
        Vector3 offset = seeThroTarget.position - transform.position;
        List<RaycastHit> hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, seeThroLayer).ToList();

        if (lastHitObjects.Count > 0)
        {
            foreach (RaycastHit hit in lastHitObjects.Except(hitObjects))
            {
                if (hitObjects.Exists(x => x.transform == hit.transform)) continue;
                
                Material[] materials = hit.transform.GetComponent<Renderer>().materials;
                foreach (Material material in materials)
                {
                    material.SetFloat("_CutoutSize", 0f);
                }
            }
        }

        if (hitObjects.Count <= 0) return;

        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(seeThroTarget.position);
        cutoutPos.y /= (Screen.width / Screen.height);
        foreach (RaycastHit hit in hitObjects)
        {
            //if (lastHitObjects.Exists(x => x.transform == hit.transform)) continue;

            Material[] materials = hit.transform.GetComponent<Renderer>().materials;
            foreach (Material material in materials)
            {
                material.SetVector("_CutoutPos", cutoutPos);
                material.SetFloat("_CutoutSize", .06f);
            }
        }

        lastHitObjects = hitObjects;
    }
}
