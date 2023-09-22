using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowSpriteShadows : MonoBehaviour
{

    private void Start()
    {
        SpriteShadows();
    }

    public void SpriteShadows()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
        {
            spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            spriteRenderer.receiveShadows = true;

            // I want make it so i can just automatically add the "Add sprite shadow" material but idk how rn -_-
            //spriteRenderer.material = 
        }
    }
}
