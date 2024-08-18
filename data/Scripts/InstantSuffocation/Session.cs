using System;
using Suffocation.Menu;
using Draygo.API;

using Sandbox;
using Sandbox.ModAPI;
using Sandbox.Definitions;

using Suffocation.Utilities;

namespace Suffocation
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public partial class InstSuffSession : MySessionComponentBase
    {
        public static InstSuffSession Instance;
        private const string CONFIG_FILE_NAME = "Config.xml";
        public static InstSuffSettingsConfig ConfigData;
        public HudAPIv2 HudAPI;
        public PlayerMenu PlayerMenu;

        public override void BeforeStart()
        {
            Instance = this;

            PlayerMenu = new PlayerMenu();
            HudAPI = new HudAPIv2(HudAPICallback);
        }

        public override void LoadData()
        {
            if (MyAPIGateway.Session.IsServer)
            {
                ConfigData = Config.ReadFileFromWorldStorage<InstSuffSettingsConfig>(CONFIG_FILE_NAME, typeof(InstSuffSettingsConfig));
                if (ConfigData == null)
                {
                    ConfigData = new InstSuffSettingsConfig();
                }
            }
            else
            {
                try
                {
                    string str;
                    MyAPIGateway.Utilities.GetVariable("OrbitSettings_Config_xml", out str);

                    byte[] bytes = Convert.FromBase64String(str);
                    ConfigData = MyAPIGateway.Utilities.SerializeFromBinary<InstSuffSettingsConfig>(bytes);
                }
                catch
                {
                    ConfigData = new InstSuffSettingsConfig();
                }
            }

            foreach (var charDef in ConfigData.Characters)
            {
                MyDefinitionManager.Static.Characters[charDef.SubtypeId].DamageAmountAtZeroPressure = charDef.DamageAmountAtZeroPressure;
            }
        }

        private void HudAPICallback()
        {
            try
            {
                PlayerMenu.Register();
                PlayerMenu.UpdateValues();

                //var pkt = new SettingRequestPacket();
                //Network.SendToServer(pkt);
            }
            catch (Exception ex)
            {

            }
        }
    }
}