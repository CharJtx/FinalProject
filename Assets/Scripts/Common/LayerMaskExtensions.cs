using UnityEngine;

public static class LayerMaskExtensions
{
    public static bool IsLayerInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }
}