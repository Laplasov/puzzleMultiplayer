using System.Collections.Generic;
using UnityEngine;

public class Highlighter
{
    List<SpaceMark> _currentMarks = new List<SpaceMark>(); 

    public void SetHighlight(List<SpaceMark> marks)
    {
        if (_currentMarks == marks) return;

        if (marks != null)
        {
            foreach (SpaceMark markCurrent in _currentMarks)
                markCurrent?.ResetColor();

            foreach (SpaceMark mark in marks)
                mark?.SetColor(Color.red); 

            _currentMarks = marks;
        }
    }

    public void StopHighlight()
    {
        if (_currentMarks != null)
        {
            foreach (SpaceMark markCurrent in _currentMarks)
                markCurrent?.ResetColor();
            _currentMarks = null;
        }
    }
}