using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TarodevController {
    /// <summary>
    /// Hey!
    /// Tarodev here. I built this controller as there was a severe lack of quality & free 2D controllers out there.
    /// Right now it only contains movement and jumping, but it should be pretty easy to expand... I may even do it myself
    /// if there's enough interest. You can play and compete for best times here: https://tarodev.itch.io/
    /// If you hve any questions or would like to brag about your score, come to discord: https://discord.gg/GqeHHnhHpz
    /// </summary>
    public class PlayerController : MonoBehaviour, IPlayerController {
        // Public for external hooks
        public Vector3 Velocity { get; private set; }
        public FrameInput Input { get; private set; }
        public bool JumpingThisFrame { get; private set; }
        public bool LandingThisFrame { get; private set; }
        public Vector3 RawMovement { get; private set; }
        public bool Grounded => _colDown;

        private Vector3 _lastPosition;
        [HideInInspector] public float _currentHorizontalSpeed, _currentVerticalSpeed;
        private GameObject body;
        private Player player_script;
        private BoxCollider2D box;
        private float add_y;

        //mine
        private PlayerAnimation anim_script;

        // This is horrible, but for some reason colliders are not fully established when update starts...
        private bool _active;
        void Awake() => Invoke(nameof(Activate), 0.5f);
        void Activate() =>  _active = true;
        
        private void Start(){
            anim_script = GetComponent<PlayerAnimation>();
            player_script = GetComponent<Player>();
            box = GetComponent<BoxCollider2D>();

            Bounds boxBounds = box.bounds;
            add_y = boxBounds.extents.y;
        }

        private void Update() {
            
            if(!_active) return;
            // Calculate velocity
            Velocity = (transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;
            
            getBody();
            GatherInput();
            RunCollisionChecks();

            CalculateWalk(); // Horizontal movement
            CalculateJumpApex(); // Affects fall speed, so calculate before gravity
            CalculateGravity(); // Vertical movement
            CalculateJump(); // Possibly overrides vertical

            MoveCharacter(); // Actually perform the axis movement
            
            //if(JumpingThisFrame) Debug.Log(JumpingThisFrame);

            //wall jump frames
            if(WallJumpedFrames > 0)
            {
                WallJumpedFrames--;
            }
            //double jump frames
            if(DoubleJumpFrames > 0)
            {
                DoubleJumpFrames--;
            }
        }


        #region Gather Input

        private void GatherInput() {
            Input = new FrameInput {
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
                X = WallJumpedFrames > 0 || DoubleJumpFrames > 0 ? 0 : UnityEngine.Input.GetAxisRaw("Horizontal"),
            };
            if (Input.JumpDown) {
                _lastJumpPressed = Time.time;
            }
        }

        #endregion

        #region Collisions

        [Header("COLLISION")] [SerializeField] private Bounds _characterBounds;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private int _detectorCount = 3;
        [SerializeField] private float _detectionRayLength = 0.1f;
        [SerializeField] [Range(0.1f, 0.3f)] private float _rayBuffer = 0.1f; // Prevents side detectors hitting the ground

        private RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
        private RayRange _raysUpR, _raysUpL, _raysDownR, _raysDownL;
        [HideInInspector] public bool _colUp, _colRight, _colDown, _colLeft;

        private float _timeLeftGrounded;

        // We use these raycast checks for pre-collision information
        private void RunCollisionChecks() {
            // Generate ray ranges. 
            CalculateRayRanged();

            // Ground
            LandingThisFrame = false;
            var groundedCheck = RunDetection(_raysDown);// || RunDetection(_raysDownR) || RunDetection(_raysDownL);
            if (_colDown && !groundedCheck)    // Only trigger when first leaving
            {
                _timeLeftGrounded = Time.time; 
            }
            else if (!_colDown && groundedCheck) {
                _coyoteUsable = true; // Only trigger when first touching
                LandingThisFrame = true;

                DoubleJumping = false;
                WallJumping = false;
                doubleJumpsRemain = _doubleJumps;
            }

            _colDown = groundedCheck;

            // The rest
            _colUp      = RunDetection(_raysUp)     || RunDetection(_raysUpR) || RunDetection(_raysUpL);
            _colLeft    = RunDetection(_raysLeft)   || RunDetection(_raysUpL) || RunDetection(_raysDownL);
            _colRight   = RunDetection(_raysRight)  || RunDetection(_raysUpR) || RunDetection(_raysDownR);

            bool RunDetection(RayRange range) {
                return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));
            }
        }

        private void CalculateRayRanged() {
            // This is crying out for some kind of refactor. 
            var b = new Bounds(transform.position + _characterBounds.center, _characterBounds.size);

            _raysDown   = new RayRange(b.min.x + _rayBuffer, b.min.y, b.max.x - _rayBuffer, b.min.y, Vector2.down);
            _raysUp     = new RayRange(b.min.x + _rayBuffer, b.max.y, b.max.x - _rayBuffer, b.max.y, Vector2.up);
            _raysLeft   = new RayRange(b.min.x, b.min.y + _rayBuffer, b.min.x, b.max.y - _rayBuffer, Vector2.left);
            _raysRight  = new RayRange(b.max.x, b.min.y + _rayBuffer, b.max.x, b.max.y - _rayBuffer, Vector2.right);

            float offset = _detectionRayLength * 0.6f;

            _raysUpR    = new RayRange(b.max.x - offset, b.max.y - offset, b.max.x - offset, b.max.y - offset, Vector2.one);
            _raysUpL    = new RayRange(b.min.x + offset, b.max.y - offset, b.min.x + offset, b.max.y - offset, new Vector2(-1,1));
            _raysDownL  = new RayRange(b.min.x + offset, b.min.y + offset, b.min.x + offset, b.min.y + offset, new Vector2(-1,-1));
            _raysDownR  = new RayRange(b.max.x - offset, b.min.y + offset, b.max.x - offset, b.min.y + offset, new Vector2(1,-1));
        }

        private IEnumerable<Vector2> EvaluateRayPositions(RayRange range) {
            for (var i = 0; i < _detectorCount; i++) {
                var t = (float)i / (_detectorCount - 1);
                yield return Vector2.Lerp(range.Start, range.End, t);
            }
        }  

        private void OnDrawGizmos() {
            // Bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + _characterBounds.center, _characterBounds.size);

            // Rays
            if (!Application.isPlaying) {
                CalculateRayRanged();
                Gizmos.color = Color.blue;
                foreach (var range in new List<RayRange> { _raysUp, _raysRight, _raysDown, _raysLeft, _raysUpR, _raysUpL, _raysDownR, _raysDownL }) {
                    foreach (var point in EvaluateRayPositions(range)) {
                        Gizmos.DrawRay(point, range.Dir * _detectionRayLength);
                    }
                }
            }

            if (!Application.isPlaying) return;

            // Draw the future position. Handy for visualizing gravity
            Gizmos.color = Color.red;
            var move = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed) * Time.deltaTime;
            Gizmos.DrawWireCube(transform.position + move, _characterBounds.size);
        }

        #endregion

        #region Walk

        [Header("WALKING")] [SerializeField] private float _acceleration = 90;
        [SerializeField] private float _moveClamp = 13;
        [SerializeField] private float _deAcceleration = 60f;
        [SerializeField] private float _AirDeAcceleration = 20f;
        [SerializeField] private float _apexBonus = 2;

        private void CalculateWalk() {
            
            if (Input.X != 0) {
                // Set horizontal move speed
                _currentHorizontalSpeed += Input.X * _acceleration * Time.deltaTime;

                // clamped by max frame movement
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);

                // Apply bonus at the apex of a jump
                var apexBonus = Mathf.Sign(Input.X) * _apexBonus * _apexPoint;
                _currentHorizontalSpeed += apexBonus * Time.deltaTime;
            }
            else {
                // No input. Let's slow the character down
                if(_colDown)
                    _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);       
            }
            
            //air friction
            if(!_colDown)
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _AirDeAcceleration * Time.deltaTime);

            if (_currentHorizontalSpeed > 0 && _colRight || _currentHorizontalSpeed < 0 && _colLeft) {
                // Don't walk through walls
                _currentHorizontalSpeed = 0;
            }
        }

        #endregion

        #region Gravity

        [Header("GRAVITY")] [SerializeField] private float _fallClamp = -40f;
        [SerializeField] private float _minFallSpeed = 80f;
        [SerializeField] private float _maxFallSpeed = 120f;
        private float _fallSpeed;

        private void CalculateGravity() {
            if (_colDown) {
                // Move out of the ground
                if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
            }
            else {
                // Add downward force while ascending if we ended the jump early
                var fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;

                // Fall
                _currentVerticalSpeed -= fallSpeed * Time.deltaTime;

                // Clamp
                if (_currentVerticalSpeed < _fallClamp) _currentVerticalSpeed = _fallClamp;
            }
        }

        #endregion

        #region Jump

        [Header("JUMPING")] 
        [SerializeField] private float _jumpHeight = 30;
        [SerializeField] private float _wallJumpHeight = 20;
        [SerializeField] private float _wallJumpPush = 10;
        [SerializeField] private float _airJumpHeight = 10;
        [SerializeField] private float _airJumpPush = 10;
        [SerializeField] private float _jumpApexThreshold = 10f;
        [SerializeField] private float _coyoteTimeThreshold = 0.1f;
        [SerializeField] private float _jumpBuffer = 0.1f;
        [SerializeField] private float _jumpEndEarlyGravityModifier = 3;
        [SerializeField] private int _wallJumpFrames = 5;
        [SerializeField] private int _doubleJumps = 1;
        [SerializeField] private int _doubleJumpFrames = 10;
        private int doubleJumpsRemain = 0;
        private bool _coyoteUsable;
        private bool _endedJumpEarly = true;
        private float _apexPoint; // Becomes 1 at the apex of a jump
        private float _lastJumpPressed;
        private bool CanUseCoyote => _coyoteUsable && !_colDown && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
        private bool HasBufferedJump => (_colDown) && _lastJumpPressed + _jumpBuffer > Time.time;
        private bool HasBufferedWallJumpRight => (_colLeft) && _lastJumpPressed + _jumpBuffer > Time.time;
        private bool HasBufferedWallJumpLeft => (_colRight)  && _lastJumpPressed + _jumpBuffer > Time.time;
        private bool HasBufferedDoubleJump => (doubleJumpsRemain > 0 && body != null) && (!HasBufferedWallJumpLeft && !HasBufferedWallJumpRight) && _lastJumpPressed + _jumpBuffer > Time.time;
        private int WallJumpedFrames = 0;
        private int DoubleJumpFrames = 0;
        public bool DoubleJumping = false;
        private bool WallJumping = false;

        private void CalculateJumpApex() {
            
            if (!_colDown) {
                // Gets stronger the closer to the top of the jump
                _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(Velocity.y));
                _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
            }
            else {
                _apexPoint = 0;
            }
        }

        private void CalculateJump() {
            // Jump if: grounded or within coyote threshold || sufficient jump buffer
            if (Input.JumpDown && CanUseCoyote || HasBufferedJump) {
                _currentVerticalSpeed = _jumpHeight;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                _timeLeftGrounded = float.MinValue;
                JumpingThisFrame = true;
            }
            //walljump left
            else if(Input.JumpDown && HasBufferedWallJumpRight)
            {
                _currentVerticalSpeed = _wallJumpHeight;
                _currentHorizontalSpeed = _wallJumpPush;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                _timeLeftGrounded = float.MinValue;
                JumpingThisFrame = true;
                WallJumpedFrames = _wallJumpFrames;
                WallJumping = true;

                //reset double jumps is 2 op
                //doubleJumpsRemain = _doubleJumps;
            }
            //walljump right
            else if(Input.JumpDown && HasBufferedWallJumpLeft)
            {
                _currentVerticalSpeed = _wallJumpHeight;
                _currentHorizontalSpeed = -_wallJumpPush;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                _timeLeftGrounded = float.MinValue;
                JumpingThisFrame = true;
                WallJumpedFrames = _wallJumpFrames;
                WallJumping = true;

                //reset double jumps
                //doubleJumpsRemain = _doubleJumps;
            }
            //double jump
            else if(Input.JumpDown && HasBufferedDoubleJump)
            {
                _currentVerticalSpeed = _jumpHeight;

                //calculate direction
                Vector3 _dir = (this.transform.position - body.transform.position);//[x,y]

                float _angle = Mathf.Atan2(_dir.y,_dir.x) * Mathf.Rad2Deg;
                _angle += _angle < 0 ? 360 : 0;

                //Debug.Log(_angle + ", " + _jumpXAngle.Evaluate(_angle));
                _currentHorizontalSpeed = -_jumpXAngle.Evaluate(_angle) * _airJumpPush;
                _currentVerticalSpeed   = _jumpYAngle.Evaluate(_angle) * _airJumpHeight;

                doubleJumpsRemain--;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                _timeLeftGrounded = float.MinValue;
                JumpingThisFrame = true;
                DoubleJumping = true;
                DoubleJumpFrames = _doubleJumpFrames; //how many frames to ignore input for
            }
            //not jumping
            else {
                JumpingThisFrame = false;
            }

            // End the jump early if button released
            if (!_colDown && Input.JumpUp && !_endedJumpEarly && Velocity.y > 0 && !WallJumping && !DoubleJumping) {
                // _currentVerticalSpeed = 0;
                _endedJumpEarly = true;
            }

            if (_colUp) {
                if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
            }
                
        }

        #endregion

        #region Move

        [Header("MOVE")] [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
        private int _freeColliderIterations = 10;

        // We cast our bounds before moving to avoid future collisions
        private void MoveCharacter() {
            var pos = transform.position;
            RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed); // Used externally
            var move = RawMovement * Time.deltaTime;
            var furthestPoint = pos + move;

            // check furthest movement. If nothing hit, move and don't do extra checks
            var hit = Physics2D.OverlapBox(furthestPoint, _characterBounds.size, 0, _groundLayer);
            if (!hit) {
                transform.position += move;
                return;
            }

            // otherwise increment away from current pos; see what closest position we can move to
            var positionToMoveTo = transform.position;
            for (int i = 1; i < _freeColliderIterations; i++) {
                // increment to check all but furthestPoint - we did that already
                var t = (float)i / _freeColliderIterations;
                var posToTry = Vector2.Lerp(pos, furthestPoint, t);

                if (Physics2D.OverlapBox(posToTry, _characterBounds.size, 0, _groundLayer)) {
                    transform.position = positionToMoveTo;

                    // We've landed on a corner or hit our head on a ledge. Nudge the player gently
                    if (i == 1) {
                        if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
                        var dir = transform.position - hit.transform.position;
                        transform.position += dir.normalized * move.magnitude;
                    }

                    return;
                }
                
                positionToMoveTo = posToTry;
            }
        }

        #endregion
    
        #region Misc

        [Header("Misc")]
        [SerializeField] private AnimationCurve _jumpXAngle;
        [SerializeField] private AnimationCurve _jumpYAngle;

        private void getBody()
        {
            body = player_script.lastBody;
        }

        #endregion
    }
}