/* Brief: Audio Util
 * Author: Komal
 * Date: "2019-07-17"
 */

namespace komal {
    public partial class KomalUtil {
        public static string MSG_SWITCH_SOUND = "MSG_SWITCH_SOUND";
        public static string MSG_SWITCH_MUSIC = "MSG_SWITCH_MUSIC";
        public static string MSG_SWITCH_VIBRATION = "MSG_SWITCH_VIBRATION";
        public bool SoundEnabled {
            get { return GetItem("sound_switch", true); }
            set {
                var pre = this.SoundEnabled;
                if(pre != value){
                    SetItem("sound_switch", value);
                    this.facade.SendNotification(MSG_SWITCH_SOUND, value);
                }
            }
        }

        public bool MusicEnabled {
            get { return GetItem("music_switch", true); }
            set {
                var pre = this.SoundEnabled;
                if(pre != value){
                    SetItem("music_switch", value);
                    this.facade.SendNotification(MSG_SWITCH_MUSIC, value);
                }
            }
        }

        public bool VibrationEnabled {
            get { return GetItem("vibration_switch", true); }
            set {
                var pre = this.SoundEnabled;
                if(pre != value){
                    SetItem("vibration_switch", value);
                    this.facade.SendNotification(MSG_SWITCH_VIBRATION, value);
                }
            }
        }
    }
}
