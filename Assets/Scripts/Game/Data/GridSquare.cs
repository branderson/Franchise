namespace Game.Data
{
    public class GridSquare
    {
        public GridCoord NorthWest;
        public GridCoord NorthEast;
        public GridCoord SouthWest;
        public GridCoord SouthEast;

        /// <summary>
        /// Checks whether the given point is inside of the GridSquare
        /// </summary>
        /// <param name="point">
        /// Point to check
        /// </param>
        /// <returns>
        /// Whether the point is inside of the GridSquare
        /// </returns>
        public bool ContainsPoint(GridCoord point)
        {
            return point.X >= NorthWest.X &&
                   point.X <= NorthEast.X &&
                   point.Y >= SouthWest.Y &&
                   point.Y <= NorthWest.Y;
        }

        /// <summary>
        /// Checks whether the GridSquare intersects the given GridSquare
        /// </summary>
        /// <param name="square">
        /// Square to check for intersection with
        /// </param>
        /// <returns>
        /// Whether the squares intersect
        /// </returns>
        public bool IntersectsSquare(GridSquare square)
        {
            return NorthWest.X <= square.NorthEast.X &&
                   NorthEast.X >= square.NorthWest.X &&
                   NorthWest.Y >= square.SouthWest.Y &&
                   SouthWest.Y <= square.NorthWest.Y;
        }
    }
}