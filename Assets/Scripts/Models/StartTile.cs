using Assets.Scripts.Enumerations;

namespace Assets.Scripts.Models
{
	public class StartTile
		: Tile
	{
		public Tile Construct()
		{
			Type = TileType.Regular;
			Orientation = Orientation.North;

			return this;
		}
	}
}
