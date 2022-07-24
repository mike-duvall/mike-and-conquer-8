using mike_and_conquer_monogame.commands;

using mike_and_conquer_simulation.events;
using Newtonsoft.Json;


namespace mike_and_conquer_monogame.main
{
    public class AddJeepViewWhenJeepCreatedEventHandler : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public AddJeepViewWhenJeepCreatedEventHandler(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            if (anEvent.EventType.Equals(JeepCreateEventData.EventType))
            {
                JeepCreateEventData eventData =
                    JsonConvert.DeserializeObject<JeepCreateEventData>(anEvent.EventData);

                AddJeepCommand command = new AddJeepCommand(eventData.UnitId, eventData.X, eventData.Y);

                mikeAndConquerGame.PostCommand(command);

            }


        }
    }
}
