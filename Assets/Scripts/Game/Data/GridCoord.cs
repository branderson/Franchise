using Game.Managers;
using UnityEngine;

namespace Game.Data
{
    /// <summary>
    /// 2D coordinate in grid space
    /// </summary>
    [System.Serializable]
    public class GridCoord
    {
        [SerializeField] public int X;// { get; set; }
        [SerializeField] public int Y;// { get; set; }

        /// <summary>
        /// Creates a GridCoord with given X and Y components
        /// </summary>
        /// <param name="x">
        /// X component of GridCoord
        /// </param>
        /// <param name="y">
        /// Y compoent of GridCoord
        /// </param>
        public GridCoord(int x, int y)
        {
            X = x;
            Y = y;
            if (x > SpaceManager.MaxXPos || y > SpaceManager.MaxYPos || x < 0 || y < 0)
            {
                Debug.Log("[GridCoord] Coordinate out of range: " + this + " not in [(0, 0), (" + 
                           SpaceManager.MaxXPos + ", " + SpaceManager.MaxYPos + ")]");
            }
        }

        /// <summary>
        /// Converts the GridCoord into world-space
        /// </summary>
        /// <returns>
        /// World-space position corresponding to this GridCoord
        /// </returns>
        public Vector3 ToWorldPosition()
        {
            return new Vector3(X*SpaceManager.GridResolution,
                               SpaceManager.Instance.GetHeight(this),
                               Y*SpaceManager.GridResolution);
        }

        /// <summary>
        /// Converts the GridCoord into world-space. Omits height to avoid non-thread safe operation
        /// </summary>
        /// <remarks>
        /// Thread safe
        /// </remarks>
        /// <returns>
        /// World-space position corresponding to this GridCoord
        /// </returns>
        public Vector3 To2DWorldPosition()
        {
            return new Vector3(X*SpaceManager.GridResolution,
                               0,
                               Y*SpaceManager.GridResolution);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
}