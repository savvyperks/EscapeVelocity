﻿using UnityEngine;

[ExecuteInEditMode]
public class PlanetDeserts : MonoBehaviour, PlanetInterface {
    [Header("Transform")]
    [Range(0f, 2f)] public float Size = 1.0f;
    [Range(0f, 6.28f)] public float Rotation = 0f;
    [Range(-1f, 1f)] public float Speed = 0.5f;

    [Header("Colors")]
    public Gradient SurfaceColor;
    public Color AtmosphereColor;

    [Header("Seeds")]
    [Range(1, 100)] public int SurfaceSeed = 100;

    [Header("Misc")]
    public Vector2 LightOrigin = new Vector2(0.3f, 0.7f);
    [Range(0f, 256)] public int Pixels = 128;

    private PlanetLayer _Surface;
    private PlanetLayer _Atmosphere;

    private float _Timestamp = 0f;

    private void Awake() {
        Initialize();

        SetSeed(SurfaceSeed);
        SetColors(SurfaceColor, AtmosphereColor);
        SetPixels(Pixels);
        SetSize(Size);
        SetRotate(Rotation);
        SetLight(LightOrigin);
        SetSpeed(Speed);
    }

    public void Initialize() {
        SpriteRenderer surfaceRenderer = transform.Find("Surface").GetComponent<SpriteRenderer>();
        SpriteRenderer atmosphereRenderer = transform.Find("Atmosphere").GetComponent<SpriteRenderer>();

        Material surfaceMaterial = new Material(surfaceRenderer.sharedMaterial);
        Material atmosphereMaterial = new Material(atmosphereRenderer.sharedMaterial);

        _Surface = new PlanetLayer(gameObject, surfaceRenderer, surfaceMaterial);
        _Atmosphere = new PlanetLayer(gameObject, atmosphereRenderer, atmosphereMaterial);
    }

    public void SetSeed(int seed) {
        _Surface.SetMaterialProperty(ShaderProperties.Seed, seed);

        SurfaceSeed = seed;
    }

    public void SetPixels(float ppu) {
        _Surface.SetMaterialProperty(ShaderProperties.Pixels, ppu);
        _Atmosphere.SetMaterialProperty(ShaderProperties.Pixels, ppu);

        Pixels = (int)ppu;
    }

    public void SetLight(Vector2 position) {
        _Surface.SetMaterialProperty(ShaderProperties.LightOrigin, position);
        _Atmosphere.SetMaterialProperty(ShaderProperties.LightOrigin, position);

        LightOrigin = position;
    }

    public void SetRotate(float rotation) {
        _Surface.SetMaterialProperty(ShaderProperties.Rotation, rotation);

        Rotation = rotation;
    }

    public void SetSize(float size) {
        transform.localScale = new Vector3(size, size, transform.localScale.z);

        // Scale for pixel size, without tampering "pixels" property
        _Surface.SetMaterialProperty(ShaderProperties.Pixels, size * Pixels);
        _Atmosphere.SetMaterialProperty(ShaderProperties.Pixels, size * Pixels);

        Size = size;
    }

    public void SetSpeed(float speed) {
        _Surface.SetMaterialProperty(ShaderProperties.Speed, Speed);

        speed = Speed;
    }

    public void SetColors(Gradient surfaceColors, Color atmosphereColor) {
        _Surface.SetMaterialProperty(ShaderProperties.GradientTex, PlanetUtil.GenTexture(surfaceColors));
        _Atmosphere.SetMaterialProperty(ShaderProperties.Color, atmosphereColor);

        AtmosphereColor = atmosphereColor;
        SurfaceColor = surfaceColors;
    }

    public void UpdateTime(float time) {
        _Surface.SetMaterialProperty(ShaderProperties.Timestamp, time);
    }

    private void Update() {
        if (Application.IsPlaying(gameObject)) {
            _Timestamp += Time.deltaTime;
            UpdateTime(_Timestamp);
        }
    }
}
