using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpell
{
    public void Setup(SpellComponentData.Element element, List<SpellComponentData.Action> actions);

    public void Detonate();
}
