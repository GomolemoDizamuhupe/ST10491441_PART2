using System;
using System.Media;

namespace PART2_POE_
{
    public class SoundGreet
    {
        //Method for the bot voice
        public void botVoice(string voice)
        {
            string paths = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\", @voice);

            try
            {
                SoundPlayer player = new SoundPlayer(voice);
                player.Play();
            }
            catch (Exception ex)
            {
                string text = $"Something is wrong with the auto. {ex.Message}";
            }
        }

        public void PlayVoice(string voicePath, bool isMute)
        {
            if (!isMute)
            {
                botVoice(voicePath);
            }
        }



    }
}