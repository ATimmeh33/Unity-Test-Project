using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Models.Tiles.Triggers
{
	class LeftCornerTileTrigger
		: MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<Player>() != null) {
				TileManager.Instance.AddRandomTile();
			}
		}
	}
}
