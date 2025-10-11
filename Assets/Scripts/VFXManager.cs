using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager instance {  get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
