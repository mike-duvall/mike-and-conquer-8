using mike_and_conquer_simulation.events;
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
            if (anEvent.EventType.Equals(MinigunnerCreateEventData.EventName))
            {
                MinigunnerCreateEventData eventData =
                    JsonConvert.DeserializeObject<MinigunnerCreateEventData>(anEvent.EventData);


                mikeAndConquerGame.AddMinigunner(
                    eventData.ID,
                    eventData.X,
                    eventData.Y);

            }
            else if (anEvent.EventType.Equals(JeepCreateEventData.EventName))
            {
                JeepCreateEventData eventData =
                    JsonConvert.DeserializeObject<JeepCreateEventData>(anEvent.EventData);


                mikeAndConquerGame.AddJeep(
                    eventData.ID,
                    eventData.X,
                    eventData.Y);

            }
            else if (anEvent.EventType.Equals(UnitPositionChangedEventData.EventName))
            {
                UnitPositionChangedEventData unitPositionChangedEventData =
                    JsonConvert.DeserializeObject<UnitPositionChangedEventData>(anEvent.EventData);



                mikeAndConquerGame.UpdateMinigunnerPosition(unitPositionChangedEventData);


            }
            else if (anEvent.EventType.Equals(InitializeScenarioEventData.EventName))
            {
                InitializeScenarioEventData initializeScenarioEventData =
                    JsonConvert.DeserializeObject<InitializeScenarioEventData>(anEvent.EventData);

                mikeAndConquerGame.InitializeScenario(initializeScenarioEventData);
            }
            else if (anEvent.EventType.Equals("ResetScenario"))
            {
                mikeAndConquerGame.ResetScenario();
            }



        }
    }
}
