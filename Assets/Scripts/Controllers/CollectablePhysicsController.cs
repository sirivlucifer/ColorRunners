﻿using System.Threading.Tasks;
using Enums;
using Keys;
using Managers;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class CollectablePhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        #endregion

        #region Serialized Variables

        [SerializeField] private CollectableManager collectableManager;

        #endregion

        #region Private Variables
        
        #endregion

        #endregion
        private void OnTriggerEnter(Collider other)
        {
            if (CompareTag("Collected") && other.CompareTag("Collectable"))
            {
                CollectableManager otherColManager = other.transform.parent.GetComponent<CollectableManager>();

                if (collectableManager.ColorType == otherColManager.ColorType)
                {
                    StackSignals.Instance.onIncreaseStack?.Invoke(other.transform.parent.gameObject);
                    otherColManager.SetAnim(CollectableAnimationStates.Running);
                    other.tag = "Collected";
                }
                else
                {
                    other.transform.parent.gameObject.SetActive(false);
                    StackSignals.Instance.onDecreaseStack?.Invoke(new ObstacleCollisionGOParams()
                    { 
                       Collected = gameObject,
                    });
                }
            }
            if (other.CompareTag("Obstacle"))
            {
                other.gameObject.SetActive(false);
                StackSignals.Instance.onDecreaseStack?.Invoke(new ObstacleCollisionGOParams()
                {
                    Collected = gameObject,
                    Obstacle = other.gameObject,
                });
                StackSignals.Instance.onCheckStack?.Invoke();
            }
            if (other.CompareTag("TurretColorArea"))
            {
                collectableManager.SetAnim(CollectableAnimationStates.CrouchWalking);
            }
            if (other.CompareTag("DroneArea"))
            {
                collectableManager.DelistFromStack();
            }
            if (other.CompareTag("DroneColorArea")&& CompareTag("Collected"))
            {
                collectableManager.SetCollectablePositionOnDroneArea(other.gameObject.transform);
                var droneAreaColorController = other.GetComponent<DroneAreaColorController>();
                
                if (collectableManager.ColorType == droneAreaColorController.ColorType)
                {
                    collectableManager.MatchType = CollectableMatchType.Match;
                }
                else
                {
                    collectableManager.MatchType = CollectableMatchType.UnMatch;
                }
                tag = "Collectable";
            }

            if (other.CompareTag("AfterGround"))
            {
                if (collectableManager.MatchType != CollectableMatchType.Match)
                {
                    collectableManager.SetAnim(CollectableAnimationStates.Dead);
                    collectableManager.SetActiveFalse();
                    ParticleSignals.Instance.onPlayerDeath.Invoke(transform.position);
                }
                else
                {
                    tag = "Collected";
                    collectableManager.IncreaseStackAfterDroneArea();
                    StackSignals.Instance.onCheckStack?.Invoke();
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("TurretColorArea"))
            {
                collectableManager.SetAnim(CollectableAnimationStates.Running);
                collectableManager.exitTurretArea();
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("TurretColorArea"))
            {
                collectableManager.EnterTurretArea(other.gameObject);
            }
        }
    }
}
