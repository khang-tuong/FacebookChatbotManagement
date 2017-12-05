using FacebookChatbotManagement.Models.Entities;
using FacebookChatbotManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using FacebookChatbotManagement.Models.Redis;
using Newtonsoft.Json;

namespace FacebookChatbotManagement.Models.Services
{
    public class ExperimentService
    {
        public ExperimentResultViewModel Analyze(string input)
        {
            string inputTmp = input.Trim();
            PatternService patternService = new PatternService();
            List<Pattern> patterns = patternService.Get(q => q.Active == true).ToList();

            int maxElements = 0;
            Pattern matchPattern = null;
            Dictionary<string, string> matches = null;
            string specialValues = ""; //when the entity simply just want some words
            foreach (var pattern in patterns)
            {
                specialValues = "";
                bool hasUnknwonPhrase = false;
                Dictionary<string, string> matchesTmp = new Dictionary<string, string>();
                inputTmp = input.Trim();
                List<PatternEntityMapping> pems = pattern.PatternEntityMappings.Where(q => q.Active == true).OrderBy(q => q.Position).ToList();
                for (int i = 0; i < pems.Count; i++)
                {
                    Regex regex = null;
                    if (pems[i].Entity.Words == @".*?")
                    {
                        specialValues = inputTmp;
                        hasUnknwonPhrase = true;
                    }
                    else
                    {
                        if (pattern.MatchBegin && pattern.MatchEnd && pems.Count == 1)
                        {
                            regex = new Regex(@"(?:^|\W)^(" + pems[i].Entity.Words + @")$(?:$|\W)", RegexOptions.IgnoreCase);
                        }
                        else if (i == pems.Count - 1 && pattern.MatchEnd)
                        {
                            regex = new Regex(@"(?:^|\W)(" + pems[i].Entity.Words + @")$(?:$|\W)", RegexOptions.IgnoreCase);
                        }
                        else if (specialValues == "")
                        {
                            regex = new Regex(@"(?:^|\W)^(" + pems[i].Entity.Words + @")(?:$|\W)", RegexOptions.IgnoreCase);
                        }
                        else
                        {
                            regex = new Regex(@"(?:^|\W)" + pems[i].Entity.Words + @"(?:$|\W)", RegexOptions.IgnoreCase);
                        }
                        Match result = regex.Match(inputTmp);
                        if (result.Success)
                        {
                            if (matchesTmp.ContainsKey(pems[i].Entity.Name))
                            {
                                string value = matchesTmp[pems[i].Entity.Name];
                                matchesTmp[pems[i].Entity.Name] = value + ";" + result.Value;
                            } else
                            {
                                matchesTmp.Add(pems[i].Entity.Name, result.Value);

                            }
                            if (specialValues != "")
                            {
                                matchesTmp.Add("Cụm từ", specialValues.Substring(0, specialValues.Length - result.Length).Trim());
                                specialValues = "";
                            }
                            if (i == pems.Count - 1)
                            {
                                if ((!hasUnknwonPhrase && matchesTmp.Count > maxElements) || (hasUnknwonPhrase && matchesTmp.Count - 1 > maxElements))
                                {
                                    matchPattern = pattern;
                                    matches = matchesTmp;
                                    maxElements = hasUnknwonPhrase ? matchesTmp.Count - 1 : matchesTmp.Count;
                                    if (result.Index + result.Value.Trim().Length + 1 < inputTmp.Length)
                                    {
                                        specialValues = inputTmp.Substring(result.Index + result.Value.Trim().Length + 1);
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                if (result.Index + result.Value.Trim().Length + 1 < inputTmp.Length)
                                {
                                    inputTmp = inputTmp.Substring(result.Index + result.Value.Trim().Length + 1);
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                }
                if (specialValues != "")
                {
                    if (matches == null && ((!hasUnknwonPhrase && matchesTmp.Count > maxElements) || (hasUnknwonPhrase && matchesTmp.Count - 1 > maxElements)))
                    {
                        matches = matchesTmp;
                        matchPattern = pattern;
                        matches.Add("Cụm từ", specialValues.Trim());
                        maxElements = hasUnknwonPhrase ? matchesTmp.Count - 1 : matchesTmp.Count;
                    } else if (matches!= null && !matches.ContainsKey("Cụm từ") && ((!hasUnknwonPhrase && matchesTmp.Count > maxElements) || (hasUnknwonPhrase && matchesTmp.Count - 1 > maxElements)))
                    {
                        matches.Add("Cụm từ", specialValues.Trim());
                        matchPattern = pattern;
                        maxElements = hasUnknwonPhrase ? matchesTmp.Count - 1 : matchesTmp.Count;
                    }
                    specialValues = "";
                }
            }



            ExperimentResultViewModel model = new ExperimentResultViewModel();
            if (matchPattern != null)
            {
                model.IntentId = matchPattern.IntentId ?? 0;
                model.Group = matchPattern.Group ?? 0;
                model.Matches = matches;
            }

            return model;
        }

        public void PushToRedis()
        {
            IntentService intentService = new IntentService();
            PatternService patternService = new PatternService();
            List<IntentRedisViewModel> intents = intentService.GetAllForRedis();
            List<string> patterns = patternService.GetFullPatterns();

            var db = RedisConnectionHelper.Connection.GetDatabase();
            db.StringSet(@"BotIntents", JsonConvert.SerializeObject(intents));
            db.StringSet(@"BotPatterns", JsonConvert.SerializeObject(patterns));
        }

        private int CalcLevenshteinDistance(string a, string b)
        {
            if (String.IsNullOrEmpty(a) || String.IsNullOrEmpty(b)) return 0;

            int lengthA = a.Length;
            int lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthA; i++)
                for (int j = 1; j <= lengthB; j++)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min
                        (
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost
                        );
                }
            return distances[lengthA, lengthB];
        }
    }
}