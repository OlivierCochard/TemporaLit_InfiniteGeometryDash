using UnityEngine;

public class PlayerParticleVelocity : MonoBehaviour
{
    public ParticleSystem ps;
    public Rigidbody2D rb;

    private ParticleSystem.VelocityOverLifetimeModule vel;

    void Awake(){
        vel = ps.velocityOverLifetime;
    }

    void Update(){
        vel.y = new ParticleSystem.MinMaxCurve(-rb.linearVelocity.y/3);
    }
}