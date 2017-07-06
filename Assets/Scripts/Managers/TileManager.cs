﻿using Assets.Scripts.Enumerations;
using Assets.Scripts.Models.Tiles;
using Assets.Scripts.Utilities;
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
		private const int TILES_BEHIND_PLAYER = 2;
		private const double CORNER_CHANCE = RandomUtilities.HUNDRED_PERCENT * 2 / 3;

		// Use this for initialization
		private void Awake()
		{
			if (Instance == null) {
				Instance = this;
			}
			else if (Instance != this) {
				Destroy(gameObject);
			}

			DontDestroyOnLoad(gameObject);
		}

		// Use this for initialization
		void Start()
		{
			Tiles = new List<Tile>(MAX_TILES + 1);
			ResetTiles();
		}

		public static TileManager Instance { get; private set; }

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

			AddTile(startTile);
			for (int i = 1; i < MAX_TILES - TILES_BEHIND_PLAYER; i++) {
				AddRandomTile();
			}
		}

		public void AddRandomTile()
		{
			var previousTile = Tiles.Last();
			if (RandomUtilities.PercentageChance(MayCreateCorner ? CORNER_CHANCE : 0)) {
				var randomCorner = RandomUtilities.Pick(leftCornerTile, rightCornerTile);
				AddTile(randomCorner, previousTile, TileType.Corner);
			}
			else {
				AddTile(straightTile, previousTile, TileType.Regular);
			}

			if (Tiles.Count > MAX_TILES) {
				var removeTile = Tiles[0];
				Destroy(removeTile.gameObject);
				Tiles.Remove(removeTile);
			}
		}

		public void AddTile(Transform tilePrefab, Tile previousTile = null, TileType? type = null)
		{
			var tile = Instantiate(tilePrefab).GetComponent<Tile>();
			if (previousTile != null && type.HasValue) {
				tile.Construct(previousTile, type.Value);
			}
			Tiles.Add(tile);
		}
	}
}
