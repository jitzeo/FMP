using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class GuidingParticles : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> particles = new List<ParticleSystem>();
    [SerializeField] DialogueRunner dialogueRunner;

    private ParticleSystem activeParticleSystem;
    private int activeParticleIndex;
    void Start()
    {
        activeParticleSystem = particles[activeParticleIndex];
        activeParticleSystem.Play();
        dialogueRunner.AddCommandHandler<int>("set_guide", SetActiveGuidingParticles);
    }

    public void SetActiveGuidingParticles(int particleIndex)
    {
        activeParticleSystem.Stop();
        activeParticleIndex = particleIndex;
        activeParticleSystem = particles[activeParticleIndex];
        activeParticleSystem.Play();
    }
}
