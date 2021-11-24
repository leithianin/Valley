using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHeatmapDatas : MonoBehaviour
{
    public Vector4[] positions;
    public Vector4[] properties;

    public Material material;
    public Texture2D text;

    public int count = 5;

    public Vector2 mapLength;
    public float squareLength;

    private float[] soundTab;

    private float wantedDist = 3;

    private Vector2Int DataTabSize => new Vector2Int(Mathf.RoundToInt(mapLength.x / squareLength), Mathf.RoundToInt(mapLength.y / squareLength));

    private int DataSize => DataTabSize.x * DataTabSize.y;

    void Start()
    {
        positions = new Vector4[count];
        properties = new Vector4[count];

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = new Vector4(Random.Range(-mapLength.x/2, mapLength.x/2), Random.Range(-mapLength.y / 2, mapLength.y / 2), 0, 0);
            properties[i] = new Vector4(Random.Range(0f, 0.25f), Random.Range(-0.25f, 1f), 0, 0);
        }

        soundTab = new float[DataSize];

        StartCoroutine(DatasUpdates());
    }

    /*void Update()
    {
        for (int i = 0; i < positions.Length; i++)
            positions[i] += new Vector4(Random.Range(-0.1f, +0.1f), Random.Range(-0.1f, +0.1f), 0, 0) * Time.deltaTime;

        material.SetInt("_Points_Length", count);
        material.SetVectorArray("_Points", positions);
        material.SetVectorArray("_Properties", properties);

        Color[] testColor = new Color[DataTabSize.x * DataTabSize.y];

        for(int i = 0; i < soundTab.Length; i++)
        {
            Vector2Int index = new Vector2Int(i % DataTabSize.x, i / DataTabSize.y);

            float score = 0;
            for (int j = 0; j < count; j++)
            {
                float distance = Vector2.Distance(positions[j], GetWorldFromIndex(index));
                distance = Mathf.Clamp(wantedDist - distance, 0, wantedDist);
                score += distance*2/count;
            }

            score = Mathf.Clamp(score, 0, 10);
            testColor[i] = new Color(0, score / wantedDist, 0);

            text.SetPixel(index.x, index.y, new Color(0, score / wantedDist, 0));
            text.Apply();
        }
    }*/

    IEnumerator DatasUpdates()
    {
        int i = 0;
        while (i < soundTab.Length)
        {
            Debug.Log("T : " + i);
            yield return new WaitForFixedUpdate();
            int l = 0;
            for (l = i; l < i + (DataSize / 200); l++)
            {
                Vector2Int index = new Vector2Int(l % DataTabSize.x, l / DataTabSize.y);

                float score = 0;
                for (int j = 0; j < count; j++)
                {
                    float distance = Vector2.Distance(positions[j], GetWorldFromIndex(index));
                    distance = Mathf.Clamp(wantedDist - distance, 0, wantedDist);
                    score += distance * 2 / count;
                }

                score = Mathf.Clamp(score, 0, 10);
            }
            i = l;
        }
        StartCoroutine(DatasUpdates());
    }

    private Vector2Int GetIndexFromWorld(Vector2 worldPos)
    {
        int x = Mathf.RoundToInt((worldPos.x + mapLength.x / 2) / squareLength);
        int y = Mathf.RoundToInt((worldPos.y + mapLength.y / 2) / squareLength);
        return new Vector2Int(x, y);
    }

    private Vector2 GetWorldFromIndex(Vector2Int index)
    {
        float x = squareLength * index.x - mapLength.x / 2;
        float y = squareLength * index.y - mapLength.y / 2;
        return new Vector2(x, y);
    }
}