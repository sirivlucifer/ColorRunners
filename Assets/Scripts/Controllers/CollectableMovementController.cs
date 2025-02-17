﻿using DG.Tweening;
using Enums;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class CollectableMovementController : MonoBehaviour
    {
        
        #region Self Variables
    
        #region Public Variables
    
        #endregion
    
        #region Serialized Variables

        [SerializeField] private CollectableManager collectableManager;
        
        #endregion
    
        #endregion
        public void MoveToColorArea(Transform coloredDroneArea)
        {
            var RandomZ = Random.Range(-(coloredDroneArea.localScale.z/2-6),(coloredDroneArea.localScale.z/2 - 2));
            Vector3 newPos = new Vector3(coloredDroneArea.position.x,coloredDroneArea.position.y + 0.5f,
                coloredDroneArea.position.z + RandomZ);
            gameObject.transform.DOMove(newPos, 2f).OnComplete(() =>
            {
                collectableManager.SetAnim(CollectableAnimationStates.Crouching);
            });
        }
    }
}