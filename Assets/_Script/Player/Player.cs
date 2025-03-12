using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 MousePosBegin, MousePosEnd;
    private Vector2 LeftBottomPos, RightTopPos, sizeScale, rectangleDrawPosition;
    [SerializeField] private GameObject rectangleDraw;
    private List<Solder> selectedSoldiers = new List<Solder>();

    void Start()
    {
        SetActiveSelectedArea(false);
        InputManager.instance.subEventInput(EvenInputCategory.MouseDownLeft, () => SetMouseBeginHold());
        InputManager.instance.subEventInput(EvenInputCategory.MouseHoldLeft, () => DrawSelectedArea(MousePosBegin, MousePosEnd));
        InputManager.instance.subEventInput(EvenInputCategory.MouseUpLeft, () => SelectAllSoldiersOnAreaSelected());
        InputManager.instance.subEventInput(EvenInputCategory.MouseDownRight, () => MoveSoldiersInCircle(InputManager.instance.MousePosition, selectedSoldiers));
    }

    void SetActiveSelectedArea(bool set)
    {
        rectangleDraw.SetActive(set);
    }

    void SetMouseBeginHold()
    {
        MousePosBegin = InputManager.instance.MousePosition;
        MousePosEnd = MousePosBegin; 
        SetActiveSelectedArea(false); 
        rectangleDraw.transform.localScale = Vector3.zero;
    }

    void DrawSelectedArea(Vector2 mousePosBegin, Vector2 mousePosEnd)
    {
        MousePosEnd = InputManager.instance.MousePosition;
       
        LeftBottomPos.x = Mathf.Min(mousePosBegin.x, mousePosEnd.x);
        LeftBottomPos.y = Mathf.Min(mousePosBegin.y, mousePosEnd.y);
        RightTopPos.x = Mathf.Max(mousePosBegin.x, mousePosEnd.x);
        RightTopPos.y = Mathf.Max(mousePosBegin.y, mousePosEnd.y);
        sizeScale = RightTopPos - LeftBottomPos;
        rectangleDraw.transform.localScale = new Vector3(sizeScale.x, sizeScale.y, 1);
        rectangleDrawPosition.x = LeftBottomPos.x + sizeScale.x / 2;
        rectangleDrawPosition.y = LeftBottomPos.y + sizeScale.y / 2;
        rectangleDraw.transform.position = rectangleDrawPosition;
        SetActiveSelectedArea(true);
    }

    void SelectAllSoldiersOnAreaSelected()
    {
        SetActiveSelectedArea(false);
        foreach (Solder S in selectedSoldiers)
        {
            S.UnSelected();
        }
        selectedSoldiers.Clear();
        Collider2D[] colliders = Physics2D.OverlapAreaAll(LeftBottomPos, RightTopPos);
        foreach (var collider in colliders)
        {
            Solder soldier = collider.GetComponent<Solder>();
            if (soldier != null)
            {
                selectedSoldiers.Add(soldier);
                soldier.Selected();
            }
        }
    }
    public void MoveSoldiersInCircle(Vector3 targetPos, List<Solder> selectedSoldiers)
    {
        int soldierCount = selectedSoldiers.Count;
        float baseRadius = 1f;  
        int soldiersPerCircle = 3; 
        float spacing = 1f; 
        int remainingSoldiers = soldierCount;
        int currentCircle = 0;

        while (remainingSoldiers > 0)
        {
            int soldiersInThisCircle = Mathf.Min(soldiersPerCircle + currentCircle * 3, remainingSoldiers);
            float radius = baseRadius + spacing * currentCircle; 
            float angleStep = 360f / soldiersInThisCircle; 

            for (int i = 0; i < soldiersInThisCircle; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
                Vector3 finalPosition = targetPos + offset;

                selectedSoldiers[soldierCount - remainingSoldiers].ActionWhenSelected(finalPosition);
                remainingSoldiers--;
            }
            currentCircle++; 
        }
    }
}
