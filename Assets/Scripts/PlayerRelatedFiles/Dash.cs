using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu]
public class Dash: Ability
{
    public float dashMultiplier;

    public override void Activate(GameObject player)
    {
        player.GetComponent<PlanarMove>().playerSpeed *= dashMultiplier;
    }

    public override void ActiveEffect(GameObject player)
    {
        player.GetComponent<Jump>().isDashing = true;
        player.GetComponent<Jump>().verticalVelocity.y = 0;
    }

    public override void Cooldown(GameObject player)
    {
        player.GetComponent<Jump>().isDashing = false;
        player.GetComponent<PlanarMove>().playerSpeed /= dashMultiplier;
    }
}
