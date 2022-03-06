
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using mike_and_conquer_simulation.commands;
using mike_and_conquer_simulation.commands.commandbody;
using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.gameworld;
using Newtonsoft.Json;

namespace mike_and_conquer_simulation.main
{
    public class SimulationMain
    {

        public static int globalId = 1;


        private SimulationOptions simulationOptions;

        private Queue<AsyncSimulationCommand> inputCommandQueue;

        private List<SimulationStateUpdateEvent> simulationStateUpdateEventsHistory;

        private List<SimulationStateListener> listeners;

        public static ILoggerFactory loggerFactory;

        private static ILogger logger;


        public static SimulationMain instance;

        public static ManualResetEvent condition;

        // private List<Unit> unitList;

        private GameWorld gameWorld;

        public static void StartSimulation(SimulationStateListener listener)
        {



            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: false)
                .Build();

            var loggingConfig = configuration.GetSection("Logging");

            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddDebug()
                    .AddConsole()
                    .AddConfiguration(loggingConfig)
                    ;
            });


            logger = loggerFactory.CreateLogger<SimulationMain>();
            logger.LogInformation("************************Simulation Mike is cool");
            logger.LogWarning("************************Simulation Mike is cool");


            new SimulationMain();
            SimulationMain.instance.AddListener(listener);

            Pickup here
            Get this code working

            PlayerController playerController = new MonogameUIHumanPlayerController();
            SimulationMain.instance.SetGDIPlayerController(playerController);

            condition = new ManualResetEvent(false);
            Thread backgroundThread = new Thread(new ThreadStart(SimulationMain.Main));
            backgroundThread.IsBackground = true;

            // Start thread  
            backgroundThread.Start();
            condition.WaitOne();

            // EmitInitializeScenarioEvent(27,23);


        }

        private static void EmitInitializeScenarioEvent(
            int mapWidth,
            int mapHeight,
            List<MapTileInstance> mapTileInstanceList,
            List<TerrainItem> terrainItemList)
        {
            SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
            simulationStateUpdateEvent.EventType = InitializeScenarioEventData.EventName;
            InitializeScenarioEventData eventData = new InitializeScenarioEventData();


            List<MapTileInstanceCreateEventData> mapTileInstanceCreateEventDataList =
                new List<MapTileInstanceCreateEventData>();

            foreach (MapTileInstance mapTileInstance in mapTileInstanceList)
            {
                MapTileInstanceCreateEventData mapTileCreateEventData = new MapTileInstanceCreateEventData(
                    mapTileInstance.ID,
                    mapTileInstance.MapTileLocation.XInWorldMapTileCoordinates,
                    mapTileInstance.MapTileLocation.YInWorldMapTileCoordinates,
                    mapTileInstance.TextureKey,
                    mapTileInstance.ImageIndex,
                    mapTileInstance.IsBlockingTerrain,
                    mapTileInstance.Visibility.ToString()
                    );

                mapTileInstanceCreateEventDataList.Add(mapTileCreateEventData);
            }

            eventData.MapTileInstanceCreateEventDataList = mapTileInstanceCreateEventDataList;

            List<TerrainItemCreateEventData> terrainItemCreateEventDataList =
                new List<TerrainItemCreateEventData>();

            foreach (TerrainItem terrainItem in terrainItemList)
            {
                TerrainItemCreateEventData terrainItemCreateEventData = new TerrainItemCreateEventData(
                    terrainItem.MapTileLocation.XInWorldMapTileCoordinates,
                    terrainItem.MapTileLocation.YInWorldMapTileCoordinates,
                    terrainItem.TerrainItemType);
                terrainItemCreateEventDataList.Add(terrainItemCreateEventData);

            }

            eventData.TerrainItemCreateEventDataList = terrainItemCreateEventDataList;

            eventData.MapWidth = mapWidth;
            eventData.MapHeight = mapHeight;
            

            simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);
            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);

        }


        public void AddListener(SimulationStateListener listener)
        {
            listeners.Add(listener);
        }


        public static void Main()
        {
            SimulationMain.condition.Set();
            long previousTicks = 0;
            SimulationMain.instance.SetGameSpeed(SimulationOptions.GameSpeed.Normal);

            while (true)
            {


                int sleepTime = (int) SimulationMain.instance.simulationOptions.CurrentGameSpeed;
                TimerHelper.SleepForNoMoreThan(sleepTime);


                SimulationMain.instance.Tick();

                bool doneWaiting = false;
                long delta = -1;
                long currentTicks = -1;

                while (!doneWaiting)
                {
                
                     currentTicks = DateTime.Now.Ticks;
                     delta = (currentTicks - previousTicks) / TimeSpan.TicksPerMillisecond;
                     if (delta >= sleepTime)
                     {
                         doneWaiting = true;
                     }


                }
                // logger.LogInformation("delta=" + delta);
                previousTicks = currentTicks;
            }

        }


        SimulationMain()
        {
            inputCommandQueue = new Queue<AsyncSimulationCommand>();
            simulationStateUpdateEventsHistory = new List<SimulationStateUpdateEvent>();
            listeners = new List<SimulationStateListener>();
            listeners.Add(new SimulationStateHistoryListener(this));

            // unitList = new List<Unit>();

            simulationOptions = new SimulationOptions();

            SimulationMain.instance = this;
        }

        private void Tick()
        {
            Update();
            ProcessInputEventQueue();
        }

        private void Update()
        {
            // foreach (Unit unit in unitList)
            // {
            //     unit.Update();
            // }
            gameWorld.Update();

        }

        private void ProcessInputEventQueue()
        {
            lock (inputCommandQueue)
            {
                while (inputCommandQueue.Count > 0)
                {
                    AsyncSimulationCommand anEvent = inputCommandQueue.Dequeue();
                    anEvent.Process();
                }
            }
        }

        internal void PostSetGameSpeedCommand(SimulationOptions.GameSpeed aGameSpeed)
        {
            SetGameSpeedCommand aCommand = new SetGameSpeedCommand();
            aCommand.GameSpeed = aGameSpeed;

            lock (inputCommandQueue)
            {
                inputCommandQueue.Enqueue(aCommand);
            }

        }

        public List<SimulationStateUpdateEvent> GetCopyOfEventHistoryViaEvent()
        {
            GetCopyOfEventHistoryCommand anEvent = new GetCopyOfEventHistoryCommand();

            lock (inputCommandQueue)
            {
                inputCommandQueue.Enqueue(anEvent);
            }

            List<SimulationStateUpdateEvent> list = anEvent.GetCopyOfEventHistory();
            return list;
        }

        // internal Minigunner CreateMinigunner(int x, int y)
        // {
        //
        //     Minigunner minigunner = new Minigunner();
        //     minigunner.GameWorldLocation.X = x;
        //     minigunner.GameWorldLocation.Y = y;
        //     unitList.Add(minigunner);
        //
        //     SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
        //     simulationStateUpdateEvent.EventType = MinigunnerCreateEventData.EventName;
        //     MinigunnerCreateEventData eventData = new MinigunnerCreateEventData();
        //     eventData.ID = minigunner.ID;
        //     eventData.X = x;
        //     eventData.Y = y;
        //
        //     simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);
        //
        //     foreach (SimulationStateListener listener in listeners)
        //     {
        //         listener.Update(simulationStateUpdateEvent);
        //     }
        //
        //     return minigunner;
        // }

        internal Minigunner CreateMinigunner(int xInWorldCoordinates, int yInWorldCoordinates)
        {

            return gameWorld.CreateMinigunner(xInWorldCoordinates, yInWorldCoordinates);

            // Minigunner minigunner = new Minigunner();
            // minigunner.GameWorldLocation.X = x;
            // minigunner.GameWorldLocation.Y = y;
            // unitList.Add(minigunner);
            //
            // SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
            // simulationStateUpdateEvent.EventType = MinigunnerCreateEventData.EventName;
            // MinigunnerCreateEventData eventData = new MinigunnerCreateEventData();
            // eventData.ID = minigunner.ID;
            // eventData.X = x;
            // eventData.Y = y;
            //
            // simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);
            //
            // foreach (SimulationStateListener listener in listeners)
            // {
            //     listener.Update(simulationStateUpdateEvent);
            // }
            //
            // return minigunner;
        }


        internal Jeep CreateJeep(int xInWorldCoordinates, int yInWorldCoordinates)
        {

            return gameWorld.CreateJeep(xInWorldCoordinates, yInWorldCoordinates);

            // Jeep jeep = new Jeep();
            // jeep.GameWorldLocation.X = x;
            // jeep.GameWorldLocation.Y = y;
            // unitList.Add(jeep);
            //
            // SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
            // simulationStateUpdateEvent.EventType = JeepCreateEventData.EventName;
            // JeepCreateEventData eventData = new JeepCreateEventData();
            // eventData.ID = jeep.ID;
            // eventData.X = x;
            // eventData.Y = y;
            //
            // simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);
            //
            // foreach (SimulationStateListener listener in listeners)
            // {
            //     listener.Update(simulationStateUpdateEvent);
            // }
            //
            // return jeep;
        }

        internal MCV CreateMCV(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            return gameWorld.CreateMCV(xInWorldCoordinates, yInWorldCoordinates);

            // MCV mcv = new MCV();
            // mcv.GameWorldLocation.X = x;
            // mcv.GameWorldLocation.Y = y;
            // unitList.Add(mcv);
            //
            // SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
            // simulationStateUpdateEvent.EventType = MCVCreateEventData.EventName;
            // MCVCreateEventData eventData = new MCVCreateEventData();
            // eventData.ID = mcv.ID;
            // eventData.X = x;
            // eventData.Y = y;
            //
            // simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);
            //
            // foreach (SimulationStateListener listener in listeners)
            // {
            //     listener.Update(simulationStateUpdateEvent);
            // }
            //
            // return mcv;
        }



        public void OrderUnitToMove(int unitId, int destinationXInWorldCoordinates, int destinationYInWorldCoordinates)
        {

            Unit foundUnit = FindUnitWithUnitId(unitId);
            foundUnit.OrderMoveToDestination(destinationXInWorldCoordinates, destinationYInWorldCoordinates);

            SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
            simulationStateUpdateEvent.EventType = UnitMoveOrderEventData.EventName;
            UnitMoveOrderEventData eventData = new UnitMoveOrderEventData();
            eventData.ID = unitId;
            eventData.DestinationXInWorldCoordinates = destinationXInWorldCoordinates;
            eventData.DestinationYInWorldCoordinates = destinationYInWorldCoordinates;
            eventData.Timestamp = DateTime.Now.Ticks;

            simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);

            foreach (SimulationStateListener listener in listeners)
            {
                listener.Update(simulationStateUpdateEvent);
            }

        }

        private Unit FindUnitWithUnitId(int unitId)
        {
            // Unit foundUnit = null;
            //
            // foreach (Unit unit in unitList)
            // {
            //     if (unit.ID == unitId)
            //     {
            //         foundUnit = unit;
            //     }
            // }
            //
            // return foundUnit;

            return gameWorld.FindUnitWithUnitId(unitId);
        }


        public List<SimulationStateUpdateEvent> GetCopyOfEventHistory()
        {
            List<SimulationStateUpdateEvent> copyList = new List<SimulationStateUpdateEvent>();

            foreach (SimulationStateUpdateEvent simulationStateUpdateEvent in simulationStateUpdateEventsHistory)
            {
                SimulationStateUpdateEvent copyEvent = new SimulationStateUpdateEvent();
                copyEvent.EventType = simulationStateUpdateEvent.EventType;
                String copyEventData = new string(simulationStateUpdateEvent.EventData);

                copyEvent.EventData = copyEventData;
                copyList.Add(copyEvent);
            }

            return copyList;
        }


        public void AddHistoryEvent(SimulationStateUpdateEvent anEvent)
        {
            simulationStateUpdateEventsHistory.Add(anEvent); 
        }

        public void PublishEvent(SimulationStateUpdateEvent simulationStateUpdateEvent)
        {
            foreach (SimulationStateListener listener in listeners)
            {
                listener.Update(simulationStateUpdateEvent);
            }
        }

        internal void SetGameSpeed(SimulationOptions.GameSpeed aGameSpeed)
        {
            this.simulationOptions.CurrentGameSpeed = aGameSpeed;
        }


        public void ResetScenario()
        {
            SimulationStateUpdateEvent resetScenarioEvent = new SimulationStateUpdateEvent();
            resetScenarioEvent.EventType = "ResetScenario";

            PublishEvent(resetScenarioEvent);

            lock (simulationStateUpdateEventsHistory)
            {
                simulationStateUpdateEventsHistory.Clear();
            }

            SimulationMain.globalId = 1;

            // gam
            // lock (unitList)
            // {
            //     unitList.Clear();
            // }

            gameWorld = new GameWorld();
            gameWorld.InitializeDefaultMap();

            EmitInitializeScenarioEvent(27, 23, gameWorld.gameMap.MapTileInstanceList, gameWorld.terrainItemList);

            // EmitInitializeScenarioEvent(27, 23);
        }

        internal void PostCommand(RawCommand incomingAdminCommand)
        {

            AsyncSimulationCommand command = ConvertRawCommand(incomingAdminCommand);

            lock (inputCommandQueue)
            {
                inputCommandQueue.Enqueue(command);
            }


        }

        AsyncSimulationCommand ConvertRawCommand(RawCommand rawCommand)
        {
            if (rawCommand.CommandType.Equals(CreateMinigunnerCommand.CommandName))
            {

                CreateMinigunnerCommandBody commandBody =
                    JsonConvert.DeserializeObject<CreateMinigunnerCommandBody>(rawCommand.CommandData);

                CreateMinigunnerCommand createUnit = new CreateMinigunnerCommand();
                createUnit.X = commandBody.StartLocationXInWorldCoordinates;
                createUnit.Y = commandBody.StartLocationYInWorldCoordinates;

                return createUnit;

            }
            else if (rawCommand.CommandType.Equals(CreateJeepCommand.CommandName))
            {

                CreateJeepCommandBody commandBody =
                    JsonConvert.DeserializeObject<CreateJeepCommandBody>(rawCommand.CommandData);

                CreateJeepCommand createdUnit = new CreateJeepCommand();
                createdUnit.X = commandBody.StartLocationXInWorldCoordinates;
                createdUnit.Y = commandBody.StartLocationYInWorldCoordinates;

                return createdUnit;

            }
            else if (rawCommand.CommandType.Equals(CreateMCVCommand.CommandName))
            {

                CreateMCVCommandBody commandBody =
                    JsonConvert.DeserializeObject<CreateMCVCommandBody>(rawCommand.CommandData);

                CreateMCVCommand createdUnit = new CreateMCVCommand();
                createdUnit.X = commandBody.StartLocationXInWorldCoordinates;
                createdUnit.Y = commandBody.StartLocationYInWorldCoordinates;

                return createdUnit;

            }

            else if (rawCommand.CommandType.Equals(ResetScenarioCommand.CommandName))
            {

                return new ResetScenarioCommand();

            }

            else if (rawCommand.CommandType.Equals(OrderUnitToMoveCommand.CommandName))
            {

                OrderUnitMoveCommandBody commandBody =
                    JsonConvert.DeserializeObject<OrderUnitMoveCommandBody>(rawCommand.CommandData);

                OrderUnitToMoveCommand anEvent = new OrderUnitToMoveCommand();
                anEvent.UnitId = commandBody.UnitId;
                anEvent.DestinationXInWorldCoordinates = commandBody.DestinationLocationXInWorldCoordinates;
                anEvent.DestinationYInWorldCoordinates = commandBody.DestinationLocationYInWorldCoordinates;

                return anEvent;

            }
            else if (rawCommand.CommandType.Equals(SetGameSpeedCommand.CommandName))
            {
                SetSimulationOptionsCommandBody commandBody =
                    JsonConvert.DeserializeObject<SetSimulationOptionsCommandBody>(rawCommand.CommandData);

                SimulationOptions.GameSpeed inputGameSpeed = ConvertGameSpeedStringToEnum(commandBody.GameSpeed);
                SetGameSpeedCommand aCommand = new SetGameSpeedCommand();
                aCommand.GameSpeed = inputGameSpeed;

                return aCommand;

            }
            else
            {
                throw new Exception("Unknown CommandType:" + rawCommand.CommandType);
            }


        }

        private SimulationOptions.GameSpeed ConvertGameSpeedStringToEnum(String gameSpeedAsString)
        {
            if (gameSpeedAsString == "Slowest") return SimulationOptions.GameSpeed.Slowest;
            if (gameSpeedAsString == "Slower") return SimulationOptions.GameSpeed.Slower;
            if (gameSpeedAsString == "Slow") return SimulationOptions.GameSpeed.Slow;
            if (gameSpeedAsString == "Moderate") return SimulationOptions.GameSpeed.Moderate;
            if (gameSpeedAsString == "Normal") return SimulationOptions.GameSpeed.Normal;
            if (gameSpeedAsString == "Fast") return SimulationOptions.GameSpeed.Fast;
            if (gameSpeedAsString == "Faster") return SimulationOptions.GameSpeed.Faster;
            if (gameSpeedAsString == "Fastest") return SimulationOptions.GameSpeed.Fastest;

            throw new Exception("Could not map game speed string of:" + gameSpeedAsString);
        }

    }
}
