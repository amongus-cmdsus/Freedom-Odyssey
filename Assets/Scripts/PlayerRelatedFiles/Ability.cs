using UnityEngine;

public class Ability : ScriptableObject
{
    public new string name;
    public float cooldown;
    public float activeTime;

    public virtual void Activate(GameObject player)
    {

    }

    public virtual void ActiveEffect(GameObject player)
    {

    }

    public virtual void Cooldown(GameObject player)
    {

    }
}
