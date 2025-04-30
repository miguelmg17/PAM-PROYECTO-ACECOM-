using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease,
    }

    public ItemType type;
    public List<string> allowedTags = new List<string> { "Player", "Bot" };

    private Score scoreManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<Score>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (allowedTags.Contains(other.tag))
        {
            OnItemPickup(other.gameObject);
        }
    }

    private void OnItemPickup(GameObject target)
    {
        switch (type)
        {
            case ItemType.ExtraBomb:
                BombController bombController = target.GetComponent<BombController>();
                if (bombController != null)
                {
                    bombController.AddBomb();
                }

                if (scoreManager != null)
                {
                    scoreManager.scoreUp(1f);
                }
                break;

            case ItemType.BlastRadius:
                BombController bombControllerRadius = target.GetComponent<BombController>();
                if (bombControllerRadius != null)
                {
                    bombControllerRadius.explosionRadius++;
                }
                break;

            case ItemType.SpeedIncrease:
                
                PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.moveSpeed += 0.5f; 
                }
                break;
        }

        Destroy(gameObject);
    }
}
