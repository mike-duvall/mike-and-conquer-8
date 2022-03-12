
using Microsoft.Xna.Framework;
using mike_and_conquer.gameobjects;
// using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;
using mike_and_conquer_monogame.main;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

 
namespace mike_and_conquer.gameview
{
    public class MinigunnerView : UnitView
    {
        private UnitSprite unitSprite;
        private UnitSelectionCursor unitSelectionCursor;
        private DestinationSquare destinationSquare;
        // private Minigunner myMinigunner;
        private bool drawDestinationSquare;


        public bool Selected { get; set; }

        // public int XInWorldCoordinates { get; set; }
        // public int YInWorldCoordinates { get; set; }
        //

        enum AnimationSequences { STANDING_STILL, WALKING_UP, SHOOTING_UP };

        // protected MinigunnerView(Minigunner minigunner, string spriteListKey)
        // {
        //     this.myMinigunner = minigunner;
        //     this.unitSprite = new UnitSprite(spriteListKey);
        //     this.unitSprite.drawBoundingRectangle = false;
        //     this.unitSprite.drawShadow = true;
        //
        //     this.unitSelectionCursor = new UnitSelectionCursor(myMinigunner, (int)this.myMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.X, (int)this.myMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y);
        //     this.destinationSquare = new DestinationSquare();
        //     this.drawDestinationSquare = false;
        //     SetupAnimations();
        // }

        protected MinigunnerView(int id,string spriteListKey, int xInWorldCoordinates, int yInWorldCoordinates)
        {
            // this.myMinigunner = minigunner;
            this.ID = id;
            this.XInWorldCoordinates = xInWorldCoordinates;
            this.YInWorldCoordinates = yInWorldCoordinates;
            this.unitSprite = new UnitSprite(spriteListKey);
            this.unitSprite.drawBoundingRectangle = false;
            this.unitSprite.drawShadow = true;

            this.unitSize = new UnitSize(12, 16);


            this.unitSelectionCursor = new UnitSelectionCursor(this, XInWorldCoordinates, YInWorldCoordinates);
            // this.destinationSquare = new DestinationSquare();
            this.drawDestinationSquare = false;
            SetupAnimations();
            this.selectionCursorOffset = new Point(-6, -10);

        }



        private void SetupAnimations()
        {
            AnimationSequence walkingUpAnimationSequence = new AnimationSequence(40);
            walkingUpAnimationSequence.AddFrame(16);
            walkingUpAnimationSequence.AddFrame(17);
            walkingUpAnimationSequence.AddFrame(18);
            walkingUpAnimationSequence.AddFrame(19);
            walkingUpAnimationSequence.AddFrame(20);
            walkingUpAnimationSequence.AddFrame(21);

            unitSprite.AddAnimationSequence((int)AnimationSequences.WALKING_UP, walkingUpAnimationSequence);

            AnimationSequence standingStillAnimationSequence = new AnimationSequence(10);
            standingStillAnimationSequence.AddFrame(0);
            unitSprite.AddAnimationSequence((int)AnimationSequences.STANDING_STILL, standingStillAnimationSequence);
            unitSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.STANDING_STILL);


            AnimationSequence shootinUpAnimationSequence = new AnimationSequence(10);
            shootinUpAnimationSequence.AddFrame(65);
            shootinUpAnimationSequence.AddFrame(66);
            shootinUpAnimationSequence.AddFrame(67);
            shootinUpAnimationSequence.AddFrame(68);
            shootinUpAnimationSequence.AddFrame(69);
            shootinUpAnimationSequence.AddFrame(70);
            shootinUpAnimationSequence.AddFrame(71);
            shootinUpAnimationSequence.AddFrame(72);
            unitSprite.AddAnimationSequence((int)AnimationSequences.SHOOTING_UP, shootinUpAnimationSequence);
        }



        public void Update(GameTime gameTime)
        {
            unitSelectionCursor.Update(gameTime);
        }


        internal void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // if (myMinigunner.Health <= 0)
            // {
            //     return;
            // }


            // TODO:  move everything but actual drawing to Update() method
            // if (myMinigunner.state == Minigunner.State.IDLE)
            // {
                unitSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.STANDING_STILL);
            // }
            // else if (myMinigunner.state == Minigunner.State.MOVING)
            // {
            //     unitSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.WALKING_UP);
            // }
            // else if (myMinigunner.state == Minigunner.State.ATTACKING)
            // {
            //     unitSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.SHOOTING_UP);
            // }


            // unitSprite.DrawNoShadow(gameTime, spriteBatch, myMinigunner.GameWorldLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);
            Vector2 worldCoordinatesAsVector2 = new Vector2(
                XInWorldCoordinates,
                YInWorldCoordinates);

            unitSprite.DrawNoShadow(gameTime, spriteBatch, worldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);

            if (Selected)
            {
                unitSelectionCursor.DrawNoShadow(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
            }

        }


        public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // if (myMinigunner.Health <= 0)
            // {
            //     return;
            // }


            // unitSprite.DrawShadowOnly(gameTime, spriteBatch, myMinigunner.GameWorldLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);

            Vector2 worldCoordinatesAsVector2 = new Vector2(
                XInWorldCoordinates,
                YInWorldCoordinates);

            unitSprite.DrawShadowOnly(gameTime, spriteBatch, worldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);


            // if (myMinigunner.selected)
            // {
            //     unitSelectionCursor.DrawShadowOnly(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
            // }

            if (Selected)
            {
                unitSelectionCursor.DrawShadowOnly(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
            }


        }


        public void SetAnimate(bool animateFlag)
        {
            unitSprite.SetAnimate(animateFlag);
        }


        public bool ContainsPoint(int mouseX, int mouseY)
        {
            Rectangle clickDetectionRectangle = CreateClickDetectionRectangle();
            return clickDetectionRectangle.Contains(new Point(mouseX, mouseY));
        }


        internal Rectangle CreateClickDetectionRectangle()
        {

            int unitWidth = 12;
            int unitHeight = 12;

            int x = (int)(XInWorldCoordinates - (unitWidth / 2));
            int y = (int)(YInWorldCoordinates - unitHeight) + (int)(1);

            Rectangle rectangle = new Rectangle(x, y, unitWidth, unitHeight);
            return rectangle;
        }



    }
}
