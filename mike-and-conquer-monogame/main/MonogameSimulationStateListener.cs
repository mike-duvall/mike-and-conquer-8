using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Logging;
using mike_and_conquer_monogame.commands;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            if (anEvent.EventType.Equals(MinigunnerCreateEventData.EventType))
            {
                MinigunnerCreateEventData eventData =
                    JsonConvert.DeserializeObject<MinigunnerCreateEventData>(anEvent.EventData);

                AddMinigunnerCommand command = new AddMinigunnerCommand(eventData.UnitId, eventData.X, eventData.Y);

                mikeAndConquerGame.PostCommand(command);
                
            }
            else if (anEvent.EventType.Equals(JeepCreateEventData.EventType))
            {
                JeepCreateEventData eventData =
                    JsonConvert.DeserializeObject<JeepCreateEventData>(anEvent.EventData);

                AddJeepCommand command = new AddJeepCommand(eventData.UnitId, eventData.X, eventData.Y);

                mikeAndConquerGame.PostCommand(command);

            }
            else if (anEvent.EventType.Equals(MCVCreateEventData.EventType))
            {
                MCVCreateEventData eventData =
                    JsonConvert.DeserializeObject<MCVCreateEventData>(anEvent.EventData);

                AddMCVCommand command = new AddMCVCommand(eventData.UnitId, eventData.X, eventData.Y);

                mikeAndConquerGame.PostCommand(command);

            }

            else if (anEvent.EventType.Equals(UnitPositionChangedEventData.EventType))
            {
                UnitPositionChangedEventData unitPositionChangedEventData =
                    JsonConvert.DeserializeObject<UnitPositionChangedEventData>(anEvent.EventData);

                UpdateUnitPositionCommand command = new UpdateUnitPositionCommand(unitPositionChangedEventData);
                mikeAndConquerGame.PostCommand(command);

            }
            else if (anEvent.EventType.Equals(InitializeScenarioEventData.EventType))
            {
                InitializeScenarioEventData initializeScenarioEventData =
                    JsonConvert.DeserializeObject<InitializeScenarioEventData>(anEvent.EventData);

                InitializeScenarioCommand command = new InitializeScenarioCommand(initializeScenarioEventData);
                mikeAndConquerGame.PostCommand(command);

            }
            else if (anEvent.EventType.Equals(UnitMovementPlanCreatedEventData.EventType))
            {
                // Pickup here
                // When received path planning event, mark the appropriate MapTileInstanceViews as partOfPath = true


                UnitMovementPlanCreatedEventData eventData =
                     JsonConvert.DeserializeObject<UnitMovementPlanCreatedEventData>(anEvent.EventData);


                // var rawEventData =
                //     JsonConvert.DeserializeObject(anEvent.EventData);
                //
                // var pathSteps = rawEventData["PathSteps"];

                List<PathStep> pathStepList = new List<PathStep>();


                var jsonObject = JObject.Parse(anEvent.EventData);
                var pathSteps = jsonObject["PathSteps"];

                foreach(JObject jObject in pathSteps)
                {
                    int x = jObject.GetValue("X").ToObject<int>();
                    int y = jObject.GetValue("Y").ToObject<int>();
                    PathStep pathStep = new PathStep(x, y);
                    pathStepList.Add(pathStep);
                }

                eventData.PathSteps = pathStepList;

                HandleUnitMovementPlanCreatedCommand command = new HandleUnitMovementPlanCreatedCommand(eventData);
                mikeAndConquerGame.PostCommand(command);
            }
            else if (anEvent.EventType.Equals(UnitArrivedAtPathStepEventData.EventType))
            {
                UnitArrivedAtPathStepEventData eventData =
                    JsonConvert.DeserializeObject<UnitArrivedAtPathStepEventData>(anEvent.EventData);

                
                HandleUnitArrivedAtPathStepCommand command = new HandleUnitArrivedAtPathStepCommand(eventData);
                mikeAndConquerGame.PostCommand(command);

            }
            else
            {
                MikeAndConquerGame.instance.logger.LogInformation("Ignored event:" + anEvent.EventType);
            }



        }
    }
}
