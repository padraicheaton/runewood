using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellCaster : MonoBehaviour
{
    public static UnityAction<Vector3, Vector3, SpellItemData> OnItemSpellCastRequested;
    public static UnityAction<Vector3, Vector3, SpellComponentData.Element, List<SpellComponentData.Action>> OnSpellCastRequested;

    [Header("References")]
    [SerializeField] private List<SpellAction> spellActions = new List<SpellAction>();

    private void Awake()
    {
        OnItemSpellCastRequested += CastSpell;
        OnSpellCastRequested += CastSpell;
    }

    private void OnDestroy()
    {
        OnItemSpellCastRequested -= CastSpell;
        OnSpellCastRequested -= CastSpell;
    }

    private void CastSpell(Vector3 position, Vector3 forward, SpellItemData spellItemData)
    {
        CastSpell(position, forward, spellItemData.element, spellItemData.actions);
    }

    private void CastSpell(Vector3 position, Vector3 forward, SpellComponentData.Element element, List<SpellComponentData.Action> actions)
    {
        if (actions.Count == 0)
            return;

        List<SpellComponentData.Action> copyOfActions = new List<SpellComponentData.Action>();
        copyOfActions.AddRange(actions);

        // Get the first action from the list, remove it so the list can be passed onto the casted spell
        SpellComponentData.Action action = copyOfActions[0];
        copyOfActions.RemoveAt(0);

        // Get the prefab for the first action in the list
        SpellAction spellObject = spellActions.Find(spellAction => spellAction.action == action);

        if (spellObject == null || spellObject.prefab == null)
            return;

        // Instantiate the spell object
        GameObject instantiatedSpell = Instantiate(spellObject.prefab, position, Quaternion.identity);
        instantiatedSpell.transform.forward = forward;

        // Get the spell component on the object to set it up with the element and remaining actions (copyOfActions)
        if (instantiatedSpell.TryGetComponent<ISpell>(out ISpell spellComponent))
            spellComponent.Setup(element, copyOfActions);
    }



    // Data containers
    [System.Serializable]
    private class SpellAction
    {
        public SpellComponentData.Action action;
        public GameObject prefab;
    }
}
