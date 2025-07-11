using Sandbox.Game;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
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

namespace SE_Programm
{

    public sealed class Program : MyGridProgram
    {


        ///////////
        // Словарь для перевода названий компонентов
        private readonly Dictionary<string, string> _componentNames = new Dictionary<string, string>
{
    {"SteelPlate", "Стальные пластины"},
    {"Construction", "Строительные компон."},
    {"MetalGrid", "Металлические решетки"},
    {"InteriorPlate", "Внутренние пластины"},
    {"Girder", "Металлические балки"},
    {"SmallTube", "Малые трубы"},
    {"LargeTube", "Большие трубы"},
    {"Motor", "Моторы"},
    {"Display", "Дисплеи"},
    {"BulletproofGlass", "Бронестекло"},
    {"Computer", "Компьютеры"}
};

        // Кэшированные ссылки на блоки
        private IMyProjector _projector;
        private IMyTextPanel _lcdDisplay;

        public Program()
        {
            // Инициализация при старте
            var projectors = new List<IMyProjector>();
            GridTerminalSystem.GetBlocksOfType(projectors, p => p.CubeGrid == Me.CubeGrid);
            _projector = projectors.Count > 0 ? projectors[0] : null;

            _lcdDisplay = GridTerminalSystem.GetBlockWithName("PROJECTOR LCD") as IMyTextPanel;
            if (_lcdDisplay != null)
            {
                _lcdDisplay.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
                _lcdDisplay.FontSize = 1.0f;
            }

            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        string GetProjectorInfo()
        {
            if (_projector == null)
                return "Проектор не найден!";

            if (!_projector.IsProjecting)
                return "Проектор: нет активного чертежа";

            // Получаем информацию о блоках
            int totalBlocks = _projector.TotalBlocks;
            int remainingBlocks = _projector.RemainingBlocks;
            int remainingArmor = _projector.RemainingArmorBlocks;
            int buildableBlocks = _projector.BuildableBlocksCount;

            // Получаем имя чертежа (альтернативный способ)
            string blueprintName = "Неизвестный чертеж";
            try
            {
                blueprintName = _projector.DetailedInfo.Split('\n')[0].Replace("Blueprint: ", "");
            }
            catch { }

            // Формируем отчет
            var sb = new StringBuilder();
            sb.AppendLine("=== ИНФОРМАЦИЯ ПРОЕКТОРА ===");
            sb.AppendLine($"Чертеж: {blueprintName}");
            sb.AppendLine($"Прогресс: {totalBlocks - remainingBlocks}/{totalBlocks} блоков");
            sb.AppendLine("-----------------------------");
            sb.AppendLine($"Осталось блоков: {remainingBlocks}");
            sb.AppendLine($"Армор-блоки: {remainingArmor}");
            sb.AppendLine($"Готово к сварке: {buildableBlocks}");

            return sb.ToString();
        }

        void UpdateDisplay()
        {
            string info = GetProjectorInfo();

            if (_lcdDisplay != null)
                _lcdDisplay.WriteText(info);
            else
                Echo(info);
        }

        void Main(string argument)
        {
            UpdateDisplay();
        }
        ////////
    }
}
