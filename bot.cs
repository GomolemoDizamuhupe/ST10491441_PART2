using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.WebRequestMethods;

namespace PART2_POE_
{
    public class bot
    {
        //Topic lists
        private List<string> passwordTopics = new List<string>()
        {
            "password a password is used to secure access to your accounts or devices.",
            "password it should be strong, long and not easy to guess.",
            "password avoid using personal details when creating one."
        };

        private List<string> malwareTopics = new List<string>()
        {
            "Malware malware is malicious software designed to damage or gain unauthorized access to systems.",
            "Malware keep your antivirus software updated to detect and remove malware.",
            "Malware avoid downloading files or software from untrusted sources."
        };
        
        private List<string> phishingTopics = new List<string>()
        {
            "Phishing is a scam where attackers pretend to be trusted sources to steal information.",
            "Phishing it uses fake messages or websites to trick users into revealing sensitive data",
            "Phishing attackers use deception to make users believe they are legitimate."
        };
        
       private List<string> TwoFactorAuthenticationTopics = new List<string>()
       {
           "2FA or two-factor authentication adds an extra layer of security beyond just a password.",
           "2FA it requires a second verification step such as a code sent to your phone.",
           "2FA always enable 2fa on important accounts to reduce the risk of unauthorized access."
       };
        
        private List<string> ransomwareTopics = new List<string>()
        {
            "Ransomware ransomware is a type of malware that locks your files and demands payment to restore access.",
            "Ransomware never pay the ransom as it does not guarantee your files will be recovered.",
            "Ransomware regularly back up your data to minimize the impact of a ransomware attack."
        };

        private List<string> safeBrowsingTopics = new List<string>()
        {
            "Safe browsing avoid clicking on suspicious links or pop-ups while browsing the internet.",
            "Safe browsing always check that a website uses https before entering personal information.",
            "Safe browsing keep your browser and extensions updated to protect against known vulnerabilities."
        };
        
        private List<string> scamTopics = new List<string>()
        {
            "Scam scammers often impersonate trusted organizations to steal your personal information.",
            "Scam never share sensitive details like passwords or bank information with unverified contacts.",
            "Scam if something seems too good to be true online it is likely a scam."
        };
        
        private List<string> privacyTopics = new List<string>()
        {
            "Privacy protect your privacy by limiting the personal information you share online.",
            "Privacy review app permissions regularly and revoke access that is not necessary.",
            "Privacy use strong privacy settings on your social media and online accounts."
        };

        //Tracks how many times the user has asked about each topic
        private Dictionary<string, int> topicSearchCount = new Dictionary<string, int>();

        //Stores the topic the user explicitly said they are interested in
        private string userFavouriteTopic = string.Empty;

        //The limit at which the bot comments on the user's repeated interest
        private const int InterestThreshold = 3;

        //Tracks whether the bot has already commented on interest for a topic
        private HashSet<string> interestCommentedTopics = new HashSet<string>();


        //Ensures that the random value does not repeat twices in a row
        private int prevIndex = -1;

        private Random random = new Random();
        private int GetRandomNumber()
        {
            int len = 3;

            int index;

            do
            {
                index = random.Next(len);

            } while (prevIndex == index);

            prevIndex = index;

            return index;
        }

        //Store past topics SEARCHED by the user
        private List<string> searchedTopics = new List<string>();


        //Dictionary for topics
        private Dictionary<string, List<string>> topics;

        public bot()
        {
            topics = new Dictionary<string, List<string>>()
            {
                { "password", passwordTopics },
                { "malware",  malwareTopics  },
                { "phishing", phishingTopics },
                { "2fa",  TwoFactorAuthenticationTopics  },
                { "ransomware", ransomwareTopics },
                { "safe browsing",  safeBrowsingTopics  },
                { "scam",  scamTopics  },
                { "privacy",  privacyTopics  }
            };

            foreach (var key in topics.Keys)
                topicSearchCount[key] = 0;
        }


        // Stores the topic so the bot can reference it in later responses.
        public string SetUserFavouriteTopic(string user_response, string username)
        {
            foreach (var key in topics.Keys)
            {
                if (user_response.Contains(key))
                {
                    userFavouriteTopic = key;
                    return $"Great! I'll remember that you're interested in {key}, {username}. " +
                           $"It's a crucial part of staying safe online.";
                }
            }

            return $"I'm glad you're interested in cybersecurity, {username}! " +
                   $"Could you tell me which specific topic? " +
                   $"For example: passwords, phishing, malware, 2FA, ransomware, safe browsing, privacy, or scams.";
        }

        // Builds a personalised prefix if the user has a remembered favourite topic.
        private string GetFavouriteTopicPrefix()
        {
            if (!string.IsNullOrEmpty(userFavouriteTopic))
                return $"As someone interested in {userFavouriteTopic}, you might want to review the security settings on your accounts. ";

            return string.Empty;
        }

        // Checks whether the bot should comment on the user's repeated interest
        // in a topic (fires once, exactly when the count hits InterestThreshold).
        private string CheckInterestNotice(string topicKey)
        {
            if (topicSearchCount[topicKey] == InterestThreshold &&
                !interestCommentedTopics.Contains(topicKey))
            {
                interestCommentedTopics.Add(topicKey); // only comment once per topic
                return $"I've noticed you're really interested in {topicKey}! " +
                       $"It seems to be one of your favourite topics. Here's more on it: ";
            }

            return string.Empty;
        }


        // Gets the searched topic (information)
        public string GetTopic(string user_response)
        {
            string topicsTracker = string.Empty;

            foreach (var topic in topics)
            {
                if (user_response.Contains(topic.Key))
                {

                    topicSearchCount[topic.Key]++;

                    searchedTopics.Add(topic.Key);

                    //Check if the interest notice message should be prepended
                    string interestNotice = CheckInterestNotice(topic.Key);

                    string personalPrefix;

                    if (string.IsNullOrEmpty(interestNotice))
                    {
                        personalPrefix = GetFavouriteTopicPrefix();
                    }
                    else
                    {
                        personalPrefix = string.Empty;
                    }

                    topicsTracker += interestNotice + personalPrefix + topic.Value[GetRandomNumber()];
                }
            }

            if (string.IsNullOrEmpty(topicsTracker))
            {
                return "Sorry, I don't have information on that topic. Try asking about 'password' or 'malware'.";
            }
            else
            {
                return topicsTracker;
            }
        }

        //Sentiment Detection
        public string sentimentDetection(string txtUserResponse, string username)
        {
            string user_response = txtUserResponse.Trim().ToLower();

            // Detect sentiment
            string sentiment = string.Empty;
            string sentimentResponse = string.Empty;

            if (user_response.Contains("worried") || user_response.Contains("scared") || user_response.Contains("nervous"))
            {
                sentiment = "worried";
                sentimentResponse = $"It's completely understandable to feel that way, {username}. " +
                                    "These threats can be very unsettling. Let me share some tips to help you stay safe. ";
            }
            else if (user_response.Contains("curious") || user_response.Contains("interested") || user_response.Contains("want to know"))
            {
                sentiment = "curious";
                sentimentResponse = $"Great that you're curious, {username}! " +
                                    "Staying informed is one of the best ways to protect yourself. Here's what you should know. ";
            }
            else if (user_response.Contains("frustrated") || user_response.Contains("annoyed") || user_response.Contains("confused"))
            {
                sentiment = "frustrated";
                sentimentResponse = $"I understand this can feel overwhelming, {username}. " +
                                    "Let me break it down simply for you. ";
            }
            else if (user_response.Contains("happy") || user_response.Contains("glad") || user_response.Contains("excited"))
            {
                sentiment = "happy";
                sentimentResponse = $"Love the enthusiasm, {username}! " +
                                    "Here's some useful info to keep you even better protected. ";
            }


            // Detect topic and combine with sentiment response
            if (user_response.Contains("password"))
                return sentimentResponse + passwordTopics[GetRandomNumber()];

            else if (user_response.Contains("malware"))
                return sentimentResponse + malwareTopics[GetRandomNumber()];

            else if (user_response.Contains("phishing"))
                return sentimentResponse + phishingTopics[GetRandomNumber()];

            else if (user_response.Contains("2fa"))
                return sentimentResponse + TwoFactorAuthenticationTopics[GetRandomNumber()];

            else if (user_response.Contains("ransomware"))
                return sentimentResponse + ransomwareTopics[GetRandomNumber()];

            else if (user_response.Contains("safe browsing"))
                return sentimentResponse + safeBrowsingTopics[GetRandomNumber()];

            else if (user_response.Contains("privacy"))
                return sentimentResponse + privacyTopics[GetRandomNumber()];

            else if (user_response.Contains("scam"))
                return sentimentResponse + scamTopics[GetRandomNumber()];

            // Sentiment detected but no topic
            else
                return $"{sentimentResponse}Could you tell me which topic you're {sentiment} about? " +
                       "For example: passwords, phishing, malware, 2FA, ransomware, safe browsing, privacy, or scams.";
        }

        //Follow up questions
        public string followUpQuestions(string user_reponse, string username)
        {

            Random random = new Random();
            string prevTopic = string.Empty;

            try
            {
                prevTopic = searchedTopics[searchedTopics.Count - 1];
            }
            catch (ArgumentOutOfRangeException e)
            {
                return $"Can you search a topic about like password safety, phishing, safe browsing, 2FA, malware, ransomware, privacy or scam first {username}.";
            }

            List<string> followUpResponse = new List<string>()
            {
                $"Oh ok {username}, I see you want me to tell you more about {prevTopic}.",
                $"I see you are interested in {prevTopic}."
            };

            if (user_reponse.ToLower().Contains("explain") ||
                user_reponse.ToLower().Contains("another tip") ||
                user_reponse.ToLower().Contains("more"))
            {
                return $"{followUpResponse[random.Next(followUpResponse.Count)]} {GetTopic(prevTopic)}";
            }
            else
            {
                return "I didn't quite understand that. Could you rephrase?";
            }
        }


    }
}
