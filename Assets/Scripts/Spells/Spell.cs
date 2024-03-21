using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    [Header("Particle Effect References")]
    [SerializeField] private ParticleSystem fireParticleSystem;
    [SerializeField] private ParticleSystem waterParticleSystem;
    [SerializeField] private ParticleSystem earthParticleSystem;
    [SerializeField] private ParticleSystem airParticleSystem;

    [Header("General Spell Settings")]
    [SerializeField] private float baseDamage;

    protected SpellComponentData.Element element;
    protected List<SpellComponentData.Action> actions;

    public void Setup(SpellComponentData.Element element, List<SpellComponentData.Action> actions)
    {
        this.element = element;
        this.actions = actions;

        fireParticleSystem.Stop();
        waterParticleSystem.Stop();
        earthParticleSystem.Stop();
        airParticleSystem.Stop();

        switch (element)
        {
            case SpellComponentData.Element.Fire:
            default:
                fireParticleSystem.Play();
                break;
            case SpellComponentData.Element.Water:
                waterParticleSystem.Play();
                break;
            case SpellComponentData.Element.Earth:
                earthParticleSystem.Play();
                break;
            case SpellComponentData.Element.Air:
                airParticleSystem.Play();
                break;
        }

        OnSetup();
    }

    protected abstract void OnSetup();

    public void Detonate()
    {
        switch (element)
        {
            case SpellComponentData.Element.Fire:
            default:
                FireDetonate();
                break;
            case SpellComponentData.Element.Water:
                WaterDetonate();
                break;
            case SpellComponentData.Element.Earth:
                EarthDetonate();
                break;
            case SpellComponentData.Element.Air:
                AirDetonate();
                break;
        }

        OnDetonationFinish();
    }

    protected abstract void FireDetonate();
    protected abstract void WaterDetonate();
    protected abstract void EarthDetonate();
    protected abstract void AirDetonate();

    protected abstract void OnDetonationFinish();
}
