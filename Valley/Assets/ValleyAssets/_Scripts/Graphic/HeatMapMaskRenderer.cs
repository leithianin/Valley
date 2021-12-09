using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapMaskRenderer : MonoBehaviour
{
    private static List<NoiseEntity> entities;

    public static void RegisterEntities(NoiseEntity entity)
    {
        entities.Add(entity);
    }

    //Properties
    [SerializeField] private ComputeShader computeShader = null;

    [Range(64, 4096)] [SerializeField] private int TextureSize = 2048;
    [SerializeField] private float MapSize = 0;

    [SerializeField] private float BlendDistance = 4f;

    public Color MaskColor0;
    public Color MaskColor1;
    public Color MaskColor2;
    public Color MaskColor3;

    public Texture2D NoiseTexture;
    [Range(0f, 5f)] public float NoiseDetail = 4f;

    private RenderTexture maskTexture;

    //Caching shader properties
    private static readonly int textureSizeId = Shader.PropertyToID("_TextureSize");
    private static readonly int entityCountId = Shader.PropertyToID("_EntityCount");
    private static readonly int mapSizeId = Shader.PropertyToID("_MapSize");
    private static readonly int blendId = Shader.PropertyToID("_Blend");

    private static readonly int color0Id = Shader.PropertyToID("_Color0");
    private static readonly int color1Id = Shader.PropertyToID("_Color1");
    private static readonly int color2Id = Shader.PropertyToID("_Color2");
}
