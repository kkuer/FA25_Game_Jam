using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [Header("Particle Prefabs")]
    public GameObject slashParticles;
    public GameObject hitParticles;
    public GameObject hitParticlesRed;
    public GameObject sparkParticles;
    public GameObject deathParticles;
    public GameObject stunParticles;

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
    }

    public void spawnLimitedVFX(GameObject particles, Vector3 pos, float duration)
    {
        ParticleSystem ps = particles.GetComponent<ParticleSystem>();

        var mainModule = ps.main;
        mainModule.duration = duration;

        ps.Play();
    }

    public void playVFX(ParticleSystem ps, float startRot)
    {
        var mainModule = ps.main;
        mainModule.startRotationZ = startRot;

        ps.Play();
    }

    public void playHit(Vector3 pos, bool redVersion)
    {
        if (redVersion)
        {
            spawnVFX(hitParticlesRed, pos, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
        else
        {
            spawnVFX(hitParticles, pos, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }

    public void playSparks(Vector3 pos)
    {
        spawnVFX(sparkParticles, pos, Quaternion.Euler(new Vector3(0, 0, 0)));
    }

    public void playSlash(ParticleSystem slashFX, float angle)
    {
        playVFX(slashFX, angle);
    }

    public void playDeath(Vector3 pos)
    {
        spawnVFX(deathParticles, pos, Quaternion.Euler(new Vector3(0, 0, 0)));
    }

    public void playStun(Vector3 pos, float duration)
    {
        spawnLimitedVFX(stunParticles, pos, duration);
    }
}
