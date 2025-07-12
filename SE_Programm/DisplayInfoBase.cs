using Sandbox.Game;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRageMath;
using static VRage.Game.MyObjectBuilder_SessionComponentMission;
//using Sandbox.ModAPI;

namespace DisplayInfoBase

{
    public sealed class Program : MyGridProgram
    {


        /// Start of the script
        const String ingotType = "MyObjectBuilder_Ingot/";
        const String componentType = "MyObjectBuilder_Component/";
        const String oreType = "MyObjectBuilder_Ore/";
        const String unknownType = "Unknown";

        const String hydrogenCapacity = "hydrogenCapacity";
        const String hydrogenCurrent = "hydrogenCurrent";
        const String hydrogenPercent  = "hydrogenPercent";
        const String oxygenCapacity = "oxygenCapacity";
        const String oxygenCurrent = "oxygenCurrent";
        const String oxygenPercent = "oxygenPercent";

        Dictionary<String, String> dictionary = new Dictionary<String, String> {
            {"MyObjectBuilder_Ingot/Stone", "Гравий"},
            {"MyObjectBuilder_Ingot/Iron", "Железо"},
            {"MyObjectBuilder_Ingot/Silicon", "Кремний"},
            {"MyObjectBuilder_Ingot/Nickel", "Никель"},
            {"MyObjectBuilder_Ingot/Cobalt", "Кобальт"},
            {"MyObjectBuilder_Ingot/Magnesium", "Магний"},
            {"MyObjectBuilder_Ingot/Silver", "Серебро"},
            {"MyObjectBuilder_Ingot/Gold", "Золото"},
            {"MyObjectBuilder_Ingot/Uranium", "Уран"},

            {"MyObjectBuilder_Ore/Ice", "Лёд"},
            {"MyObjectBuilder_Ore/Stone", "Камень"},
            {"MyObjectBuilder_Ore/Iron", "Железная руда"},
            {"MyObjectBuilder_Ore/Silicon", "Кремневая руда"},
            {"MyObjectBuilder_Ore/Nickel", "Никелевая руда"},
            {"MyObjectBuilder_Ore/Cobalt", "Кобольтовая руда"},
            {"MyObjectBuilder_Ore/Magnesium", "Магниевая руда"},
            {"MyObjectBuilder_Ore/Silver", "Серебряная руда"},
            {"MyObjectBuilder_Ore/Gold", "Золотая руда"},

            {"MyObjectBuilder_Component/SteelPlate", "Стальная пластина"},
            {"MyObjectBuilder_Component/Construction", "Строительные компоненты"},
            {"MyObjectBuilder_Component/Motor", "Мотор"},
            {"MyObjectBuilder_Component/BulletproofGlass", "Бронированное стекло"},
            {"MyObjectBuilder_Component/InteriorPlate", "Внутренняя пластина"},
            {"MyObjectBuilder_Component/Computer", "Компьютер"},
            {"MyObjectBuilder_Component/MetalGrid", "Компонент решетки"},
            {"MyObjectBuilder_Component/LargeTube", "Большая труба"},
            {"MyObjectBuilder_Component/SmallTube", "Маленькая труба"},
            {"MyObjectBuilder_Component/Reactor", "Компонент реактора"},
            {"MyObjectBuilder_Component/Display", "Дисплей"},
            {"MyObjectBuilder_Component/Detector", "Компонент детектора"},
            {"MyObjectBuilder_Component/RadioCommunication", "Радиокомпонент"},
            {"MyObjectBuilder_Component/Girder", "Балка"},
            {"MyObjectBuilder_Component/PowerCell", "Энергоячейка"},
            {"MyObjectBuilder_Component/Medical", "Медицинский компонент"},
        };

        List<IMyTextPanel> textPanels = new List<IMyTextPanel>();

        List<IMyCargoContainer> cargoContainers = new List<IMyCargoContainer>();
        List<IMyAssembler> assemblers = new List<IMyAssembler>();
        List<IMyRefinery> refinerys = new List<IMyRefinery>();
        List<IMyGasGenerator> gasGenerators = new List<IMyGasGenerator>();
        List<IMyShipDrill> drills = new List<IMyShipDrill>();
        List<IMyCockpit> cockpits = new List<IMyCockpit>();

        List<IMyBatteryBlock> batteries = new List<IMyBatteryBlock>();
        List<IMyWindTurbine> turbines = new List<IMyWindTurbine>();
        List<IMyPowerProducer> generators = new List<IMyPowerProducer>();
        List<IMyGasTank> hydrogenTanks = new List<IMyGasTank>();
        List<IMyGasTank> oxygenTanks = new List<IMyGasTank>();
        List<IMyShipConnector> connectors = new List<IMyShipConnector>();

        List<IMyShipController> controllers = new List<IMyShipController>();
        IMyProgrammableBlock program;

        double cargoMass = 0;
        double cargoMassMax = 0;

        double generatorManagerBattareyPercent = 25;
        int generatorManagerIceMinCount = 1000;
        double generatorManagerGasPercent = 25;

        int counter = 1;

        public Program() {
            initialize();
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        void Main(String Argument) {
            Dictionary<String, int> allCargo = getCargo();
            Dictionary<String, Double> gasInfo = getGasInfo();
            var stringItemMap = getCompleteTypesItemCount(allCargo);
            String ingots = stringItemMap[ingotType];
            String components = stringItemMap[componentType];
            String ore = stringItemMap[oreType];
            String unknown = stringItemMap[unknownType];
            double hydrogenPercentValue = gasInfo[hydrogenPercent];
            double hydrogenCurrentValue = gasInfo[hydrogenCurrent];
            double hydrogenCapacityValue = gasInfo[hydrogenCapacity];
            double oxygenPercentValue = gasInfo[oxygenPercent];
            double oxygenCurrentValue = gasInfo[oxygenCurrent];
            double oxygenCapacityValue = gasInfo[oxygenCapacity];
            String generatorInfoString = getGeneratorInfo();
            String turbinesInfoString = getTurbinesInfo();
            String connectorsInfoString = getConnectorInfo();
            var shipMassPair = getShipBaseTotalMass();
            float shipMassBase = shipMassPair.Key;
            float shipCargoMass = shipMassPair.Value - shipMassBase;
            var iceCount = getItemCount(allCargo, "MyObjectBuilder_Ore/Ice");
            KeyValuePair<String, double> batteriesInfo = getBatteriesInfo();
            if (program.CustomData.ToLower().Contains("generatormanager")) {
                manageGenegators(
                    batteriesInfo.Value, 
                    generatorManagerBattareyPercent, 
                    iceCount, 
                    generatorManagerIceMinCount, 
                    hydrogenPercentValue, 
                    generatorManagerGasPercent
                );
            }
            foreach (IMyTextPanel myTextPanel in textPanels) {
                string customData = myTextPanel.CustomData.ToLower();
                StringBuilder output = new StringBuilder();
                List<string> tags = customData.Split('\n').Select(t => t.Trim()).ToList();
                foreach (string tag in tags) {
                    if (tag == "space")
                        output.AppendLine("");
                    else if (tag == "ignots" && ingots != "")
                        output.AppendLine($"-= Слитки =-\n{ingots}");
                    else if (tag == "ores" && ore != "")
                        output.AppendLine($"-= Руда =-\n{ore}");
                    else if (tag == "components" && components != "")
                        output.AppendLine($"-= Компоненты =-\n{components}");
                    else if (tag == "unknown" && unknown != "")
                        output.AppendLine($"-=  Не распознано =-\n{unknown}");
                    else if (tag == "batteries")
                        output.AppendLine(batteriesInfo.Key);
                    else if (tag == "turbines")
                        output.AppendLine(turbinesInfoString);
                    else if (tag == "generators")
                        output.AppendLine(generatorInfoString);
                    else if (tag == "volumecargo")
                        output.AppendLine($"Объём груза: {cargoMass:#,##0}/{cargoMassMax:#,##0}m3");
                    else if (tag == "massship")
                        output.AppendLine($"Масса корабля: {shipMassBase:#,##0}кг");
                    else if (tag == "masscargo")
                        output.AppendLine($"Масса груза: {shipCargoMass:#,##0}кг");
                    else if (tag == "hydrogen")
                        output.AppendLine($"Водород: {hydrogenPercentValue}% ({hydrogenCurrentValue:#,##0}/{hydrogenCapacityValue:#,##0})");
                    else if (tag == "oxygen")
                        output.AppendLine($"Кислород: {oxygenPercentValue}% ({oxygenCurrentValue:#,##0}/{oxygenCapacityValue:#,##0})");
                    else if (tag == "connectors")
                        output.AppendLine(connectorsInfoString);
                }

                if (output.Length > 0) {
                    myTextPanel.WriteText(output.ToString());
                } else {
                    myTextPanel.WriteText(
                        "Введите доступные значения: " +
                        "\n\nignots" +
                        "\nores" +
                        "\ncomponents" +
                        "\nunknown" +
                        "\n\nbatteries" +
                        "\nturbines" +
                        "\ngenerators" +
                        "\nvolumeCargo" +
                        "\nmassShip" +
                        "\nmassCargo" +
                        "\nhydrogen" +
                        "\noxygen" +
                        "\nconnectors" +
                        "\n\nspace" +
                        "\n-fontsize(1.0)" +
                        "\n-align(l/c/r)"
                    );
                }
            }
            runCounter();
        }

        void initialize() {
            program = Me;
            IMyCubeGrid currentGrid = program.CubeGrid;
            GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(cargoContainers, i => i.CubeGrid == currentGrid);
            GridTerminalSystem.GetBlocksOfType<IMyAssembler>(assemblers, i => i.CubeGrid == currentGrid);
            GridTerminalSystem.GetBlocksOfType<IMyRefinery>(refinerys, i => i.CubeGrid == currentGrid);
            GridTerminalSystem.GetBlocksOfType<IMyGasGenerator>(gasGenerators, i => i.CubeGrid == currentGrid);
            GridTerminalSystem.GetBlocksOfType<IMyShipDrill>(drills, i => i.CubeGrid == currentGrid);
            GridTerminalSystem.GetBlocksOfType<IMyCockpit>(cockpits, i => i.CubeGrid == currentGrid);
            GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(batteries, i => i.CubeGrid == currentGrid);
            GridTerminalSystem.GetBlocksOfType<IMyWindTurbine>(turbines, i => i.CubeGrid == currentGrid);
            GridTerminalSystem.GetBlocksOfType<IMyPowerProducer>(generators, i => i.CubeGrid == currentGrid && i.BlockDefinition.SubtypeName.Contains("HydrogenEngine"));
            GridTerminalSystem.GetBlocksOfType<IMyGasTank>(hydrogenTanks, i => i.CubeGrid == currentGrid && i.BlockDefinition.SubtypeId.Contains("Hydrogen"));
            GridTerminalSystem.GetBlocksOfType<IMyGasTank>(oxygenTanks, i => i.CubeGrid == currentGrid && i.BlockDefinition.SubtypeId.Contains("Oxygen"));
            GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(textPanels, textPanel => textPanel.CubeGrid == currentGrid);
            GridTerminalSystem.GetBlocksOfType<IMyShipConnector>(connectors, i => i.CubeGrid == currentGrid);
            GridTerminalSystem.GetBlocksOfType<IMyShipController>(controllers, i => i.CubeGrid == currentGrid);
            applyTextPannelsSettings();
            applyProgrammBlockSettings();
        }

        void runCounter() {
            if (counter < 10) counter++;
            else counter = 1;
        }

        string getAnimate() {
            switch (counter) {
                case 1: return "[|=========]";
                case 2: return "[=|========]";
                case 3: return "[==|=======]";
                case 4: return "[===|======]";
                case 5: return "[====|=====]";
                case 6: return "[=====|====]";
                case 7: return "[======|===]";
                case 8: return "[=======|==]";
                case 9: return "[========|=]";
                case 10: return "[=========|]";
                default: return "[==========]";
            }
        }

        void writeOnPBScreen(string text) {
            var screen = program.GetSurface(0);
            screen.WriteText($"-=DisplayInfoBase=-\n\n{text}");
        }

        void applyProgrammBlockSettings() {
            var screen = program.GetSurface(0);
            screen.ContentType = ContentType.TEXT_AND_IMAGE;
            screen.FontSize = 1.0f;
            screen.FontColor = Color.Green;
            screen.BackgroundColor = Color.Black;
            screen.Alignment = TextAlignment.CENTER;
            string successInit = 
                "Успешная инициализация!" +
                $"\nнайдено {textPanels.Count} текстовых панели!";
            Echo(successInit);
            writeOnPBScreen(successInit + "\n\nДоступны команды:\ngeneratorManager");
        }

        void applyTextPannelsSettings() {
            foreach (IMyTextPanel myTextPanel in textPanels) {
                myTextPanel.ContentType = ContentType.TEXT_AND_IMAGE;
                myTextPanel.FontColor = Color.DarkGreen;
                string customData = myTextPanel.CustomData.ToLower();
                int fontSizeStart = customData.IndexOf("-fontsize(");
                if (fontSizeStart >= 0) {
                    int start = fontSizeStart + 10;
                    int end = customData.IndexOf(")", start);
                    if (end > start) {
                        string sizeValue = customData.Substring(start, end - start);
                        float fontSize = ParseFloat(sizeValue);
                        if (fontSize > 0) {
                            myTextPanel.FontSize = MathHelper.Clamp(fontSize, 0.1f, 10f);
                        }
                    }
                }
                int alignStart = customData.IndexOf("-align(");
                if (alignStart >= 0) {
                    int start = alignStart + 7;
                    int end = customData.IndexOf(")", start);
                    if (end > start) {
                        string alignValue = customData.Substring(start, end - start);
                        if (alignValue == "c") myTextPanel.Alignment = TextAlignment.CENTER;
                        else if (alignValue == "r") myTextPanel.Alignment = TextAlignment.RIGHT;
                        else if (alignValue == "l") myTextPanel.Alignment = TextAlignment.LEFT;
                    }
                }
            }
        }
        
        Dictionary<String, Double> getGasInfo() {
            float hydrogenCapacity = 0;
            Double hydrogenCurrent = 0;
            Double hydrogenPercent = 0;
            float oxygenCapacity = 0;
            Double oxygenCurrent = 0;
            Double oxygenPercent = 0;
            if (hydrogenTanks.Count > 0) {
                foreach (IMyGasTank tank in hydrogenTanks) {
                    hydrogenCapacity += tank.Capacity;
                    hydrogenPercent += tank.FilledRatio;
                    hydrogenCurrent += tank.Capacity * tank.FilledRatio;
                }
            }
            if (oxygenTanks.Count > 0) {
                foreach (IMyGasTank tank in oxygenTanks) {
                    oxygenCapacity += tank.Capacity;
                    oxygenPercent += tank.FilledRatio;
                    oxygenCurrent += tank.Capacity * tank.FilledRatio;
                }
            }
            return new Dictionary<string, double> {
                { Program.hydrogenCapacity, Math.Round(hydrogenCapacity, 1) },
                { Program.hydrogenCurrent, Math.Round(hydrogenCurrent, 1) },
                { Program.hydrogenPercent, Math.Round(hydrogenTanks.Count > 0 ? (hydrogenPercent/hydrogenTanks.Count)*100 : 0, 1) },
                { Program.oxygenCapacity, Math.Round(oxygenCapacity, 1) },
                { Program.oxygenCurrent, Math.Round(oxygenCurrent, 1) },
                { Program.oxygenPercent, Math.Round(oxygenTanks.Count > 0 ? (oxygenPercent/oxygenTanks.Count)*100 : 0, 1) }
            };
        }

        Dictionary<String, int> getCargo() {
            Dictionary<String, int> itemMap = new Dictionary<string, int>();
            float cargoVolume = 0;
            float maxVolume = 0;
            if (cargoContainers.Count > 0) {
                foreach (IMyCargoContainer container in cargoContainers) {
                    IMyInventory inventory = container.GetInventory(0);
                    cargoVolume += inventory.CurrentVolume.RawValue;
                    maxVolume += inventory.MaxVolume.RawValue;
                    collectInventoryItems(inventory, itemMap);
                }
            }
            if (assemblers.Count > 0) {
                foreach (IMyAssembler container in assemblers) {
                    IMyInventory inventory0 = container.GetInventory(0);
                    IMyInventory inventory1 = container.GetInventory(1);
                    cargoVolume += inventory0.CurrentVolume.RawValue;
                    cargoVolume += inventory1.CurrentVolume.RawValue;
                    maxVolume += inventory0.MaxVolume.RawValue;
                    maxVolume += inventory1.MaxVolume.RawValue;
                    collectInventoryItems(inventory0, itemMap);
                    collectInventoryItems(inventory1, itemMap);
                }
            }
            if (refinerys.Count > 0) {
                foreach (IMyRefinery container in refinerys) {
                    IMyInventory inventory0 = container.GetInventory(0);
                    IMyInventory inventory1 = container.GetInventory(1);
                    cargoVolume += inventory0.CurrentVolume.RawValue;
                    cargoVolume += inventory1.CurrentVolume.RawValue;
                    maxVolume += inventory0.MaxVolume.RawValue;
                    maxVolume += inventory1.MaxVolume.RawValue;
                    collectInventoryItems(inventory0, itemMap);
                    collectInventoryItems(inventory1, itemMap);
                }
            }
            if (gasGenerators.Count > 0) {
                foreach (IMyGasGenerator container in gasGenerators) {
                    IMyInventory inventory = container.GetInventory(0);
                    cargoVolume += inventory.CurrentVolume.RawValue;
                    maxVolume += inventory.MaxVolume.RawValue;
                    collectInventoryItems(inventory, itemMap);
                }
            }
            if (drills.Count > 0) {
                foreach (IMyShipDrill container in drills) {
                    IMyInventory inventory = container.GetInventory(0);
                    cargoVolume += inventory.CurrentVolume.RawValue;
                    maxVolume += inventory.MaxVolume.RawValue;
                    collectInventoryItems(inventory, itemMap);
                }
            }
            if (cockpits.Count > 0) {
                foreach (IMyCockpit container in cockpits) {
                    if (container.HasInventory) {
                        IMyInventory inventory = container.GetInventory(0);
                        cargoVolume += inventory.CurrentVolume.RawValue;
                        maxVolume += inventory.MaxVolume.RawValue;
                        collectInventoryItems(inventory, itemMap);
                    }
                    
                }
            }
            cargoMass = cargoVolume;
            cargoMassMax = maxVolume;
            return itemMap;
        }

        void collectInventoryItems(IMyInventory inventory, Dictionary<String, int> itemMap) {
            List<MyInventoryItem> items = new List<MyInventoryItem>();
            inventory.GetItems(items);
            if (items.Count > 0) {
                foreach (MyInventoryItem item in items) {
                    String name = item.Type.ToString();
                    int count = item.Amount.ToIntSafe();
                    if (itemMap.ContainsKey(name)) { itemMap[name] += count; }
                    else { itemMap.Add(name, count); }
                }
            }
        }

        String tryTranslate(string name) => dictionary.ContainsKey(name) ? dictionary[name] : name;

        Dictionary<String, String> getCompleteTypesItemCount(Dictionary<String, int> itemMap) {
            var result = new Dictionary<String, String>();
            result[ingotType] = GetFormattedItems(itemMap, ingotType);
            result[componentType] = GetFormattedItems(itemMap, componentType);
            result[oreType] = GetFormattedItems(itemMap, oreType);
            result[unknownType] = GetUnknownItems(itemMap, new List<String> { ingotType, componentType, oreType });
            return result;
        }

        String GetFormattedItems(Dictionary<String, int> source, String type) {
            var sb = new StringBuilder();
            var keyList = source.Keys.Where(key => key.Contains(type)).ToList();
            keyList.Sort();
            foreach (String key in keyList) {
                String name = tryTranslate(key);
                sb.AppendLine($"{name}: {source[key]}");
            }
            var result = sb.ToString() != "" ? sb.ToString(): "нет в наличии";
            return result;
        }

        String GetUnknownItems(Dictionary<String, int> source, List<String> knownTypes) {
            var sb = new StringBuilder();
            var unknownKeys = source.Keys
                .Where(key => !knownTypes.Any(type => key.Contains(type)))
                .OrderBy(key => key)
                .ToList();
            foreach (String key in unknownKeys) {
                String name = tryTranslate(key);
                sb.AppendLine($"{name}: {source[key]}");
            }
            var result = sb.ToString() != "" ? sb.ToString() : "нет в наличии";
            return result;
        }

        int getItemCount(Dictionary<String, int> itemMap, String type) {
            foreach (var pair in itemMap) {
                if (pair.Key == type) return pair.Value;
            }
            return 0;
        }

        KeyValuePair<String, double> getBatteriesInfo() {
            if (batteries.Count == 0) return new KeyValuePair<String, double>("Батареи не найдены", 0.0);
            double stored = 0;
            double max = 0;
            bool isCharging = false;
            bool isDischarging = false; 
            foreach (IMyBatteryBlock battery in batteries) {
                stored += battery.CurrentStoredPower;
                max += battery.MaxStoredPower; 
                if (battery.CurrentInput > 0) isCharging = true;
                if (battery.CurrentOutput > 0) isDischarging = true;
            } 
            string status = isCharging ? "заряжаются" : isDischarging ? "разряжаются" : "в режиме ожидания";
            double percent = Math.Round(stored / max * 100, 1);
            return new KeyValuePair<String, double>($"Баттареи {status}: {percent}%", percent);
        }

        String getTurbinesInfo() {
            if (turbines.Count == 0) return "Ветряные турбины не найдены";
            float totalOutput = 0f;
            int workingTurbines = 0;
            foreach (var turbine in turbines) {
                totalOutput += turbine.CurrentOutput;
                if (turbine.IsWorking) workingTurbines++;
            }
            return $"Турбины: {workingTurbines}/{turbines.Count} работают ({Math.Round(totalOutput, 2)} МВт)";
        }

        String getGeneratorInfo() {
            if (generators.Count == 0) return "Водородные генераторы не найдены";
            float currentOutput = 0f;
            float maxOutput = 0f;
            int workingGenerators = 0;
            foreach (var gen in generators) {
                maxOutput += gen.MaxOutput;
                currentOutput += gen.CurrentOutput;
                if (gen.IsWorking) workingGenerators++;
            }
            if (workingGenerators == 0) return "Генераторы не работают";
            return $"Генераторы: {workingGenerators}/{generators.Count} работают ({Math.Round(currentOutput, 2)} МВт)";
        }

        String getConnectorInfo() {
            var sb = new StringBuilder();
            if (connectors.Count == 0) return "Нет коннекторов!";
            foreach (var connector in connectors) {
                if (connector.Status == MyShipConnectorStatus.Connected) { 
                    IMyShipConnector otherConnector = connector.OtherConnector;
                    IMyCubeGrid connectedGrid = otherConnector.CubeGrid;
                    sb.AppendLine($"'{connector.CustomName}' -> {connectedGrid.DisplayName}");
                } else {
                    sb.AppendLine($"'{connector.CustomName}' свободен");
                }
            }
            return sb.ToString();
        }

        KeyValuePair<float, float> getShipBaseTotalMass() {
            var result = new KeyValuePair<float, float>();
            if (controllers.Count == 0) return result;
            IMyShipController controller = controllers[0];
            var mass = controller.CalculateShipMass();
            result = new KeyValuePair<float, float>(mass.BaseMass, mass.TotalMass);
            return result;
        }

        void manageGenegators(double batteriesPercent, double enableOn, int iceCount, int iceMinValue, double gasPercent, double gasMinValue) {
            if (generators.Count == 0) {
                writeOnPBScreen("Генераторы не найдены");
                return;
            }
            Boolean enable = batteriesPercent <= enableOn && iceCount > iceMinValue && gasPercent > gasMinValue;
            string status = enable ? "зарядка" : "простой";
            writeOnPBScreen(
                    $"GeneratorManager: {status}" +
                    $"\n{getAnimate()}" +
                    "\n\nУсловия для работы:" +
                    $"\nЗарядка батарей <= {generatorManagerBattareyPercent}%" +
                    $"\nЛёд > {generatorManagerIceMinCount}" +
                    $"\nВодород > {generatorManagerGasPercent}%"
                );
            foreach (var gen in generators) {
                gen.Enabled = enable;
            }
        }

        float ParseFloat(string value) {
            float result = 0;
            try {
                result = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            } catch {
                Echo("ParseFloat fail");
            }
            return result;
        }
        /// End of the script

    }
}