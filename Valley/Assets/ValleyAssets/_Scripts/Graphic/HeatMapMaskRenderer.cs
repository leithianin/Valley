using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapMaskRenderer : MonoBehaviour
{
    //Register entities making noise (temporary)
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
    private static readonly int color3Id = Shader.PropertyToID("_Color3");

    private static readonly int noiseTextId = Shader.PropertyToID("_NoiseTex");
    private static readonly int noiseDetailId = Shader.PropertyToID("_NoiseDetail");

    private static readonly int maskTextId = Shader.PropertyToID("_Mask");

    private static readonly int entityBufferId = Shader.PropertyToID("_EntityBuffer");

    //Informations for each entity to parse to the compute
    private struct EntityBufferElement
    {
        public float PositionX;
        public float PositionY;
        public float Range;
        public float Noise;
    }

    private List<EntityBufferElement> bufferElements;
    private ComputeBuffer buffer = null;

    private void Awake()
    {
        //Create new list & add entities in Start()
        entities = new List<NoiseEntity>();

        //Create new render texture for mask
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        maskTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
#else
        maskTexture = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGB32)
#endif
        {
            enableRandomWrite = true
        };
        maskTexture.Create();

        //Set texture dimensions and mask texture in compute
        computeShader.SetInt(textureSizeId, TextureSize);
        computeShader.SetTexture(0, maskTextId, maskTexture);

        //Set blend distance
        computeShader.SetFloat(blendId, BlendDistance);

        //Set mask colors
        computeShader.SetVector(color0Id, MaskColor0);
        computeShader.SetVector(color1Id, MaskColor1);
        computeShader.SetVector(color2Id, MaskColor2);
        computeShader.SetVector(color3Id, MaskColor3);

        //Set noise texture
        computeShader.SetTexture(0, noiseTextId, NoiseTexture);
        computeShader.SetFloat(noiseDetailId, NoiseDetail);

        //Set mask texture and map size
        Shader.SetGlobalTexture(maskTextId, maskTexture);
        Shader.SetGlobalFloat(mapSizeId, MapSize);

        //Create new list to add entities infos later
        bufferElements = new List<EntityBufferElement>();
    }

    private void OnDestroy()
    {
        buffer?.Dispose();

        if (maskTexture != null) DestroyImmediate(maskTexture);
    }

    //Setup all buffers and vars
    private void Update()
    {
        //Recreate buffer
        bufferElements.Clear();

        foreach(NoiseEntity entity in entities)
        {
            EntityBufferElement element = new EntityBufferElement
            {
                PositionX = entity.transform.position.x,
                PositionY = entity.transform.position.z,
                //Range = entity.Range,
                //Noise = entity.Noise
            };

            bufferElements.Add(element);
        }

        buffer?.Release();
        buffer = new ComputeBuffer(bufferElements.Count * 4, sizeof(float));

        //Set buffer data & parse it to the compute
        buffer.SetData(bufferElements);
        computeShader.SetBuffer(0, entityBufferId, buffer);

        //Set other vars
        computeShader.SetInt(entityCountId, bufferElements.Count);

        //Execute compute
        //Thread group is 8x8=64 => Dispatch (TextureSize / 8) * (TextureSize / 8) threads groups
        computeShader.Dispatch(0, Mathf.CeilToInt(TextureSize / 8f), Mathf.CeilToInt(TextureSize / 8f), 1);
    }
}
