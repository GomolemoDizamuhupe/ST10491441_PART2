using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace PART2_POE_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bot botManager = new bot();
        string voicePath;
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();
        SoundGreet SoundManager = new SoundGreet();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            //Playing audio(Bot voice)
            voicePath = "Greeting.wav";
            SoundManager.botVoice(voicePath);
        }
        //Message layout
        //User message layout method
        private void AddUserMessage(string text, string sender)
        {
            Messages.Add(new Message
            {
                Time = DateTime.Now.ToString("HH:mm"),
                Text = text,
                Sender = sender,

                MessageColor = Brushes.LimeGreen
            });
        }

        //Message layout
        //Bot message layout method
        private void AddBotMessage(string text, string sender)
        {
            Messages.Add(new Message
            {
                Time = DateTime.Now.ToString("HH:mm"),
                Text = text,
                Sender = sender,
                MessageColor = Brushes.LimeGreen
            });
        }
        private void start_bot(object sender, RoutedEventArgs e)
        {
            welcome_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
        }

        private void submit_username(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();

            if (!string.IsNullOrWhiteSpace(username))
            {

                if (HasSpecialChars(username))
                {
                    string filePath = "Usernames.txt";

                    bool returningUser = DoesUserExists(username, filePath);


                    if (!returningUser)
                    {
                        File.AppendAllText(filePath, username + "\n");
                    }

                    username_grid.Visibility = Visibility.Hidden;
                    chatbot_grid.Visibility = Visibility.Visible;

                    if (returningUser)
                    {
                        AddBotMessage($"Hi welcome back {username}. I hope you still remember me, my name is Cyberbot.", "Bot");

                    }
                    else
                    {
                        AddBotMessage($"Hi {username}, my name is Cyberbot.", "Bot");
                    }

                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                    AddBotMessage($"Let me tell you a bit about myself, I'm here to help you with online password safety, phishing, safe browsing, 2FA, malware, ransomware, privacy and scam.", "Bot");
                    AddBotMessage($"How can I help you {username} ? ", "Bot");
                }
                else
                {
                    MessageBox.Show("Your name must not contain numbers[0-9] and special characters.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtUsername.Clear();
                    txtUsername.Focus();
                }

            }
            else 
            {
                MessageBox.Show("Enter your name.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsername.Clear();
                txtUsername.Focus();
            }
        }


        public bool DoesUserExists(string username, string filePath)
        {
            if (!File.Exists(filePath)) 
            {
                return false;
            }

            string users = File.ReadAllText(filePath);
            return users.Contains(username);
        }



        private void submit_response(object sender, RoutedEventArgs e)
        {
            
            string username = txtUsername.Text.Trim();

            string chat_response = string.Empty;
            string user_response = txtUserResponse.Text.Trim().ToLower();


            logicResponse(username, chat_response, user_response);
        }

        //Method checks if the string has an integer or special character
        private bool HasSpecialChars(string userName)
        {
            var pattern = @"^[A-Za-z]+$";
            return Regex.IsMatch(userName, pattern);
        }


        private void logicResponse(string username, string chat_response, string user_response)
        {
                        
            if (!string.IsNullOrWhiteSpace(txtUserResponse.Text))
            {
                AddUserMessage(user_response, username);

                if (user_response.Contains("i'm interested in") ||
                    user_response.Contains("im interested in") ||
                    user_response.Contains("i am interested in") ||
                    user_response.Contains("my favourite topic is") ||
                    user_response.Contains("my favorite topic is"))
                {
                    chat_response = botManager.SetUserFavouriteTopic(user_response, username);

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }

                else if (user_response.Contains("worried") ||
                    user_response.Contains("curious") ||
                    user_response.Contains("frustrated") ||
                    user_response.Contains("stressed"))
                {
                    chat_response = botManager.sentimentDetection(user_response, username);

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }
                else if (user_response.Contains("password") ||
                    user_response.Contains("malware") ||
                    user_response.Contains("2fa") ||
                    user_response.Contains("phishing") ||
                    user_response.Contains("ransomware") ||
                    user_response.Contains("safe browsing") ||
                    user_response.Contains("privacy") ||
                    user_response.Contains("scam"))
                {

                    chat_response = botManager.GetTopic(user_response);

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }
                else if (user_response.Contains("my name"))
                {
                    chat_response = $"Your name is {username}";

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }
                else if (user_response.Contains("purpose"))
                {
                    chat_response = "My purpose is to help you with online safety topics like password safety, phishing, safe browsing, 2FA, malware, ransomware, privacy and scam. ";

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }
                else if (user_response.Contains("how are you"))
                {
                    chat_response = $"I'm great {username}, how can I help you with online safety topics like password safety, phishing, safe browsing, 2FA, malware, ransomware, privacy and scam? ";

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }

                else if (user_response.Contains("i ask"))
                {
                    chat_response = "You can ask me about with online safety topics like password safety, phishing, safe browsing, 2FA, malware, ransomware, privacy and scam. ";

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }
                else if (user_response.Contains("explain") ||
                    user_response.Contains("another tip") ||
                    user_response.Contains("more"))
                {
                    chat_response = botManager.followUpQuestions(user_response, username);

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }
                else
                {
                    chat_response = "I didn't quite understand that. Could you rephrase?";

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }
                }
                else
                {
                    MessageBox.Show($"Please enter something {username}.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtUsername.Clear();
                    txtUsername.Focus();
                }
            }
        private void Exit_chatbot(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }

    public class Message
    {
        public string Time { get; set; }
        public string Text { get; set; }
        public string Sender { get; set; }

        //Stores the color of the messages
        public Brush MessageColor { get; set; }
    }
}