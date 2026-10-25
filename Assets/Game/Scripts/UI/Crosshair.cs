using UnityEngine;
using Zenject;

public class Crosshair 
{
    private IInputService _input;
    private RectTransform _rect;

    public Crosshair(IInputService input, RectTransform rect)
    {
        _input = input;
        _rect = rect;
        _input.LookPerformed += Move;
    }

    private void Move(Vector2 screenPos)
    {
        _rect.position += Vector3.one;
    }
}