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

        //List for Password
        private List<string> passwordTopics = new List<string>()
        {
            "password a password is used to secure access to your accounts or devices.",
            "password it should be strong, long and not easy to guess.",
            "password avoid using personal details when creating one."
        };


        private List<string> malwareTopics = new List<string>()
        {
            "Malware, short for malicious software, is any program designed to harm, disrupt, or" +
                " secretly access a computer or device. It includes viruses, ransomware, spyware, and trojans," +
                " which can steal personal information, damage files, or lock your data for money.",
                "Malware often spreads through unsafe downloads, email attachments, fake websites, or infected USB drives." +
                " Keeping your software updated and avoiding suspicious links are simple but effective ways" +
                " to protect yourself.",
                "Coming soon MW"
        };

        private List<string> phishingTopics = new List<string>()
        {
            "Phishing is a common online scam where attackers pretend to be trusted companies" +
            " to trick people into revealing personal information. These messages often create " +
            "urgency, claim there is a problem with your account, or offer fake rewards.",
            "To stay safe, check the sender's email address carefully, avoid clicking suspicious links or " +
            "downloading unknown attachments, and instead type the official website address directly" +
            " into your browser to verify any claims.",
            "Coming soon PHISHING"
        };

        private List<string> TwoFactorAuthenticationTopics = new List<string>()
        {
"2FA (Two-Factor Authentication) is a security feature that requires two different types" +
                    " of verification before you can access an account. Instead of just entering a password, you also" +
                    " need a second step, like a one-time code sent to your phone, an authenticator app code, or a" +
                    " fingerprint. ",
            "This makes it much harder for someone to access your account even if they steal " +
                    "your password, because they would still need the second factor to log in.",
            "Coming soon 2FA"
        };

        private List<string> ransomwareTopics = new List<string>()
        {
            "Ransomware is a type of malware that locks or encrypts your files and demands money " +
                    "to restore access. It usually spreads through phishing emails, malicious downloads, or outdated" +
                    " software with security weaknesses.",
            "Once infected, you may see a message asking for payment, " +
                    "often in cryptocurrency. Paying does not guarantee your files will be recovered, so the best " +
                    "protection is keeping your software updated, avoiding suspicious links, and regularly backing " +
                    "up important data.",
            "Coming soon RW"
        };

        private List<string> safeBrowsingTopics = new List<string>()
        {
            "Safe browsing means being cautious about where you share your" +
                    " information online. Always check that the website address is correct" +
                    " before entering sensitive details and look for https in the URL, although " +
                    "this alone does not guarantee the site is legitimate.",
            " Avoid downloading files or " +
                    "software from untrusted sources, keep your device and applications updated to fix " +
                    "security weaknesses, and use antivirus software and firewall protection to reduce " +
                    "the risk of malware and other cyber threats.",
            "coming soon SB"
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
