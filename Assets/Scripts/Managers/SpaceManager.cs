using System.IO;
using System.Linq;
using Controllers;
using Data;
using Assets.Utility;
using Assets.TerrainGenerator.Scripts;
using UnityEditor;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Manages game space
    /// </summary>
	public class SpaceManager : Singleton<SpaceManager>
    {
        private Terrain _terrain;
        private TerrainCollider _terrainCollider;
        private static readonly float _gridResolution = 2f;
        public static int MaxXPos { get; private set; }
        public static int MaxYPos { get; private set; }

        public float SeaLevel = 0f;

        protected SpaceManager() { }

        public static float GridResolution
        {
            get { return _gridResolution; }
        }

        protected void Start()
		{
            Initialize();
            InitializeGameCamera();
		}
		
		protected void Update()
		{
		}

        /// <summary>
        /// Initialize the space manager
        /// </summary>
	    public void Initialize()
        {
            _terrain = FindObjectOfType<Terrain>();
            _terrainCollider = _terrain.GetComponent<TerrainCollider>();
            _terrain.transform.parent.GetComponent<MapController>().Initialize();
            MaxXPos = (int) (_terrain.terrainData.size.x/_gridResolution);
            MaxYPos = (int) (_terrain.terrainData.size.z/_gridResolution);

        }

        public void InitializeGameCamera()
        {
            // Move the camera to the correct position
            GameCameraController controller = FindObjectOfType<GameCameraController>();
            GridCoord center = new GridCoord(MaxXPos / 2, MaxYPos / 2);
            controller.transform.position = new Vector3(center.ToWorldPosition().x,
                                                        controller.transform.position.y,
                                                        center.ToWorldPosition().z);
            controller.Initialize();
        }

	    public void Uninitialize()
	    {
	        
	    }

        /// <summary>
        /// Testing function to initialize SpaceManager to a 10x10 grid
        /// </summary>
        public static void InitializeTestingSize()
        {
            MaxXPos = 9;
            MaxYPos = 9;
        }

        /// <summary>
        /// Gets the Z value of the heightmap at the game-space x and y position specified
        /// </summary>
        /// <param name="x">
        /// X position of heightmap cooridinate
        /// </param>
        /// <param name="y">
        /// Y position of heightmap coordinate
        /// </param>
        /// <returns>
        /// Height of heightmap at specified location
        /// </returns>
        public float GetHeight(float x, float y)
        {
            if (SingletonSettings.EditorTest) return 0;
            return _terrain.SampleHeight(new Vector3(x*_gridResolution, 0, y*_gridResolution));
        }

        /// <summary>
        /// Gets the Z value of the heightmap at the game-space position specified in pos
        /// </summary>
        /// <param name="pos">
        /// GridCoord representing x and y position to get height at
        /// </param>
        /// <returns>
        /// Height of heightmap at specified location
        /// </returns>
        public float GetHeight(GridCoord pos)
        {
            return GetHeight(pos.X, pos.Y);
        }

        /// <summary>
        /// Gets the Z value of the heightmap at the world-space x and y position specified in pos
        /// </summary>
        /// <param name="pos">
        /// Vector2 representing x and y position to get height at
        /// </param>
        /// <returns>
        /// Height of heightmap at specified location
        /// </returns>
        public float GetHeight(Vector2 pos)
        {
            return GetHeight(ToGridPosition(pos));
        }

        /// <summary>
        /// Gets the Z value of the heightmap at the world-space x and y position specified in pos
        /// </summary>
        /// <param name="pos">
        /// Vector3 representing x and y position to get height at. Z value of parameter is unused
        /// </param>
        /// <returns>
        /// Height of heightmap at specified location
        /// </returns>
        public float GetHeight(Vector3 pos)
        {
            return GetHeight(ToGridPosition(pos));
        }

        /// <summary>
        /// Gets the point in world-space that the given ray intersects the ground
        /// </summary>
        /// <param name="ray">
        /// Ray to find intersect with the ground
        /// </param>
        /// <returns>
        /// World-space point where ray intersects the ground
        /// </returns>
        public Vector3 GetGroundIntersectionPoint(Ray ray)
        {
            RaycastHit hit;
            if (_terrainCollider.Raycast(ray, out hit, 1000f))
            {
                return hit.point;
            }
            return Vector3.zero;
        }

        /// <summary>
        /// Converts the given position to the nearest position in game-space
        /// </summary>
        /// <param name="pos">
        /// World-space position to convert
        /// </param>
        /// <returns>
        /// Nearest game-space position
        /// </returns>
        public GridCoord ToGridPosition(Vector3 pos)
	    {
            // Divide by resolution and take nearest point
            if (pos.x < 0 || pos.z < 0)
            {
                Debug.Log("[SpaceManager] Converting position outside quadrant I to GridCoord");
            }
            GridCoord position = new GridCoord(Mathf.RoundToInt(pos.x/_gridResolution), 
                                               Mathf.RoundToInt(pos.z/_gridResolution));
            return position;
	    }

        /// <summary>
        /// Deprecated: Use GridCoord.ToWorldSpace()
        /// Converts the given game-space position to a world-space position
        /// </summary>
        /// <param name="pos">
        /// game-space position to convert
        /// </param>
        /// <returns>
        /// Corresponding world-space position
        /// </returns>
	    public Vector3 ToWorldPosition(GridCoord pos)
	    {
            Vector3 position = new Vector3
            {
                x = pos.X*_gridResolution,
                y = GetHeight(pos.X, pos.Y),
                z = pos.Y*_gridResolution
            };

	        return position;
	    }

        /// <summary>
        /// Scales to given value in world-space to game-space
        /// </summary>
        /// <param name="val">
        /// Value to scale
        /// </param>
        /// <returns>
        /// Value scaled to game-space
        /// </returns>
        public long ScaleToGrid(long val)
        {
            return Mathf.RoundToInt(val/_gridResolution);
        }

        /// <summary>
        /// Scales to given value in world-space to game-space
        /// </summary>
        /// <param name="val">
        /// Value to scale
        /// </param>
        /// <returns>
        /// Value scaled to game-space
        /// </returns>
        public long ScaleToGrid(float val)
        {
            return Mathf.RoundToInt(val/_gridResolution);
        }

        /// <summary>
        /// Scales to given value in game-space to world-space
        /// </summary>
        /// <param name="val">
        /// Value to scale
        /// </param>
        /// <returns>
        /// Value scaled to world-space
        /// </returns>
        public float ScaleToWorld(long val)
        {
            return val*_gridResolution;
        }

        /// <summary>
        /// Scales to given value in game-space to world-space
        /// </summary>
        /// <param name="val">
        /// Value to scale
        /// </param>
        /// <returns>
        /// Value scaled to world-space
        /// </returns>
        public float ScaleToWorld(float val)
        {
            return val*_gridResolution;
        }

        /// <summary>
        /// Calculate the Manhattan distance between two coordinates in game-space
        /// </summary>
        /// <param name="c1">
        /// First coordinate
        /// </param>
        /// <param name="c2">
        /// Second coordinate
        /// </param>
        /// <returns>
        /// Manhattan distance between the two coordinates
        /// </returns>
        public static long ManhattanDistance(GridCoord c1, GridCoord c2)
        {
            return Mathf.Abs(c1.X - c2.X) + Mathf.Abs(c1.Y - c2.Y);
        }

        /// <summary>
        /// Serialize a GridCoord into a long
        /// </summary>
        /// <param name="pos">
        /// GridCoord to serialize
        /// </param>
        /// <returns>
        /// Serialized value of GridCoord
        /// </returns>
        public static long SerializePosition(GridCoord pos)
        {
            return pos.Y*MaxXPos + pos.X;
        }

        /// <summary>
        /// Deserialize a long into a GridCoord
        /// </summary>
        /// <param name="pos">
        /// Long to deserialize
        /// </param>
        /// <returns>
        /// Deserialized GridCoord
        /// </returns>
        public static GridCoord DeserializePosition(long pos)
        {
            int xPos = (int) (pos%MaxXPos);
            int yPos = (int) (pos/MaxXPos);
            return new GridCoord(xPos, yPos);
        }
	}
}