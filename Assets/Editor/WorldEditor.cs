using UnityEditor;

[CustomEditor(typeof(WorldGen))]
public class WorldEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WorldGen world = target as WorldGen;

        world.GenerateWorld();
    }

}