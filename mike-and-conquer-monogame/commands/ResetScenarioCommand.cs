using Microsoft.VisualBasic;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.commands;
using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class ResetScenarioCommand : AsyncViewCommand
    {




        protected override void ProcessImpl()

        {

            MikeAndConquerGame.instance.ResetScenario();

        }
    }
}
