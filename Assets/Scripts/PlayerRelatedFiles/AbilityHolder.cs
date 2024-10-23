using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability;
    private float cooldownTime;
    private float activeTime;

    private enum AbilityState
    {
        ready,
        active, 
        cooldown
    }

    AbilityState state = AbilityState.ready;

    public KeyCode key;

    void Update()
    {
        switch (state)
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key))
                {
                    ability.Activate(gameObject);
                    state = AbilityState.active;
                    activeTime = ability.activeTime;
                }

                break;
            case AbilityState.active:
                if (activeTime > 0)
                {
                    ability.ActiveEffect(gameObject);
                    activeTime -= Time.deltaTime;
                }
                else
                {
                    ability.Cooldown(gameObject);
                    state = AbilityState.cooldown;
                    cooldownTime = ability.cooldown;
                }

                break;
            case AbilityState.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.ready;
                }

                break;
        }
    }
}
