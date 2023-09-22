
using UnityEditor;

[CustomEditor(typeof(AllowSpriteShadows))]
public class AllowSpriteShadowEditor : Editor
{
    //has the sprite render been modified or not?
    private bool modified = false;


    // I would rather have this just do it once and be over with it
    // rn it's doing this everytime the sprite is selected
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AllowSpriteShadows spriteShadows = target as AllowSpriteShadows;

        if (!modified)
        {
            spriteShadows.SpriteShadows();
            modified = true;
        }
    }

}
