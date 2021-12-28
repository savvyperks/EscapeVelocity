using UnityEditor;

[CustomEditor(typeof(PlanetBlackHole))]
public class PlanetBlackHoleEditor : Editor {
    private PlanetBlackHole Planet;

    protected virtual void OnEnable() {
        Planet = (PlanetBlackHole)target;
        Planet.Initialize();
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        Planet.SetSeed();
        Planet.SetColors();
        Planet.SetPixels(Planet.Pixels);
        Planet.SetSize(Planet.Size);
        Planet.SetRotate(Planet.Rotation);
        Planet.SetSpeed();
    }
}
