using System;
using System.Collections.Generic;

namespace CyberSecurityChatBotWithUI
{
    //the result of analysing a user's message
    public class NlpResult
    {
        public string Sentiment { get; set; } = "neutral"; //positive, negative, neutral, or worried
        public string Intent    { get; set; } = "none";    //greeting, help, thanks, urgent, or none
        public string? Prefix   { get; set; }              //optional response prefix based on tone
    }

    //simulates basic natural language processing to make the bot feel more responsive
    //this is rule-based (keyword matching) rather than a real ML model,
    //but it achieves the same effect of adapting responses to the user's tone
    static class NlpHelper
    {
        //words that suggest the user is worried, panicked, or in trouble
        private static readonly HashSet<string> WorriedWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "hacked", "breach", "stolen", "infected", "virus", "help", "emergency",
            "urgent", "compromised", "attacked", "scared", "worried", "panic", "problem",
            "leak", "exposed", "ransomware", "locked out"
        };

        //words that suggest a positive or curious tone
        private static readonly HashSet<string> PositiveWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "great", "thanks", "thank you", "awesome", "good", "nice", "love", "helpful",
            "perfect", "excellent", "cool", "interesting", "brilliant"
        };

        //words that suggest frustration or negativity
        private static readonly HashSet<string> NegativeWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "bad", "useless", "wrong", "broken", "stupid", "hate", "terrible",
            "confused", "annoying", "frustrating", "doesn't work", "not working"
        };

        //greeting patterns the user might type instead of going through the menu
        private static readonly HashSet<string> GreetingWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "hi", "hello", "hey", "howdy", "greetings", "sup", "what's up"
        };

        //thanks patterns
        private static readonly HashSet<string> ThanksWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "thanks", "thank you", "cheers", "appreciate", "ty", "thx"
        };

        //analyses the user's raw input and returns a result with detected sentiment and intent
        public static NlpResult Analyse(string input)
        {
            string lower = input.ToLower();
            string[] words = lower.Split(new[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            var result = new NlpResult();

            //check intent first (greeting or thanks can override the menu flow)
            foreach (string word in words)
            {
                if (GreetingWords.Contains(word))  { result.Intent = "greeting"; break; }
                if (ThanksWords.Contains(word))    { result.Intent = "thanks";   break; }
            }

            //check for urgency/worry in the full input
            foreach (string word in words)
            {
                if (WorriedWords.Contains(word))
                {
                    result.Sentiment = "worried";
                    result.Prefix    = "That sounds serious — let me help right away. ";
                    if (result.Intent == "none") result.Intent = "urgent";
                    break;
                }
            }

            //only check positive/negative if we haven't already found a worried tone
            if (result.Sentiment == "neutral")
            {
                foreach (string word in words)
                {
                    if (PositiveWords.Contains(word))
                    {
                        result.Sentiment = "positive";
                        result.Prefix    = "Glad to hear it! ";
                        break;
                    }
                    if (NegativeWords.Contains(word))
                    {
                        result.Sentiment = "negative";
                        result.Prefix    = "I'm sorry to hear that. Let me try to help. ";
                        break;
                    }
                }
            }

            return result;
        }

        //builds a short personalised prefix based on the detected sentiment
        //called before displaying any bot response so the tone feels natural
        public static string? GetResponsePrefix(string input)
        {
            return Analyse(input).Prefix;
        }
    }
}
