using System;
using System.Text;
using System.Collections.Generic;

using Sandbox.ModAPI;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using static Sandbox.Definitions.MyCharacterDefinition;

using VRageMath;

using BlendTypeEnum = VRageRender.MyBillboard.BlendTypeEnum;

using static Draygo.API.HudAPIv2;

using Suffocation.Utilities;


namespace Suffocation.Menu
{
    public class PlayerMenu
    {
        internal MenuRootCategory AdminMenu;
        internal MenuTextInput GlobalZeroPDamageTxt;
        internal MenuSubCategory CharMenu;
        internal Dictionary<string, CharacterTextMenu> Characters;
        private MyDefinitionManager DefMng;
        private static InstSuffSettingsConfig ConfigData;

        float GlobalZeroPDamageVal;

        public PlayerMenu()
        {
            //Character = MyAPIGateway.Session.Player as MyCharacter;
            this.DefMng = MyDefinitionManager.Static;
            GlobalZeroPDamageVal = 0;
        }

        /*public override void RegisterComponent()
        {
            Api = new HudAPIv2();
        }*/

        public void Register()
        {
            AdminMenu = new MenuRootCategory("Instant Suffocation", MenuRootCategory.MenuFlag.AdminMenu, "Admin Settings");
            GlobalZeroPDamageTxt = new MenuTextInput($"Global damage at zero pressure: <color=orange>{GlobalZeroPDamageVal}", AdminMenu, "Set the damage per tick for all characters", Global_ZeroPDamage_Submitted);
            CharMenu = new MenuSubCategory($"Individual Characters <color=orange>=>", AdminMenu, "Characters");
            Characters = new Dictionary<string, CharacterTextMenu>();
            foreach (var charDef in DefMng.Characters)
            {
                Characters.Add(charDef.Id.SubtypeName, new CharacterTextMenu(charDef, CharMenu));
            }
        }

        internal void Global_ZeroPDamage_Submitted(string input)
        {
            if(float.TryParse(input, out GlobalZeroPDamageVal))
            {
                GlobalZeroPDamageTxt.Text = $"<color=white>Global damage at zero pressure: <color=orange>{GlobalZeroPDamageVal}";
                foreach (var charDef in DefMng.Characters)
                {
                    charDef.DamageAmountAtZeroPressure = GlobalZeroPDamageVal;
                    Characters[charDef.Id.SubtypeName].menu.Text = $"<color=white>{charDef.Id.SubtypeName}:<color=orange>{charDef.DamageAmountAtZeroPressure}";
                }
            }
        }

        public void UpdateValues()
        {
            foreach (var charDef in DefMng.Characters)
            {
                Characters[charDef.Id.SubtypeName].menu.Text = $"<color=white>{charDef.Id.SubtypeName}:<color=orange>{charDef.DamageAmountAtZeroPressure}";
            }
        }
    }

    public class CharacterTextMenu
    {
        public MyCharacterDefinition charDef;
        public MenuTextInput menu;
        private static InstSuffSettingsConfig ConfigData;

        public CharacterTextMenu(MyCharacterDefinition charDef, MenuCategoryBase CharMenu)
        {
            this.charDef = charDef;
            this.menu = new MenuTextInput($"{this.charDef.Id.SubtypeName}:<color=orange>{this.charDef.DamageAmountAtZeroPressure}", CharMenu, "Set the damage per tick at zero pressure", this.ZeroPDamage_Submitted);
        }
        
        private void ZeroPDamage_Submitted(string input)
        {
            float num;
            if(float.TryParse(input, out num))
            {
                this.charDef.DamageAmountAtZeroPressure = num;
                this.menu.Text = $"<color=white>{this.charDef.Id.SubtypeName}:<color=orange>{this.charDef.DamageAmountAtZeroPressure}";
                ConfigData = new InstSuffSettingsConfig();
                Config.WriteFileToWorldStorage<InstSuffSettingsConfig>("Config.xml", typeof(InstSuffSettingsConfig), ConfigData);
            }
        }
    }
}