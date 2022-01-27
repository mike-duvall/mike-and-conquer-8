using mike_and_conquer_monogame.commands;
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

                AddMinigunnerCommand command = new AddMinigunnerCommand(eventData.ID, eventData.X, eventData.Y);

                mikeAndConquerGame.PostCommand(command);
                
                // mikeAndConquerGame.AddMinigunner(
                //     eventData.ID,
                //     eventData.X,
                //     eventData.Y);

            }
            else if (anEvent.EventType.Equals(JeepCreateEventData.EventName))
            {
                JeepCreateEventData eventData =
                    JsonConvert.DeserializeObject<JeepCreateEventData>(anEvent.EventData);

                AddJeepCommand command = new AddJeepCommand(eventData.ID, eventData.X, eventData.Y);

                mikeAndConquerGame.PostCommand(command);


                // mikeAndConquerGame.AddJeep(
                //     eventData.ID,
                //     eventData.X,
                //     eventData.Y);

            }
            else if (anEvent.EventType.Equals(MCVCreateEventData.EventName))
            {
                MCVCreateEventData eventData =
                    JsonConvert.DeserializeObject<MCVCreateEventData>(anEvent.EventData);

                AddMCVCommand command = new AddMCVCommand(eventData.ID, eventData.X, eventData.Y);

                mikeAndConquerGame.PostCommand(command);


                // mikeAndConquerGame.AddMCV(
                //     eventData.ID,
                //     eventData.X,
                //     eventData.Y);

            }

            else if (anEvent.EventType.Equals(UnitPositionChangedEventData.EventName))
            {
                UnitPositionChangedEventData unitPositionChangedEventData =
                    JsonConvert.DeserializeObject<UnitPositionChangedEventData>(anEvent.EventData);


                UpdateUnitPositionCommand command = new UpdateUnitPositionCommand(unitPositionChangedEventData);
                mikeAndConquerGame.PostCommand(command);

                // mikeAndConquerGame.UpdateMinigunnerPosition(unitPositionChangedEventData);


            }
            else if (anEvent.EventType.Equals(InitializeScenarioEventData.EventName))
            {
                InitializeScenarioEventData initializeScenarioEventData =
                    JsonConvert.DeserializeObject<InitializeScenarioEventData>(anEvent.EventData);

                InitializeScenarioCommand command = new InitializeScenarioCommand(initializeScenarioEventData);
                mikeAndConquerGame.PostCommand(command);

                // mikeAndConquerGame.InitializeScenario(initializeScenarioEventData);
            }
            else if (anEvent.EventType.Equals("ResetScenario"))
            {
                ResetScenarioCommand command = new ResetScenarioCommand();

                mikeAndConquerGame.PostCommand(command);
                // mikeAndConquerGame.ResetScenario();
            }



        }
    }
}
