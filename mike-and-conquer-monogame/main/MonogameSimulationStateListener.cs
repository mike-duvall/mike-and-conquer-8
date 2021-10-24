using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.domain;
using Newtonsoft.Json;

namespace mike_and_conquer_monogame.main
{
    public class MonogameSimulationStateListener : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public MonogameSimulationStateListener(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            if (anEvent.EventType.Equals("MinigunnerCreated"))
            {
                MinigunnerCreateEventData minigunnerCreatedEventData =
                    JsonConvert.DeserializeObject<MinigunnerCreateEventData>(anEvent.EventData);


                mikeAndConquerGame.AddMinigunner(
                    minigunnerCreatedEventData.X,
                    minigunnerCreatedEventData.Y);

            }

        }
    }
}
