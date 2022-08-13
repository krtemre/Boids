using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public enum GizmoType { Never, SelectedOnly, Always }

    public Boid boidPrefab;
    public float spawnRadius = 10f;
    public int spawnCount = 100;
    public Color color;
    public GizmoType showSpawnRegion;

    private void Awake()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            Boid boid = Instantiate(boidPrefab);
            boid.transform.position = pos;
            boid.transform.forward = Random.insideUnitSphere;
            boid.SetColor(color);
        }
    }

    private void OnDrawGizmos()
    {
        if (showSpawnRegion == GizmoType.Always)
        {
            DrawGizmos();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (showSpawnRegion == GizmoType.SelectedOnly)
        {
            DrawGizmos();
        }
    }

    private void DrawGizmos()
    {
        Gizmos.color = new Color(color.r, color.g, color.b, 0.35f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }

    IEnumerator WaitBoid()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            Boid boid = Instantiate(boidPrefab);
            boid.transform.position = pos;
            boid.transform.forward = Random.insideUnitSphere;
            boid.SetColor(color);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
