using System;
using System.Collections;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Interfaces;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public class ClassicCountdownTile : CountdownTileBase, IInteractableTile {
        
        public void OnPlayerStep() {
            ResolveCountdown();
        }

        protected override void ResolveCountdown() {
            if (countdownValue == 0) {
                //TODO: Add 1 to score and switch tile from countdown to goodTile
            } else {
                //TODO: switch tile from countdown to goodTile
            }
        }
    }
}