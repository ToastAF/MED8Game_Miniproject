using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum ColorBlindMode { Deuteranopia, Tritanopia }

[System.Serializable, VolumeComponentMenu("Custom/Color Blind Filter")]
public class ColorBlindVolume : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enabled = new BoolParameter(false);
    public EnumParameter<ColorBlindMode> mode = new EnumParameter<ColorBlindMode>(ColorBlindMode.Deuteranopia);

    public bool IsActive() => enabled.value;
    public bool IsTileCompatible() => false;
}