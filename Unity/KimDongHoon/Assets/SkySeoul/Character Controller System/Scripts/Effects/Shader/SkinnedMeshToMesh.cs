using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SkinnedMeshToMesh : MonoBehaviour
{
    public SkinnedMeshRenderer[] skinnedMeshes;
    public VisualEffect VFXGraph;
    public float refreshRate;

    private void Awake()
    {
        skinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateVFXGraph());
    }

    IEnumerator UpdateVFXGraph()
    {
        while (gameObject.activeSelf)
        {
            foreach(var skinnedMesh in skinnedMeshes)
            {
                Mesh m = new Mesh();
                skinnedMesh.BakeMesh(m);

                Vector3[] vertices = m.vertices;
                Mesh m2 = new Mesh();
                m2.vertices = vertices;

                VFXGraph.SetMesh("Mesh", m2);

                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
