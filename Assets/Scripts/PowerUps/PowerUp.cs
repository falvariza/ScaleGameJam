using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] protected bool expiresImmediately;
    [SerializeField] protected SpriteRenderer powerUpVisual;
    [SerializeField] protected bool shouldDestroyAfterTimeout;
    [SerializeField] protected float destroyTimeout;
    [SerializeField] protected float powerUpDuration;

    // public GameObject specialEffect;
    // public AudioClip soundEffect;

    protected Player player;

    protected enum PowerUpState
    {
        InAttractMode,
        IsCollected,
        IsActive,
        IsExpiring
    }

    protected PowerUpState powerUpState;

    protected virtual void Start()
    {
        powerUpState = PowerUpState.InAttractMode;
    }

    protected virtual void Update()
    {
        switch (powerUpState)
        {
            case PowerUpState.InAttractMode:
                HandleInAttractMode();
                break;
            case PowerUpState.IsActive:
                powerUpDuration -= Time.deltaTime;
                if(powerUpDuration <= 0f)
                {
                    PowerUpHasExpired();
                }
                break;
        }
    }

    protected virtual void HandleInAttractMode()
    {
        if(!shouldDestroyAfterTimeout || powerUpState != PowerUpState.InAttractMode)
        {
            return;
        }

        destroyTimeout -= Time.deltaTime;

        if(destroyTimeout <= 0f && player == null)
        {
            powerUpVisual.enabled = false;
            DestroySelfAfterDelay();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        PowerUpCollected(other.gameObject);
    }

    protected virtual void PowerUpCollected(GameObject gameObjectCollectingPowerUp)
    {
        // We only care if we've not been collected before
        if(powerUpState == PowerUpState.IsCollected || powerUpState == PowerUpState.IsExpiring)
        {
            return;
        }

        Player collectingPlayer = gameObjectCollectingPowerUp.GetComponent<Player>();

        if(collectingPlayer == null)
        {
            return;
        }
        player = collectingPlayer;

        powerUpState = PowerUpState.IsCollected;

        // We move the power up game object to be under the player that collect it, this isn't essential for functionality
        // presented so far, but it is neater in the gameObject hierarchy
        gameObject.transform.parent = player.gameObject.transform;
        gameObject.transform.position = player.gameObject.transform.position;

        // Collection effects
        PowerUpEffects();

        powerUpVisual.enabled = false;
        // Payload
        PowerUpAction();

        // // Send message to any listeners
        // foreach(GameObject go in EventSystemListeners.main.listeners)
        // {
        //     ExecuteEvents.Execute<IPowerUpEvents>(go, null,(x, y) => x.OnPowerUpCollected(this, playerSizeSystem));
        // }
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
        if(expiresImmediately)
        {
            PowerUpHasExpired();
        }

        if (powerUpDuration > 0f)
        {
            powerUpState = PowerUpState.IsActive;
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
        DestroySelfAfterDelay();
    }

    protected virtual void DestroySelfAfterDelay()
    {
        // Arbitrary delay of some seconds to allow particle, audio is all done
        // TODO could tighten this and inspect the sfx? Hard to know how many, as subclasses could have spawned their own
        Destroy(gameObject);
    }

    public float GetPowerUpDuration()
    {
        return powerUpDuration;
    }
}
