using UnityEngine;

namespace PacMan.AI
{
    /// <summary>
    /// Inky - The blue ghost with random/erratic behavior
    /// Behavior: Complex pattern based on player and Blinky positions
    /// </summary>
    public class Inky : Ghost
    {
        [Header("Inky Specific")]
        [SerializeField] private Ghost blinkyReference; // Reference to Blinky for calculation
        [SerializeField] private float randomBehaviorFactor = 0.3f;
        
        protected override void HandleChaseBehavior()
        {
            // Inky's behavior is more complex:
            // 1. Take a point 2 spaces ahead of the player
            // 2. Draw a vector from Blinky to that point
            // 3. Double the vector and target that location
            
            if (playerTransform != null)
            {
                // Get a point 2 spaces ahead of the player
                Vector3 playerDirection = playerTransform.forward;
                Vector3 targetPoint = playerTransform.position + (playerDirection * 2f);
                
                // If we have a reference to Blinky, use the complex algorithm
                if (blinkyReference != null)
                {
                    Vector3 vectorToTarget = targetPoint - blinkyReference.transform.position;
                    Vector3 inkyTarget = blinkyReference.transform.position + (vectorToTarget * 2f);
                    
                    Vector3 directionToTarget = (inkyTarget - transform.position).normalized;
                    _moveDirection = directionToTarget;
                }
                else
                {
                    // Fallback to simpler behavior if no Blinky reference
                    Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                    
                    // Add some randomness to Inky's movement
                    if (Random.value < randomBehaviorFactor)
                    {
                        _moveDirection = GetRandomDirection();
                    }
                    else
                    {
                        _moveDirection = directionToPlayer;
                    }
                }
            }
        }
        
        protected override void HandleScatterBehavior()
        {
            // Inky moves to the bottom-right corner in scatter mode
            Vector3 scatterTarget = new Vector3(100f, transform.position.y, -100f); // Bottom-right corner
            Vector3 directionToScatter = (scatterTarget - transform.position).normalized;
            _moveDirection = directionToScatter;
        }
        
        protected override void HandleFrightenedBehavior()
        {
            // Inky runs away from the player when frightened
            base.HandleFrightenedBehavior();
            
            // Inky might change direction more frequently when frightened
            if (Random.value < randomBehaviorFactor * 2)
            {
                _moveDirection = GetRandomDirection();
            }
        }
        
        protected override void HandleRecoveringBehavior()
        {
            // Inky transitions back to normal behavior
            base.HandleRecoveringBehavior();
        }
        
        protected override void HandleReturningHomeBehavior()
        {
            // Inky returns to the ghost house
            base.HandleReturningHomeBehavior();
        }
        
        public override void SetFrightened()
        {
            base.SetFrightened();
        }
        
        public override void SetRecovering()
        {
            base.SetRecovering();
        }
        
        public override void SetReturningHome()
        {
            base.SetReturningHome();
        }
        
        public override void ResetGhost()
        {
            base.ResetGhost();
        }
    }
}