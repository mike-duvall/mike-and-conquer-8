﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameview;
using mike_and_conquer.main;
using mike_and_conquer_monogame.main;


namespace mike_and_conquer.gameworld.humancontroller 
{
    public class PointerOverMapState : HumanControllerState
    {

        private Point leftMouseDownStartPoint = new Point(-1,-1);

        public override HumanControllerState Update( MouseState newMouseState, MouseState oldMouseState)
        {
            // if (MouseInputUtil.IsOverSidebar(newMouseState))
            // {
            //     return new PointerOverSidebarState();
            // }
            //
            // GameWorldView.instance.gameCursor.SetToMainCursor();

            Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);

            // if (GameWorld.instance.IsAMinigunnerSelected())
            // {
            //     UpdateMousePointerWhenMinigunnerSelected(mouseWorldLocationPoint);
            // }
            // else if (GameWorld.instance.IsAnMCVSelected())
            // {
            //     UpdateMousePointerWhenMCVSelected(mouseWorldLocationPoint);
            // }


            // if (MouseInputUtil.LeftMouseButtonClicked(newMouseState, oldMouseState))
            // {
            //     leftMouseDownStartPoint = mouseWorldLocationPoint;
            // }

            // if (MouseInputUtil.LeftMouseButtonIsBeingHeldDown(newMouseState, oldMouseState))
            // {
            //     if (MouseDragIsHappening(mouseWorldLocationPoint))
            //     {
            //         return new DragSelectingMapState(leftMouseDownStartPoint);
            //     }
            // }

            if (MouseInputUtil.LeftMouseButtonUnclicked(newMouseState, oldMouseState))
            {
                leftMouseDownStartPoint.X = -1;
                leftMouseDownStartPoint.Y = -1;
                Boolean handledEvent = HumanPlayerController.CheckForAndHandleLeftClickOnFriendlyUnit(mouseWorldLocationPoint);
                // if (!handledEvent)
                // {
                //     handledEvent = CheckForAndHandleLeftClickOnEnemyUnit(mouseWorldLocationPoint);
                // }
                //
                // if (!handledEvent)
                // {
                //     handledEvent = CheckForAndHandleLeftClickOnMap(mouseWorldLocationPoint);
                // }
            }

            if (MouseInputUtil.RightMouseButtonClicked(newMouseState, oldMouseState))
            {
                HandleRightClick(mouseWorldLocationPoint);
            }

            return this;
        }

        // private bool MouseDragIsHappening(Point mouseWorldLocationPoint)
        // {
        //
        //     if (leftMouseDownStartPoint.X != -1 && leftMouseDownStartPoint.Y != -1)
        //     {
        //         double distance = GetDistance(leftMouseDownStartPoint.X, leftMouseDownStartPoint.Y,
        //             mouseWorldLocationPoint.X, mouseWorldLocationPoint.Y);
        //
        //         if (distance > 20)
        //         {
        //             return true;
        //         }
        //     }
        //
        //     return false;
        //
        // }


        internal void HandleRightClick(Point mouseLocation)
        {

            int mouseX = mouseLocation.X;
            int mouseY = mouseLocation.Y;

            // foreach (Minigunner nextMinigunner in GameWorld.instance.GDIMinigunnerList)
            // {
            //     nextMinigunner.selected = false;
            // }
            //
            // if (GameWorld.instance.MCV != null)
            // {
            //     GameWorld.instance.MCV.selected = false;
            // }

            foreach (MinigunnerView minigunnerView in GameWorldView.instance.GdiMinigunnerViewList)
            {
                minigunnerView.Selected = false;
            }

        }

        private double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }




        // private bool CheckForAndHandleLeftClickOnMap(Point mouseLocation)
        // {
        //
        //     int mouseX = mouseLocation.X;
        //     int mouseY = mouseLocation.Y;
        //
        //     bool unitOrderedToMove = false;
        //
        //     foreach (Minigunner nextMinigunner in GameWorld.instance.GDIMinigunnerList)
        //     {
        //         if (nextMinigunner.selected == true)
        //         {
        //             if (GameWorld.instance.IsValidMoveDestination(new Point(mouseX, mouseY)))
        //             {
        //                 MapTileInstance clickedMapTileInstance =
        //                     GameWorld.instance.FindMapTileInstance(
        //                         MapTileLocation.CreateFromWorldCoordinates(mouseX, mouseY));
        //
        //
        //                 Point centerOfSquare = clickedMapTileInstance.GetCenter();
        //                 nextMinigunner.OrderToMoveToDestination(centerOfSquare);
        //                 unitOrderedToMove = true;
        //             }
        //         }
        //     }
        //
        //     MCV mcv = GameWorld.instance.MCV;
        //     if (mcv != null)
        //     {
        //         if (mcv.selected == true)
        //         {
        //             if (GameWorld.instance.IsValidMoveDestination(new Point(mouseX, mouseY)))
        //             {
        //                 MapTileInstance clickedMapTileInstance =
        //                     GameWorld.instance.FindMapTileInstance(
        //                         MapTileLocation.CreateFromWorldCoordinates(mouseX, mouseY));
        //
        //                 Point centerOfSquare = clickedMapTileInstance.GetCenter();
        //                 mcv.OrderToMoveToDestination(centerOfSquare);
        //                 unitOrderedToMove = true;
        //             }
        //
        //         }
        //     }
        //
        //     if (unitOrderedToMove)
        //     {
        //         MikeAndConquerGame.instance.SoundManager.PlayUnitAffirmative1();
        //     }
        //     return true;
        //
        // }
        //

        // internal Boolean CheckForAndHandleLeftClickOnEnemyUnit(Point mouseLocation)
        // {
        //     int mouseX = mouseLocation.X;
        //     int mouseY = mouseLocation.Y;
        //
        //     bool handled = false;
        //     foreach (Minigunner nextNodMinigunner in GameWorld.instance.NodMinigunnerList)
        //     {
        //         if (nextNodMinigunner.ContainsPoint(mouseX, mouseY))
        //         {
        //             handled = true;
        //             foreach (Minigunner nextGdiMinigunner in GameWorld.instance.GDIMinigunnerList)
        //             {
        //                 if (nextGdiMinigunner.selected)
        //                 {
        //                     nextGdiMinigunner.OrderToMoveToAndAttackEnemyUnit(nextNodMinigunner);
        //                 }
        //             }
        //         }
        //     }
        //
        //     return handled;
        // }
        //
        //
        // private static void UpdateMousePointerWhenMinigunnerSelected(Point mousePositionAsPointInWorldCoordinates)
        // {
        //     if (GameWorld.instance.IsPointOverEnemy(mousePositionAsPointInWorldCoordinates))
        //     {
        //         GameWorldView.instance.gameCursor.SetToAttackEnemyLocationCursor();
        //     }
        //     else if (GameWorld.instance.IsValidMoveDestination(mousePositionAsPointInWorldCoordinates))
        //     {
        //         GameWorldView.instance.gameCursor.SetToMoveToLocationCursor();
        //     }
        //     else
        //     {
        //         GameWorldView.instance.gameCursor.SetToMovementNotAllowedCursor();
        //     }
        // }

        // private static void UpdateMousePointerWhenMCVSelected(Point mousePositionAsPointInWorldCoordinates)
        // {
        //     if (GameWorld.instance.IsPointOverMCV(mousePositionAsPointInWorldCoordinates))
        //     {
        //         GameWorldView.instance.gameCursor.SetToBuildConstructionYardCursor();
        //     }
        //     else if (GameWorld.instance.IsValidMoveDestination(mousePositionAsPointInWorldCoordinates))
        //     {
        //         GameWorldView.instance.gameCursor.SetToMoveToLocationCursor();
        //     }
        //     else
        //     {
        //         GameWorldView.instance.gameCursor.SetToMovementNotAllowedCursor();
        //     }
        // }


    }
}
