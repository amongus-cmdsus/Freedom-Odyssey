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
        player.GetComponent<PlanarMove>().isDashing = true;
        player.GetComponent<PlanarMove>().verticalVelocity.y = 0;
    }

    public override void Cooldown(GameObject player)
    {
        player.GetComponent<PlanarMove>().isDashing = false;
        player.GetComponent<PlanarMove>().playerSpeed /= dashMultiplier;
    }
}
