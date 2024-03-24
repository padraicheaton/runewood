using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseLevelGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject groundObject;

    [Header("Settings")]
    [SerializeField] private float squareWidth;
    [SerializeField] private float heightMultiplier;
    [SerializeField] private float scale;

    [ContextMenu("Generate New Level")]
    public void GenerateLevel()
    {
        float offset = Random.Range(0f, 99999f);

        // Delete any children of itself
        for (int i = this.transform.childCount; i > 0; --i)
            DestroyImmediate(this.transform.GetChild(0).gameObject);

        for (float x = -squareWidth / 2; x < squareWidth / 2; x++)
        {
            for (float z = -squareWidth / 2; z < squareWidth / 2; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                position.y = GetHeightAtPoint(position, offset);

                position.y = Mathf.Round(position.y);

                GameObject obj = Instantiate(groundObject, position, Quaternion.identity);

                obj.transform.SetParent(transform);
            }
        }
    }

    private float GetHeightAtPoint(Vector3 point, float offset)
    {
        return Mathf.PerlinNoise(point.x * scale + offset, point.z * scale + offset);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(squareWidth, 1f, squareWidth));
    }
}
