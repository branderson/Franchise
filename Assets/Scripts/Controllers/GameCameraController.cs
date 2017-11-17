using UnityEngine;
using Assets.Utility.Static;
using Managers;

namespace Controllers
{
	public class GameCameraController : MonoBehaviour
	{
	    [SerializeField] private float _moveSpeed = 20f;
	    [SerializeField] private float _scrollSpeed = 30f;
	    [SerializeField] private float _edgeScrollSpeed = 1f;
	    [SerializeField] private float _maxZoom = 12f;
	    [SerializeField] private float _minZoom = 300f;
	    private float _maxZoomAdaptive;
	    private float _minZoomAdaptive;
	    private float _curZoom;
	    private float _baseZoom;
	    private float _baseZoomAdaptive;

	    private Vector3 _goalPos;
//	    private Vector2 _lastCursorPos;

        /// <summary>
        /// Initialize the position of the camera and zoom constants
        /// </summary>
		public void Initialize()
		{
		    transform.position = new Vector3(transform.position.x, 
                                             SpaceManager.Instance.GetHeight(transform.position) + transform.position.y,
                                             transform.position.z);
		    _baseZoom = transform.position.y;
		    _goalPos = transform.position;

		    _maxZoomAdaptive = SpaceManager.Instance.GetHeight(transform.position) + _maxZoom;
            if (_maxZoomAdaptive < SpaceManager.Instance.SeaLevel) _maxZoomAdaptive = SpaceManager.Instance.SeaLevel;
		    _minZoomAdaptive = _maxZoomAdaptive + (_minZoom - _maxZoom);
		}
		
		protected void Update()
		{
		    float hor = Input.GetAxis("Horizontal");
		    float ver = Input.GetAxis("Vertical");
		    float rot = 0;
		    Vector2 scroll = Input.mouseScrollDelta;

            // Camera rotation
		    if (Input.GetMouseButtonDown(2))
		    {
                // Move cursor to center of screen
		        Cursor.lockState = CursorLockMode.Locked;
                // Let player move cursor
                Cursor.lockState = CursorLockMode.None;
                // Make the cursor invisible while rotating
		        Cursor.visible = false;
		    }
		    if (Input.GetMouseButton(2))
		    {
		        rot = Input.GetAxis("Mouse X");
                transform.Rotate(new Vector3(0f, rot, 0f), Space.World);
                
                // Move cursor to center of screen
		        Cursor.lockState = CursorLockMode.Locked;
                // Let player move cursor
                Cursor.lockState = CursorLockMode.None;
		    }
		    if (Input.GetMouseButtonUp(2))
		    {
		        // Make cursor visible
		        Cursor.visible = true;
		    }

            // Edge movement
		    if (_edgeScrollSpeed > 0)
		    {
                if (Input.mousePosition.x < Screen.width/10f)
                {
                    float val = _edgeScrollSpeed * (Input.mousePosition.x - Screen.width/10f) / (.1f * Screen.width);
                    if (val < hor)
                    {
                        hor = val;
                    }
                }
                else if (Input.mousePosition.x > 9*Screen.width/10f)
                {
                    float val = _edgeScrollSpeed * (Input.mousePosition.x - 9*Screen.width/10f)/(.1f*Screen.width);
                    if (val > hor)
                    {
                        hor = val;
                    }
                }
                if (Input.mousePosition.y < Screen.height/5f)
                {
                    float val = _edgeScrollSpeed * (Input.mousePosition.y - Screen.height/5f) / (Screen.height * 1/5f);
                    if (val < ver)
                    {
                        ver = val;
                    }
                }
                else if (Input.mousePosition.y > 4*Screen.height/5f)
                {
                    float val = _edgeScrollSpeed * (Input.mousePosition.y - 4*Screen.height/5f)/(Screen.height * 1/5f);
                    if (val > ver)
                    {
                        ver = val;
                    }
                }
		    }

            // Adjust to current world height
		    _maxZoomAdaptive = SpaceManager.Instance.GetHeight(transform.position) + _maxZoom;
            if (_maxZoomAdaptive < SpaceManager.Instance.SeaLevel) _maxZoomAdaptive = SpaceManager.Instance.SeaLevel;
		    _baseZoomAdaptive = _maxZoomAdaptive - _maxZoom + _baseZoom;

            // Cap scroll speed
		    if (scroll.y > 10f)
		    {
		        scroll.y = 10f;
		    }
            else if (scroll.y < -10f)
            {
                scroll.y = -10f;
            }

            // Slow down everything as we get more zoomed in
		    _curZoom = _goalPos.y / _baseZoomAdaptive;

		    if (_goalPos.y <= _maxZoomAdaptive)
		    {
		        _goalPos.y = _maxZoomAdaptive;
                // Prevent scrolling in after _maxZoomAdaptive
                if (scroll.y > 0)
                {
                    _curZoom = 0f;
                }
		    }
		    if (_goalPos.y >= _minZoomAdaptive)
		    {
                _goalPos.y = _minZoomAdaptive;
                // Prevent scrolling out after _minZoomAdaptive
                if (scroll.y < 0 && _goalPos.y >= _minZoomAdaptive)
                {
                    _curZoom = 0f;
                }
		    }

            // Move along X and Z planes
		    if (Mathf.Abs(hor) > 0.1f || Mathf.Abs(ver) > 0.1f)
		    {
		        float zoomMod = _curZoom.Equals(0) ? transform.position.y/_baseZoomAdaptive : _curZoom;
                Vector3 localPos = new Vector3(hor * _moveSpeed * zoomMod * Time.deltaTime,
                                               0, 
                                               ver * _moveSpeed * zoomMod * Time.deltaTime);
                // Rotate by camera direction
		        localPos = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0))*localPos;
                _goalPos = new Vector3(_goalPos.x + localPos.x,
                                       _goalPos.y + localPos.y, 
                                       _goalPos.z + localPos.z);
		    }

            // Zoom in and out
            if (Mathf.Abs(scroll.y) > 0.1f)
		    {
                Vector3 scrollVector = new Vector3(0, 0, scroll.y*_scrollSpeed*_curZoom);
		        scrollVector = transform.TransformDirection(scrollVector);
		        if ((_goalPos + scrollVector).y < _maxZoomAdaptive)
		        {
                    // TODO: Get unit vector of local space, minimize scrollVector to reach max
                    // 
		            scrollVector = Vector3.zero;
		        }
                else if ((_goalPos + scrollVector).y > _minZoomAdaptive)
                {
		            scrollVector = Vector3.zero;
                }
		        _goalPos = new Vector3(_goalPos.x + scrollVector.x, _goalPos.y + scrollVector.y, 
                                       _goalPos.z + scrollVector.z);
		    }

            // Constrain to world boundaries
		    if (SpaceManager.Instance.ScaleToGrid(_goalPos.x) >= SpaceManager.MaxXPos)
		    {
		        _goalPos.x = SpaceManager.Instance.ScaleToWorld(SpaceManager.MaxXPos);
		    }
            else if (SpaceManager.Instance.ScaleToGrid(_goalPos.x) <= 0)
            {
		        _goalPos.x = SpaceManager.Instance.ScaleToWorld(0);
            }
		    if (SpaceManager.Instance.ScaleToGrid(_goalPos.z) >= SpaceManager.MaxYPos)
		    {
		        _goalPos.z = SpaceManager.Instance.ScaleToWorld(SpaceManager.MaxYPos);
		    }
            else if (SpaceManager.Instance.ScaleToGrid(_goalPos.z) <= 0)
            {
		        _goalPos.z = SpaceManager.Instance.ScaleToWorld(0);
            }

            // Move the camera smoothly
		    transform.position = Vector3.Lerp(transform.position, _goalPos, Time.deltaTime * 10f);

            // Make sure the camera never goes below the map
		    if (transform.position.y < _maxZoomAdaptive)
		    {
		        transform.position = new Vector3(transform.position.x, _maxZoomAdaptive, transform.position.z);
		    }
		}

	    public void ToggleScrollSpeed()
	    {
	        // TODO: Temporary debug function. Remove this
	        _edgeScrollSpeed = _edgeScrollSpeed > 0 ? 0 : 1f;
	    }
	}
}