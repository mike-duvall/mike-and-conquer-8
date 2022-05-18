
using mike_and_conquer_monogame.main;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class UpdateUnitPositionCommand : AsyncViewCommand
    {


        private UnitPositionChangedEventData unitPositionChangedEventData;

        public UpdateUnitPositionCommand(UnitPositionChangedEventData data)
        {
            this.unitPositionChangedEventData = data;
        }

        protected override void ProcessImpl()
        {

            MikeAndConquerGame.instance.UpdateUnitPosition(unitPositionChangedEventData);

        }
    }
}
