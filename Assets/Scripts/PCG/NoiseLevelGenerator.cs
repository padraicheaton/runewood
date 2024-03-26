using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseLevelGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject groundObject;
    [SerializeField] private GameObject edgeGroundObject;

    [Header("Settings")]
    [SerializeField] private float squareWidth;
    [SerializeField] private float heightMultiplier;
    [SerializeField] private float scale;
    [SerializeField] private float zeroPlateauRadius;

    [ContextMenu("Generate New Level")]
    public void GenerateLevel()
    {
        float offset = Random.Range(0f, 99999f);

        // Delete any children of itself
        for (int i = this.transform.childCount; i > 0; --i)
            DestroyImmediate(this.transform.GetChild(0).gameObject);

        List<Vector3> groundPoints = new List<Vector3>();

        for (float x = -squareWidth / 2; x < squareWidth / 2; x++)
        {
            for (float z = -squareWidth / 2; z < squareWidth / 2; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                position.y = GetHeightAtPoint(position, offset);

                position.y = Mathf.Round(position.y);

                groundPoints.Add(position);
            }
        }

        foreach (Vector3 point in groundPoints)
        {
            // Check for neighbours
            bool isNotEdge = groundPoints.Contains(point + Vector3.forward) && groundPoints.Contains(point + Vector3.back) && groundPoints.Contains(point + Vector3.left) && groundPoints.Contains(point + Vector3.right);

            GameObject obj = Instantiate(isNotEdge ? groundObject : edgeGroundObject, point, Quaternion.identity);

            obj.transform.SetParent(transform);
        }
    }

    private float GetHeightAtPoint(Vector3 point, float offset)
    {
        float dist = Vector3.Distance(Vector3.zero, point);

        float multiplier = 1f;

        if (dist < zeroPlateauRadius)
            multiplier = Mathf.Lerp(0f, 1f, dist / zeroPlateauRadius);

        return Mathf.PerlinNoise(point.x * scale + offset, point.z * scale + offset) * heightMultiplier * multiplier;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(squareWidth, 1f, squareWidth));
    }
}
