using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu]
public class Dash: Ability
{
    public float dashMultiplier;

    public override void Activate(GameObject player)
    {
        player.GetComponent<PlayerMovement>().playerSpeed *= dashMultiplier;
    }

    public override void ActiveEffect(GameObject player)
    {
        player.GetComponent<PlayerMovement>().isDashing = true;
        player.GetComponent<PlayerMovement>().verticalVelocity.y = 0;
    }

    public override void Cooldown(GameObject player)
    {
        player.GetComponent<PlayerMovement>().isDashing = false;
        player.GetComponent<PlayerMovement>().playerSpeed /= dashMultiplier;
    }
}
