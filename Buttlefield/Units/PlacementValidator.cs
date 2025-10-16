using UnityEngine;

public class PlacementValidator
{
    public bool CanPerformAction(SpaceMark currentUnit, SpaceMark target, PlacementSystem board, out bool shouldMove, out bool shouldSwap)
    {
        shouldMove = false;
        shouldSwap = false;

        // Case 1: Target has a unit that's not the current unit
        if (target.Unit != null && target.Unit != currentUnit.Unit)
        {
            // If target is a pointer mark, check what it points to
            if (target.PointerMark != null)
            {
                // This is a pointer mark - block if it points to a different unit
                if (target.PointerMark.Unit != currentUnit.Unit)
                {
                    return false;
                }
            }
            else
            {
                // On UnitHolder, all units are treated as size 1x1, so allow swapping
                // Only check size differences on Battlefield
                if (board.GetPlacementType() == PlacementType.Battlefield &&
                    currentUnit.GetSizeUnit() != target.GetSizeUnit())
                {
                    return false;
                }
            }
        }
        // Case 2: Target is empty but cannot place unit there
        else if (target.Unit == null && !target.CanPlaceUnit(currentUnit))
        {
            return false;
        }

        // Determine action type
        if (target.Unit == null ||
            target.Unit == currentUnit.Unit ||
            (target.PointerMark != null && target.PointerMark.Unit == currentUnit.Unit))
        {
            shouldMove = target.CanPlaceUnit(currentUnit);
            shouldSwap = false;
        }
        else
        {
            shouldMove = false;
            // On UnitHolder, always allow swaps (all units are treated as 1x1)
            // On Battlefield, only allow swaps if sizes match
            if (board.GetPlacementType() == PlacementType.Battlefield &&
                currentUnit.GetSizeUnit() != target.GetSizeUnit())
            {
                return false;
            }
            shouldSwap = true;
        }

        return true;
    }

    public bool CanPerformActionm(SpaceMark currentUnit, SpaceMark target, PlacementSystem board, out bool shouldMove, out bool shouldSwap)
    {
        shouldMove = false;
        shouldSwap = false;

        // CRITICAL FIX: Always use the main mark, not pointer marks
        SpaceMark mainCurrentMark = GetUnitToSelect(currentUnit);
        SpaceMark mainTargetMark = GetUnitToSelect(target);

        // Case 1: Target has a unit that's not the current unit
        if (mainTargetMark.Unit != null && mainTargetMark.Unit != mainCurrentMark.Unit)
        {
            // On UnitHolder, all units are treated as size 1x1, so allow swapping
            // Only check size differences on Battlefield
            if (board.GetPlacementType() == PlacementType.Battlefield &&
                mainCurrentMark.GetSizeUnit() != mainTargetMark.GetSizeUnit())
            {
                return false;
            }
        }
        // Case 2: Target is empty but cannot place unit there
        else if (mainTargetMark.Unit == null && !mainTargetMark.CanPlaceUnit(mainCurrentMark))
        {
            return false;
        }

        // Determine action type
        if (mainTargetMark.Unit == null || mainTargetMark.Unit == mainCurrentMark.Unit)
        {
            shouldMove = mainTargetMark.CanPlaceUnit(mainCurrentMark);
            shouldSwap = false;
        }
        else
        {
            shouldMove = false;
            // On UnitHolder, always allow swaps (all units are treated as 1x1)
            // On Battlefield, only allow swaps if sizes match
            if (board.GetPlacementType() == PlacementType.Battlefield &&
                mainCurrentMark.GetSizeUnit() != mainTargetMark.GetSizeUnit())
            {
                return false;
            }
            shouldSwap = true;
        }

        return true;
    }

    public SpaceMark GetUnitToSelect(SpaceMark target)
    {
        // Always use the main mark for selection, not pointer marks
        return target.PointerMark != null ? target.PointerMark : target;
    }
}