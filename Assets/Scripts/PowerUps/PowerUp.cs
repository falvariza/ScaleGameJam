using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] protected bool expiresImmediately;
    [SerializeField] protected SpriteRenderer powerUpVisual;
    // public GameObject specialEffect;
    // public AudioClip soundEffect;


    protected enum PowerUpState
    {
        InAttractMode,
        IsCollected,
        IsExpiring
    }

    protected SizeSystem playerSizeSystem;
    protected PowerUpState powerUpState;

    protected virtual void Start()
    {
        powerUpState = PowerUpState.InAttractMode;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        PowerUpCollected(other.gameObject);
    }

    protected virtual void PowerUpCollected(GameObject gameObjectCollectingPowerUp)
    {
        Player player = gameObjectCollectingPowerUp.GetComponent<Player>();

        if(player == null)
        {
            return;
        }

        // We only care if we've not been collected before
        if(powerUpState == PowerUpState.IsCollected || powerUpState == PowerUpState.IsExpiring)
        {
            return;
        }

        powerUpState = PowerUpState.IsCollected;

        // We must have been collected by a player, store handle to player for later use
        playerSizeSystem = player.GetComponent<SizeSystem>();

        // We move the power up game object to be under the player that collect it, this isn't essential for functionality
        // presented so far, but it is neater in the gameObject hierarchy
        gameObject.transform.parent = playerSizeSystem.gameObject.transform;
        gameObject.transform.position = playerSizeSystem.gameObject.transform.position;

        // Collection effects
        PowerUpEffects();

        // Payload
        PowerUpAction();

        // // Send message to any listeners
        // foreach(GameObject go in EventSystemListeners.main.listeners)
        // {
        //     ExecuteEvents.Execute<IPowerUpEvents>(go, null,(x, y) => x.OnPowerUpCollected(this, playerSizeSystem));
        // }

        // Now the power up visuals can go away
        powerUpVisual.enabled = false;
    }

    protected virtual void PowerUpEffects()
    {
        // if(specialEffect != null)
        // {
        //     Instantiate(specialEffect, transform.position, transform.rotation, transform);
        // }

        // if(soundEffect != null)
        // {
        //     MainGameController.main.PlaySound(soundEffect);
        // }
    }

    protected virtual void PowerUpAction()
    {
        Debug.Log("Power Up collected, issuing payload for: " + gameObject.name);

        // If we're instant use we also expire self immediately
        if(expiresImmediately)
        {
            PowerUpHasExpired();
        }
    }

    protected virtual void PowerUpHasExpired()
    {
        if(powerUpState == PowerUpState.IsExpiring)
        {
            return;
        }
        powerUpState = PowerUpState.IsExpiring;

        // // Send message to any listeners
        // foreach(GameObject go in EventSystemListeners.main.listeners)
        // {
        //     ExecuteEvents.Execute<IPowerUpEvents>(go, null,(x, y) => x.OnPowerUpExpired(this, playerSizeSystem));
        // }
        Debug.Log("Power Up has expired, removing after a delay for: " + gameObject.name);
        DestroySelfAfterDelay();
    }

    protected virtual void DestroySelfAfterDelay()
    {
        // Arbitrary delay of some seconds to allow particle, audio is all done
        // TODO could tighten this and inspect the sfx? Hard to know how many, as subclasses could have spawned their own
        Destroy(gameObject, 10f);
    }
}
