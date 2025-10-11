using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [Header("Particle Prefabs")]
    public GameObject slashParticles;
    public GameObject hitParticles;

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

    public void spawnVFX(GameObject particles, Vector3 pos, Quaternion rot)
    {
        GameObject newParticles = Instantiate(particles, pos, rot);
        ParticleSystem ps = newParticles.GetComponent<ParticleSystem>();

        ps.Play();
    }

    public void playVFX(ParticleSystem ps)
    {
        ps.Play();
    }

    public void playHit(Vector3 pos)
    {
        spawnVFX(hitParticles, pos, Quaternion.Euler(new Vector3(0, 0, 0)));
    }

    public void playSlash(ParticleSystem slashFX)
    {
        playVFX(slashFX);
    }
}
