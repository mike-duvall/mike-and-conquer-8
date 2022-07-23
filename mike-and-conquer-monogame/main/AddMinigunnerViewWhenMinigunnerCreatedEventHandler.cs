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
    public class AddMinigunnerViewWhenMinigunnerCreatedEventHandler : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public AddMinigunnerViewWhenMinigunnerCreatedEventHandler(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            if (anEvent.EventType.Equals(MinigunnerCreateEventData.EventType))
            {
                MinigunnerCreateEventData eventData =
                    JsonConvert.DeserializeObject<MinigunnerCreateEventData>(anEvent.EventData);

                AddMinigunnerViewCommand command = new AddMinigunnerViewCommand(eventData.UnitId, eventData.X, eventData.Y);

                mikeAndConquerGame.PostCommand(command);
                
            }


        }
    }
}
