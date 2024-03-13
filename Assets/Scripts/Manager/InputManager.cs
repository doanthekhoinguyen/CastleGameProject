using System;
using System.Collections;
using System.Collections.Generic;
using MVC.View;
using UnityEngine;
using UnityEngine.EventSystems;

public enum InputEvent
{
    OnHeroSlotClicked
}

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float raycastDistance = 50;

    [HideInInspector] public bool HasBlockInput;

    public Action<HeroSlotView> OnHeroSlotClicked = null;
    public Action OnPlaneClicked = null;

    private readonly RaycastHit[] raycastHits = new RaycastHit[1];

    public void SetCamera(Camera targetCamera)
    {
        camera = targetCamera;
    }

    void Update()
    {
        if (HasBlockInput) return;

        // Check if the left mouse button was clicked and not clicked on UI
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            int hitCount = Physics.RaycastNonAlloc(ray.origin, ray.direction, raycastHits, raycastDistance, 1 << 8);

            if (hitCount == 0) return;

            var obj = raycastHits[0].transform.gameObject;
            TriggerEvent(obj);
        }
    }

    private void TriggerEvent(GameObject target)
    {
        switch (target.tag)
        {
            case GameConst.HeroSlotTag:
                var view = target.GetComponent<HeroSlotView>();
                if (!view.isFirstSlot)
                    OnHeroSlotClicked?.Invoke(view);
                break;
            case GameConst.PlaneTag:
                OnPlaneClicked?.Invoke();
                break;
        }
    }
}
