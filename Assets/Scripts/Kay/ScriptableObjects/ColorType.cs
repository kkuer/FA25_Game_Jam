using UnityEngine;

[CreateAssetMenu( menuName = "Tag/ColorType")]
public class ColorType : ScriptableObject
{
    public enum Type
    {
        Black,
        White
    }
    public Type type;
}
