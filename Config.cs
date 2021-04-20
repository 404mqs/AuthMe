using Rocket.API;

namespace CodeRewards
{ 
    public class Config : IRocketPluginConfiguration
    {
        public bool GodModeOnLogging;
        public bool RestrictChatOnLogging;
        public bool AllowVoiceChatOnLogging;
        public bool RestrictDamageOnLogging;
        public bool RestrictLootOnLogging;
        public int SpeedOnLogging;
        public int JumpOnLogging;
        public int GravityOnLogging; 


        public void LoadDefaults()
        {
            GodModeOnLogging = true;
            RestrictChatOnLogging = true;
            AllowVoiceChatOnLogging = false;
            RestrictDamageOnLogging = true;
            RestrictLootOnLogging = true;
            SpeedOnLogging = 0;
            JumpOnLogging = 0;
            GravityOnLogging = 0;
            
        }
    }
}


