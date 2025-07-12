using Sandbox.Game;
using Sandbox.Game.Entities.Blocks;
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
using VRage.Library.Collections;
using VRage.ModAPI;
using VRageMath;
using static VRage.Game.MyObjectBuilder_SessionComponentMission;
//using Sandbox.ModAPI;

namespace DisplayInfoBase

{
    public sealed class Program : MyGridProgram
    {


        /// Start of the script
        const string pbGeneratorTemplate = "generatorManager(Bat,Ice,H2)";
        const string ingotType = "MyObjectBuilder_Ingot/";
        const string componentType = "MyObjectBuilder_Component/";
        const string oreType = "MyObjectBuilder_Ore/";
        const string physycalType = "MyObjectBuilder_PhysicalObject/";
        const string gunType = "MyObjectBuilder_PhysicalGunObject/";
        const string ammoType = "MyObjectBuilder_AmmoMagazine/";
        const string oxygenType = "MyObjectBuilder_OxygenContainerObject/";
        const string unknownType = "Unknown";
        const string userKeyGeneratorManager = "generatormanager(";
        const string userKeySpacer = "space";
        const string userKeyTextAlign = "-align(";
        const string userKeyFontSize = "-fontsize(";
        const string userKeyIgnots = "ignots";
        const string userKeyOres = "ores";
        const string userKeyComponents = "components";
        const string userKeyPhysicals = "physicals";
        const string userKeyAmmos = "ammos";
        const string userKeyGuns = "guns";
        const string userKeyOxygenBottle = "oxygenbottle";
        const string userKeyUnknown = "nunknown";
        const string userKeyBatteries = "batteries";
        const string userKeyTurbines = "turbines";
        const string userKeyGenerators = "generators";
        const string userKeyVolumeCargo = "volumecargo";
        const string userKeyMassShip = "massship";
        const string userKeyMassCargo = "masscargo";
        const string userKeyHydrogen = "hydrogen";
        const string userKeyOxygen = "oxygen";
        const string userKeyConnectors = "connectors";
        const string userKeyReactors = "reactors";


        string textPannelsComands = "Введите доступные значения: " +
                        "\n\n-= Инвентарь =-" +
                        $"\n{userKeyIgnots} | {userKeyOres} | {userKeyComponents}" +
                        $"\n{userKeyPhysicals} | {userKeyAmmos} | {userKeyGuns}" +
                        $"\n{userKeyOxygenBottle} | {userKeyUnknown}" +
                        "\n\n-= Блоки =-" +
                        $"\n{userKeyBatteries} | {userKeyTurbines} | {userKeyGenerators}" +
                        $"\n{userKeyVolumeCargo} | {userKeyMassShip} | {userKeyMassCargo}" +
                        $"\n{userKeyHydrogen} | {userKeyOxygen} | {userKeyConnectors}" +
                        $"\n{userKeyReactors}" +
                        "\n\n-= Форматирование =-" +
                        $"\n{userKeySpacer} | {userKeyFontSize}1.0) | {userKeyTextAlign}l/c/r)";

        const string hydrogenCapacity = "hydrogenCapacity";
        const string hydrogenCurrent = "hydrogenCurrent";
        const string hydrogenPercent  = "hydrogenPercent";
        const string oxygenCapacity = "oxygenCapacity";
        const string oxygenCurrent = "oxygenCurrent";
        const string oxygenPercent = "oxygenPercent";

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
            {"MyObjectBuilder_Ingot/Platinum", "Платина"},

            {"MyObjectBuilder_Ore/Ice", "Лёд"},
            {"MyObjectBuilder_Ore/Scrap", "Металлолом"},
            {"MyObjectBuilder_Ore/Stone", "Камень"},
            {"MyObjectBuilder_Ore/Iron", "Железная руда"},
            {"MyObjectBuilder_Ore/Silicon", "Кремневая руда"},
            {"MyObjectBuilder_Ore/Nickel", "Никелевая руда"},
            {"MyObjectBuilder_Ore/Cobalt", "Кобольтовая руда"},
            {"MyObjectBuilder_Ore/Magnesium", "Магниевая руда"},
            {"MyObjectBuilder_Ore/Silver", "Серебряная руда"},
            {"MyObjectBuilder_Ore/Gold", "Золотая руда"},
            {"MyObjectBuilder_Ore/Uranium", "Урановая руда"},
            {"MyObjectBuilder_Ore/Platinum", "Платиновая руда"},

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
            //
            {"MyObjectBuilder_AmmoMagazine/RapidFireAutomaticRifleGun_Mag_50rd", "Магазин винтовки MR-50A"},
            {"MyObjectBuilder_OxygenContainerObject/OxygenBottle", "Кислородный баллон"},
            {"MyObjectBuilder_PhysicalGunObject/RapidFireAutomaticRifleItem", "Винтовка MR-50A"},
            {"MyObjectBuilder_PhysicalObject/SpaceCredit", "Космо-кредиты"},
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

        List<IMyReactor> reactors = new List<IMyReactor>();

        List<IMyShipController> controllers = new List<IMyShipController>();
        IMyProgrammableBlock program;
        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;

        double cargoMass = 0;
        double cargoMassMax = 0;

        double generatorManagerBattareyPercent = 25;
        int generatorManagerIceMinCount = 2500;
        double generatorManagerGasPercent = 25;

        int counter = 1;
        int retryAttempt = 3;

        public Program() {
            initialize();
        }

        void Main(String Argument) {
            Dictionary<String, int> allCargo = getCargo();
            Dictionary<String, Double> gasInfo = getGasInfo();
            var stringItemMap = getCompleteTypesItemCount(allCargo);
            String ingots = stringItemMap[ingotType];
            String components = stringItemMap[componentType];
            String ore = stringItemMap[oreType];
            string physycal = stringItemMap[physycalType];
            string guns = stringItemMap[gunType];
            string ammo = stringItemMap[ammoType];
            string oxygenBottle = stringItemMap[oxygenType];
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
            String reactorsInfoString = getReactorsInfo();
            var shipMassPair = getShipBaseTotalMass();
            float shipMassBase = shipMassPair.Key;
            float shipCargoMass = shipMassPair.Value - shipMassBase;
            var iceCount = getItemCount(allCargo, "MyObjectBuilder_Ore/Ice");
            KeyValuePair<String, double> batteriesInfo = getBatteriesInfo();
            if (program.CustomData.ToLower().Contains(userKeyGeneratorManager)) {
                manageGenegators(
                    batteriesInfo.Value,
                    iceCount,
                    hydrogenPercentValue
                );
            } else {
                writeOnPBScreen($"\n\nДоступны команды:\n{pbGeneratorTemplate}");
            }
            foreach (IMyTextPanel myTextPanel in textPanels) {
                string customData = myTextPanel.CustomData.ToLower();
                applyTextPannelSettings(myTextPanel);
                StringBuilder output = new StringBuilder();
                List<string> tags = customData.Split('\n').Select(t => t.Trim()).ToList();
                foreach (string tag in tags) {
                    if (tag == userKeySpacer) output.AppendLine("");
                    else if (tag == userKeyIgnots && ingots != "") output.AppendLine($"-= Слитки =-{ingots}");
                    else if (tag == userKeyOres && ore != "") output.AppendLine($"-= Руда =-{ore}");
                    else if (tag == userKeyComponents && components != "") output.AppendLine($"-= Компоненты =-{components}");
                    else if (tag == userKeyPhysicals && physycal != "") output.Append(physycal);
                    else if (tag == userKeyAmmos && ammo != "") output.Append(ammo);
                    else if (tag == userKeyGuns && guns != "") output.Append(guns);
                    else if (tag == userKeyOxygenBottle && oxygenBottle != "") output.Append(oxygenBottle);
                    else if (tag == userKeyUnknown && unknown != "") output.Append(unknown);
                    else if (tag == userKeyBatteries) output.AppendLine(batteriesInfo.Key);
                    else if (tag == userKeyTurbines) output.AppendLine(turbinesInfoString);
                    else if (tag == userKeyGenerators) output.AppendLine(generatorInfoString);
                    else if (tag == userKeyVolumeCargo) output.AppendLine($"Объём груза: {cargoMass:#,##0}/{cargoMassMax:#,##0}m3");
                    else if (tag == userKeyMassShip)  output.AppendLine($"Масса корабля: {shipMassBase:#,##0}кг");
                    else if (tag == userKeyMassCargo) output.AppendLine($"Масса груза: {shipCargoMass:#,##0}кг");
                    else if (tag == userKeyHydrogen) output.AppendLine($"Водород: {hydrogenPercentValue}% ({hydrogenCurrentValue:#,##0}/{hydrogenCapacityValue:#,##0})");
                    else if (tag == userKeyOxygen) output.AppendLine($"Кислород: {oxygenPercentValue}% ({oxygenCurrentValue:#,##0}/{oxygenCapacityValue:#,##0})");
                    else if (tag == userKeyConnectors) output.AppendLine(connectorsInfoString);
                    else if (tag == userKeyReactors) output.AppendLine(reactorsInfoString);
                }
                if (output.Length > 0) {
                    myTextPanel.WriteText(output.ToString());
                } else {
                    myTextPanel.WriteText(textPannelsComands);
                }
            }
            runCounter();
            retryAttempt = 3;
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
            GridTerminalSystem.GetBlocksOfType<IMyReactor>(reactors, i => i.CubeGrid == currentGrid);
            applyProgrammBlockSettings();
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
        }

        void runCounter() {
            if (counter < 18) counter++;
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
                case 11: return "[========|=]";
                case 12: return "[=======|==]";
                case 13: return "[======|===]";
                case 14: return "[=====|====]";
                case 15: return "[====|=====]";
                case 16: return "[===|======]";
                case 17: return "[==|=======]";
                case 18: return "[=|========]";
                default: return "[==========]";
            }
        }

        void writeOnPBScreen(string text) {
            var screen = program.GetSurface(0);
            screen.WriteText($"-=DisplayInfoBase=-\n{getAnimate()}\n{text}");
        }

        void applyProgrammBlockSettings() {
            var screen = program.GetSurface(0);
            screen.ContentType = ContentType.TEXT_AND_IMAGE;
            screen.FontSize = 1.0f;
            screen.FontColor = Color.Green;
            screen.BackgroundColor = Color.Black;
            screen.Alignment = TextAlignment.CENTER;
            string successInit = 
                $"Успешная инициализация! (retry:{3-retryAttempt})" +
                $"\nНайдено {textPanels.Count} текстовых панели!";
            Echo(successInit);
            writeOnPBScreen(successInit + $"\n\nДоступны команды:\n{pbGeneratorTemplate}");
        }

        void applyTextPannelSettings(IMyTextPanel myTextPanel) {
            myTextPanel.ContentType = ContentType.TEXT_AND_IMAGE;
            myTextPanel.FontColor = Color.DarkGreen;
            string customData = myTextPanel.CustomData.ToLower();
            int fontSizeStart = customData.IndexOf(userKeyFontSize);
            if (fontSizeStart >= 0) {
                int start = fontSizeStart + userKeyFontSize.Length;
                int end = customData.IndexOf(")", start);
                if (end > start) {
                    string sizeValue = customData.Substring(start, end - start);
                    float fontSize = parseFloat(sizeValue);
                    if (fontSize > 0) {
                        myTextPanel.FontSize = MathHelper.Clamp(fontSize, 0.1f, 10f);
                    }
                }
            }
            int alignStart = customData.IndexOf(userKeyTextAlign);
            if (alignStart >= 0) {
                int start = alignStart + userKeyTextAlign.Length;
                int end = customData.IndexOf(")", start);
                if (end > start) {
                    string alignValue = customData.Substring(start, end - start);
                    if (alignValue == "c") myTextPanel.Alignment = TextAlignment.CENTER;
                    else if (alignValue == "r") myTextPanel.Alignment = TextAlignment.RIGHT;
                    else if (alignValue == "l") myTextPanel.Alignment = TextAlignment.LEFT;
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
            try {
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
                if (reactors.Count > 0) {
                    foreach (IMyReactor container in reactors) {
                        IMyInventory inventory = container.GetInventory(0);
                        cargoVolume += inventory.CurrentVolume.RawValue;
                        maxVolume += inventory.MaxVolume.RawValue;
                        collectInventoryItems(inventory, itemMap);
                    }
                }
            } catch { reInit(); }
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
            result[physycalType] = GetFormattedItems(itemMap, physycalType);
            result[ammoType] = GetFormattedItems(itemMap, ammoType);
            result[gunType] = GetFormattedItems(itemMap, gunType);
            result[oxygenType] = GetFormattedItems(itemMap, oxygenType);
            result[unknownType] = GetUnknownItems(
                itemMap, 
                new List<String> { 
                    ingotType, componentType, oreType, 
                    physycalType, ammoType, gunType, 
                    oxygenType 
                }
            );
            return result;
        }

        String GetFormattedItems(Dictionary<String, int> source, String type) {
            var sb = new StringBuilder();
            var keyList = source.Keys.Where(key => key.Contains(type)).ToList();
            keyList.Sort();
            foreach (String key in keyList) {
                String name = tryTranslate(key);
                sb.Append($"\n{name}: {source[key]}");
            }
            return sb.ToString();
        }

        String GetUnknownItems(Dictionary<String, int> source, List<String> knownTypes) {
            var sb = new StringBuilder();
            var unknownKeys = source.Keys
                .Where(key => !knownTypes.Any(type => key.Contains(type)))
                .OrderBy(key => key)
                .ToList();
            foreach (String key in unknownKeys) {
                String name = tryTranslate(key);
                sb.Append($"\n{name}: {source[key]}");
            }
            return sb.ToString();
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
            return new KeyValuePair<String, double>($"Батареи ({batteries.Count}) {status}: {percent}%", percent);
        }

        String getTurbinesInfo() {
            if (turbines.Count == 0) return "Ветряные турбины не найдены";
            float totalOutput = 0f;
            int workingTurbines = 0;
            foreach (var turbine in turbines) {
                totalOutput += turbine.CurrentOutput;
                if (turbine.IsWorking) workingTurbines++;
            }
            return $"Турбины({workingTurbines}/{turbines.Count}) работают:  {Math.Round(totalOutput, 2)} МВт";
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
            return $"Генераторы({workingGenerators}/{generators.Count}) работают: {Math.Round(currentOutput, 2)} МВт";
        }

        String getConnectorInfo() {
            var sb = new StringBuilder();
            if (connectors.Count == 0) return "Нет коннекторов!";
            foreach (var connector in connectors) {
                if (connector.Status == MyShipConnectorStatus.Connected) { 
                    IMyShipConnector otherConnector = connector.OtherConnector;
                    IMyCubeGrid connectedGrid = otherConnector.CubeGrid;
                    sb.AppendLine($"'{connector.CustomName}' -V-> {connectedGrid.DisplayName}");
                } else {
                    sb.AppendLine($"'{connector.CustomName}' -X-> Нет соединения");
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

        String getReactorsInfo() {
            StringBuilder sb = new StringBuilder();
            float currentPowerOutput = 0f;
            float maxPowerOutput = 0f;
            float uraniumAmount = 0f;
            int workingReactors = 0;
            if (reactors.Count == 0) sb.Append("\nРеактороы не найдены");
            else {
                foreach (var reactor in reactors) {
                    if (!reactor.IsWorking) continue;
                    var inventory = reactor.GetInventory(0);
                    currentPowerOutput += reactor.CurrentOutput;
                    maxPowerOutput += reactor.MaxOutput;
                    uraniumAmount += (float)inventory.CurrentMass;
                    workingReactors++;
                }
                if (workingReactors>0) {
                    sb.Append(
                        $"\nРаботает {workingReactors}/{reactors.Count} реакторов" +
                        $"\nТекущая мощность {currentPowerOutput:0.00} МВт" +
                        $"\nУран: {uraniumAmount:0.00} кг"
                    );
                } else {
                    sb.Append($"\nРеакторы простаивают");
                }  
            }
            return sb.ToString();
        }

        void manageGenegators(double batteriesPercent, int iceCount, double gasPercent) {
            if (generators.Count == 0) {
                writeOnPBScreen("GeneratorManager\nГенераторы не найдены");
                return;
            }

            int index = program.CustomData.ToLower().IndexOf(userKeyGeneratorManager);
            if (index >= 0) {
                int start = index + userKeyGeneratorManager.Length;
                int end = program.CustomData.IndexOf(")", start);
                if (end > start) {
                    String payloadRaw = program.CustomData.Substring(start, end - start);
                    if (!payloadRaw.Contains(',')) {
                        writeOnPBScreen($"GeneratorManager\nне верный формат аргументов!\n{pbGeneratorTemplate}");
                        return;
                    }
                    var payloadItems = payloadRaw.Split(',');
                    if (payloadItems.Length == 3) {
                        generatorManagerBattareyPercent = parseDouble(payloadItems[0], generatorManagerBattareyPercent);
                        generatorManagerIceMinCount = parseInt(payloadItems[1], generatorManagerIceMinCount);
                        generatorManagerGasPercent = parseDouble(payloadItems[2], generatorManagerGasPercent);
                    } else {
                        writeOnPBScreen($"GeneratorManager\nне достаточно аргументов!\n{pbGeneratorTemplate}");
                        return;
                    }
                }
            }
            Boolean mustBeCharged = batteriesPercent <= generatorManagerBattareyPercent;
            Boolean iceEnough = iceCount > generatorManagerIceMinCount;
            Boolean gasEnough = gasPercent > generatorManagerGasPercent;
            Boolean enable = mustBeCharged && iceEnough && gasEnough;
            string batStatus = mustBeCharged ? "[v] Нужда в зарядке" : "[x] Батареи заряжены";
            string iceStatus = iceEnough ? "[v] Льда достаточно" : "[x] Льда недостаточно";
            string gasStatus = gasEnough ? "[v] Водорода достаточно" : "[x] Водорода недостаточно";
            string generatorStatus = enable ? "Генераторы работают" : "Генераторы выключены";
            string status = 
                $"{generatorStatus}" +
                $"\n\n{batStatus} : {batteriesPercent}/{generatorManagerBattareyPercent}%" +
                $"\n{iceStatus} : {iceCount/1000}к/{generatorManagerIceMinCount/1000}к" +
                $"\n{gasStatus} : {gasPercent}/{generatorManagerGasPercent}%";
            writeOnPBScreen(
                    "GeneratorManager" +
                    $"\n\n{status}"
                );
            foreach (var gen in generators) {
                gen.Enabled = enable;
            }
        }
        
        float parseFloat(string value) {
            float result = 0;
            try { result = float.Parse(value, provider); } 
            catch(Exception e) { writeOnPBScreen(e.Message); }
            return result;
        }

        double parseDouble(string value, double defaultValue) {
            double result = defaultValue;
            try { result = double.Parse(value, provider); } 
            catch(Exception e) { writeOnPBScreen(e.Message); }
            return result;
        }

        int parseInt(string value, int defaultValue) {
            int result = defaultValue;
            try { result = int.Parse(value, provider); } 
            catch (Exception e) { writeOnPBScreen(e.Message); }
            return result;
        }

        void reInit() {
            if (retryAttempt > 0) {
                retryAttempt--;
                initialize();
            } else {
                Echo("Инициализация провалена!\nНе осталось RetryAttempt!");
            }
        }
        /// End of the script
    }
}