using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Logging;
using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.commands.mapper;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace mike_and_conquer_monogame.main
{
    public class UpdateUnitViewPositionWhenUnitPositionChangedEventHandler : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public UpdateUnitViewPositionWhenUnitPositionChangedEventHandler(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {

            if (anEvent.EventType.Equals(UnitPositionChangedEventData.EventType))
            {
                UnitPositionChangedEventData unitPositionChangedEventData =
                    JsonConvert.DeserializeObject<UnitPositionChangedEventData>(anEvent.EventData);

                UpdateUnitViewPositionCommand command = new UpdateUnitViewPositionCommand(unitPositionChangedEventData);
                mikeAndConquerGame.PostCommand(command);

            }

        }
    }
}
