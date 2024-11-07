////////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2020 zSpace, Inc.  All Rights Reserved.
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

using zSpace.Core.EventSystems;
using zSpace.Core.Input;

namespace zSpace.Core.Samples
{
    public class Draggable :
        ZPointerInteractable, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector3 originalPosition; // Edits made by Yazeed

        // Edits made by Yazeed
        private void Start()
        {
            originalPosition = transform.position;
        }

        IEnumerator SmoothReturn()
        {
            float duration = 0.5f; // Time to return
            float elapsed = 0f;

            Vector3 startPosition = transform.position;
            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(startPosition, originalPosition, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = originalPosition;
        }

        ////////////////////////////////////////////////////////////////////////
        // Public Methods
        ////////////////////////////////////////////////////////////////////////

        public override ZPointer.DragPolicy GetDragPolicy(ZPointer pointer)
        {
            if (pointer is ZMouse)
            {
                return ZPointer.DragPolicy.LockToScreenAlignedPlane;
            }

            if (pointer is ZStylus)
            {
                return ZPointer.DragPolicy.LockHitPosition;
            }

            return base.GetDragPolicy(pointer);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            ZPointerEventData pointerEventData = eventData as ZPointerEventData;
            if (pointerEventData == null ||
                pointerEventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            Pose pose = pointerEventData.Pointer.EndPointWorldPose;

            // Cache the initial grab state.
            this._initialGrabOffset =
                Quaternion.Inverse(this.transform.rotation) *
                (this.transform.position - pose.position);

            this._initialGrabRotation =
                Quaternion.Inverse(pose.rotation) *
                this.transform.rotation;

            // If the grabbable object has a rigidbody component,
            // mark it as kinematic during the grab.
            var rigidbody = this.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                this._isKinematic = rigidbody.isKinematic;
                rigidbody.isKinematic = true;
            }

            // Capture pointer events.
            pointerEventData.Pointer.CapturePointer(this.gameObject);
        }

        public void OnDrag(PointerEventData eventData)
        {
            ZPointerEventData pointerEventData = eventData as ZPointerEventData;
            if (pointerEventData == null ||
                pointerEventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            Pose pose = pointerEventData.Pointer.EndPointWorldPose;

            // Update the grab object's rotation.
            this.transform.rotation =
                pose.rotation * this._initialGrabRotation;

            // Update the grab object's position.
            this.transform.position =
                pose.position + 
                (this.transform.rotation * this._initialGrabOffset);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ZPointerEventData pointerEventData = eventData as ZPointerEventData;
            if (pointerEventData == null ||
                pointerEventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            // Release the pointer.
            pointerEventData.Pointer.CapturePointer(null);

            // If the grabbable object has a rigidbody component,
            // restore its original isKinematic state.
            var rigidbody = this.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = this._isKinematic;
            }

            // Edits's made by Yazeed
            // transform.position = originalPosition;
            StartCoroutine(SmoothReturn());
        }

        ////////////////////////////////////////////////////////////////////////
        // Private Members
        ////////////////////////////////////////////////////////////////////////

        private Vector3 _initialGrabOffset = Vector3.zero;
        private Quaternion _initialGrabRotation = Quaternion.identity;
        private bool _isKinematic = false;
    }
}
