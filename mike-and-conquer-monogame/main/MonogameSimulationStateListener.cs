using Microsoft.Extensions.Logging;
using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.commands.mapper;
using mike_and_conquer_simulation.events;
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
            if (anEvent.EventType.Equals(JeepCreateEventData.EventType))
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
            else if (anEvent.EventType.Equals(UnitMovementPlanCreatedEventData.EventType))
            {

                HandleUnitMovementPlanCreatedCommand command =
                    HandleUnitMovementPlanCreatedCommandMapper.ConvertToCommand(anEvent.EventData);

                mikeAndConquerGame.PostCommand(command);
            }
            else if (anEvent.EventType.Equals(UnitArrivedAtPathStepEventData.EventType))
            {
                UnitArrivedAtPathStepEventData eventData =
                    JsonConvert.DeserializeObject<UnitArrivedAtPathStepEventData>(anEvent.EventData);

                
                HandleUnitArrivedAtPathStepCommand command = new HandleUnitArrivedAtPathStepCommand(eventData.UnitId, eventData.PathStep);
                mikeAndConquerGame.PostCommand(command);

            }
            else
            {
                MikeAndConquerGame.instance.logger.LogInformation("Ignored event:" + anEvent.EventType);
            }



        }
    }
}
