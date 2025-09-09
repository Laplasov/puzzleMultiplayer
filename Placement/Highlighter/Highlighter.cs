using System.Collections.Generic;
using UnityEngine;

public class Highlighter
{
    SpaceMark _currentMark;
    public void SetHighlight(Vector3 cellCenter, Dictionary<Vector3, SpaceMark> GridMarks)
    {
        if (!GridMarks.ContainsKey(cellCenter))
        {
            if (_currentMark != null)
            {
                _currentMark.ResetColor();
                _currentMark = null;
            }
            return;
        }

        var mark = GridMarks[cellCenter];
        if (_currentMark == mark) return;

        if (mark != null)
        {
            _currentMark?.ResetColor();
            mark.SetColor(Color.red);
            _currentMark = mark;
        }
    }
    public void StopHighlight()
    {
        if (_currentMark != null)
        {
            _currentMark.ResetColor();
            _currentMark = null;
        }
    }
}
