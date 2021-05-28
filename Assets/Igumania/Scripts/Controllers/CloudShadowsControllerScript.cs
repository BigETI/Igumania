using UnityEngine;

// TODO: Fix poor API design without breaking references

public class CloudShadowsControllerScript : MonoBehaviour
{
    [Header("Shaders List")]
    public ShadowsObjectScript ShadowsPreset;

    void Start()
    {
        UpdateShadows();
    }

    void UpdateShadows()
    {
        Shader.SetGlobalTexture("CS_Clouds1", ShadowsPreset.CS_Clouds1);
        Shader.SetGlobalTexture("CS_Clouds2", ShadowsPreset.CS_Clouds2);

        Shader.SetGlobalFloat("CS_BaseSpeed", ShadowsPreset.CS_BaseSpeed);
        Shader.SetGlobalVector("CS_Speed1", ShadowsPreset.CS_Speed1);
        Shader.SetGlobalVector("CS_Speed2", ShadowsPreset.CS_Speed2);

        Shader.SetGlobalFloat("CS_Tiling1", ShadowsPreset.CS_Tiling1);
        Shader.SetGlobalFloat("CS_Tiling2", ShadowsPreset.CS_Tiling2);

        Shader.SetGlobalFloat("CS_CloudsPower", ShadowsPreset.CS_CloudsPower);
        Shader.SetGlobalFloat("CS_CloudsMultiply", ShadowsPreset.CS_CloudsMultiply);
        Shader.SetGlobalFloat("CS_CloudsTransparency", ShadowsPreset.CS_CloudsTransparency);
    }
}
