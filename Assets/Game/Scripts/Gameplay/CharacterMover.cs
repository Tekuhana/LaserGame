using Game.Scripts.Gameplay.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Zenject;

public class CharacterMover : ITickable, IInitializable
{
    private Camera _camera;
    private IInputService _input;
    private CharacterView _characterView;
    
    private float _maxSpeed;
    private float _jumpSpeed;
    private float _sensitivity;

    private Vector3 _velocity;
    private float _gravity = 9.81f;
    
    public CharacterMover(IInputService input, CharacterView characterView)
    {
        _input = input;
        _characterView = characterView;
        
        _maxSpeed = _characterView.MaxSpeed;
        _jumpSpeed = _characterView.JumpSpeed;
        _sensitivity = _characterView.Sensitivity;
        
        _input.JumpPressed += Jump;
        _input.LookPerformed += Rotate;
    }

    private void Jump()
    {
        if (_characterView.CharacterController.isGrounded == false)
            return;
        
        _velocity.y = _jumpSpeed;
    }

    private void Move()
    {
        _characterView.CharacterController.Move(_velocity * Time.deltaTime);
    }

    private void Rotate(Vector2 mousePosition)
    {
        Vector3 playerPos = _characterView.transform.position;
        float distToGround = _camera.transform.position.y - _characterView.transform.position.y;
        Vector3 worldMousePosition = _camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, distToGround));
        Vector3 rotateVector = new Vector3(worldMousePosition.x - playerPos.x, 0f, worldMousePosition.z - playerPos.z);
        Quaternion rotateDirection = Quaternion.LookRotation(rotateVector, Vector3.up);
        _characterView.transform.rotation = rotateDirection;
    }

    private void ChangeVelocity()
    {
        if (_characterView.CharacterController.isGrounded == false)
            _velocity.y -= _gravity * Time.deltaTime;

        Vector2 inputDirection = _input.Direction;

        Vector3 fwd = _characterView.transform.forward;
        fwd.y = 0f;
        fwd.Normalize();

        Vector3 right = _characterView.transform.right;
        right.y = 0f;
        right.Normalize();

        Vector3 moveDirection = inputDirection.x * right + inputDirection.y * fwd;

        _velocity.x = moveDirection.x * _maxSpeed;
        _velocity.z = moveDirection.z * _maxSpeed;
    }

    public void Tick()
    {
        ChangeVelocity();
        Move();
    }

    public void Initialize()
    {
        _camera = Camera.main;
    }
}
