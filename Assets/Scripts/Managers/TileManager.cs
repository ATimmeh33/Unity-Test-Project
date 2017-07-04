using Assets.Scripts.Enumerations;
using Assets.Scripts.Models;
using Assets.Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers
{
	public class TileManager : MonoBehaviour
	{
		public Transform startTile;
		public Transform straightTile;
		public Transform leftCornerTile;
		public Transform rightCornerTile;

		private const int MAX_TILES = 10;
		private const int MAX_CORNERS = 4;
		private const double CORNER_CHANCE = RandomUtilities.HUNDRED_PERCENT * 2 / 3;

		// Use this for initialization
		void Start()
		{
			Tiles = new List<Tile>(MAX_TILES + 1);
			ResetTiles();
			StartCoroutine(SpawnTile());
		}

		private IEnumerator SpawnTile()
		{
			while (true) {
				AddRandomTile();
				yield return new WaitForSeconds(.1f);
			}
		}

		public List<Tile> Tiles { get; private set; }

		private bool MayCreateCorner
		{
			get
			{
				if (Tiles.Last().GetComponent<Tile>().Type == TileType.Corner) {
					return false;
				}

				var reversed = Tiles.Reverse<Tile>().ToArray();
				if (reversed.Take(MAX_TILES / 2).Count(tile => IsCorner(tile)) >= MAX_CORNERS / 2 ||
					reversed.Take(MAX_TILES).Count(tile => IsCorner(tile)) >= MAX_CORNERS) {
					return false;
				}

				return true;
			}
		}

		private bool IsCorner(Tile tile)
		{
			return tile.Type == TileType.Corner;
		}

		public void ResetTiles()
		{
			while (Tiles.Count > 0) {
				// Actually remove tile from the game.
				Tiles.RemoveAt(0);
			}

			Tiles.Add(Instantiate(startTile).GetComponent<StartTile>().Construct());
			for (int i = 1; i < MAX_TILES; i++) {
				AddRandomTile();
			}
		}

		public void AddRandomTile()
		{
			var previousTile = Tiles.Last();
			if (RandomUtilities.PercentageChance(MayCreateCorner ? CORNER_CHANCE : 0)) {
				var randomCorner = RandomUtilities.Pick(leftCornerTile, rightCornerTile);
				Tiles.Add(Instantiate(randomCorner).GetComponent<Tile>().Construct(previousTile, TileType.Corner));
			}
			else {
				Tiles.Add(Instantiate(straightTile).GetComponent<Tile>().Construct(previousTile, TileType.Regular));
			}

			if (Tiles.Count > MAX_TILES) {
				var removeTile = Tiles[0];
				Destroy(removeTile.gameObject);
				Tiles.Remove(removeTile);
			}
		}
	}
}
