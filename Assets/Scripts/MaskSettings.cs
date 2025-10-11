using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class MaskSettings : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material mat = new Material(base.materialForRendering);
            mat.SetFloat("_StencilComp", (float)CompareFunction.NotEqual);
            return mat;
        }
    }
}