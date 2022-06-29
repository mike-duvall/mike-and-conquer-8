using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class ProcessUnitMovementPlanCreated : AsyncViewCommand
    {


        private int unitId;
        private int x;
        private int y;

        public ProcessUnitMovementPlanCreated(int unitId, int x, int y)
        {
            this.unitId = unitId;
            this.x = x;
            this.y = y;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.AddMinigunner(
                unitId, x, y);

        }
    }
}
