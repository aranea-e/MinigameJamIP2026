using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Marginagility {
    [DefaultExecutionOrder(0)]
    public class MarginagilityPawn : Pawn {
        Vector2 _moveInput = Vector2.zero;
        Vector2 _lookInput = Vector2.zero;
        [SerializeField]float _speed = 3f;
        [SerializeField]float _direction = 1f;

        [SerializeField] Transform _lanceTransform;

        [SerializeField]int _lane;
        [SerializeField]bool _hasLance = true;
        [SerializeField]int _lanceLane;

        [SerializeField]bool _isRiding = false;
        [SerializeField]bool _canRespawn = true;

        [SerializeField] SpriteRenderer _frogSprite;
        [SerializeField] SpriteRenderer _rabbitSprite;
        [SerializeField] SpriteRenderer _lanceSprite;

        [SerializeField] int _score;

        void Start() {
            _score = 0;
        }
        
        void Update() {
            if (_isRiding) {
                JoustUpdate();
            } else if (_canRespawn) {
                SpawnSelectUpdate();
            } else {
                RespawnUpdate();
            }
        }

        void JoustUpdate() {

            transform.Translate(_speed * _direction * Time.deltaTime, 0, 0);

            if (transform.position.x > 7.5 || transform.position.x < -7.5) {
                Despawn();
                return;
            }

            if (_moveInput.y > 0) {
                transform.position = new Vector3(transform.position.x, 2, transform.position.z);
                _lane = 1;
            } else if (_moveInput.y < 0) {
                transform.position = new Vector3(transform.position.x, -1, transform.position.z);
                _lane = 0;
            }

            if (_lookInput.x > 0) {
                _lanceLane = 1;
            } else if (_lookInput.x < 0) {
                _lanceLane = 0;
            }

            if (_lane == _lanceLane) {
                _lanceTransform.localPosition = new Vector3(2.25f * _direction, 0.5f, 1.0f);
                _lanceTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            } else if (_lane > _lanceLane) {
                _lanceTransform.localPosition = new Vector3(1.6f * _direction, -1.1f, 1.0f);
                _lanceTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, -45.0f * _direction);
            } else if (_lane < _lanceLane) {
                _lanceTransform.localPosition = new Vector3(1.6f * _direction, 1.9f, 1.0f);
                _lanceTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, 45.0f * _direction);
            }
        }

        void SpawnSelectUpdate() {

            if (_moveInput.y > 0) {
                _lane = 1;
            } else if (_moveInput.y < 0) {
                _lane = 0;
            }

            if (_moveInput.x > 0) {
                _direction = -1f;
            } else if (_moveInput.x < 0) {
                _direction = 1f;
            }
        }

        void RespawnUpdate() {

            transform.Translate(_speed * _direction * Time.deltaTime, 0, 0);

            if (transform.position.x > 7.5 || transform.position.x < -7.5) {
                Despawn();
                return;
            }

        }

        void Despawn() {
            _canRespawn = true;
            _isRiding = false;

            _frogSprite.enabled = false;
            _rabbitSprite.enabled = false;
            _lanceSprite.enabled = false;
        }

        void Respawn() {
            _canRespawn = false;
            _isRiding = true;
            _hasLance = true;

            _frogSprite.enabled = true;
            _rabbitSprite.enabled = true;
            _lanceSprite.enabled = true;

            if (_lane == 1) {
                transform.position = new Vector3(-7.5f * _direction, 2, 0);
            } else {
                transform.position = new Vector3(-7.5f * _direction, -1, 0);
            }
        }

        protected override void OnActionPressed(InputAction.CallbackContext context) {
            if (context.action.name == PawnAction.Move) {
                _moveInput = context.ReadValue<Vector2>();
            }

            if (context.action.name == PawnAction.Look) {
                _lookInput = context.ReadValue<Vector2>();
            }

            if (context.action.name == PawnAction.ButtonA) {
                if (_canRespawn) Respawn();
            }
        }

        public float GetScore() {
            return _score;
        }

        public float GetDirection() {
            return _direction;
        }

        public float GetLane() {
            return _lane;
        }

        public float GetLanceLane() {
            return _lanceLane;
        }

        public bool GetRiding() {
            return _isRiding;
        }

        public bool GetLance() {
            return _hasLance;
        }

        public void Fall() {
            _score -= 1;
            _isRiding = false;
            _hasLance = false;
        }

        public void Hit(int count) {
            _score += 2 * count;
        }
    }
}