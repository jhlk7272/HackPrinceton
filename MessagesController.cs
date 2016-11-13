using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Timers;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace HappyBot
{
    public static class Globals
    {
        public static int interactionCount = 0;
        public static float[] pastMood = { 0, 0, 0, 0, 0, 0, 0, 0 };
        public static float maxMood = 0;
        public static int maxMoodIndex = 0;
        public static float maxMoodNew = 0;
        public static int maxMoodIndexNew = 0;
    }

        [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        //private static System.Timers.Timer aTimer;
        //int interactionCount = 0;
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            
            if (activity.Type == ActivityTypes.Message)
            {
                //ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                // return our reply to the user
                Activity reply = activity.CreateReply("");
                if (activity.Text.Contains("hi") || activity.Text.Contains("Hi") || activity.Text.Contains("hello") || activity.Text.Contains("Hello") || activity.Text.Contains("hey") || activity.Text.Contains("Hey"))
                {
                    Globals.interactionCount++;
                    reply = activity.CreateReply($"Hello, my name is HappyBot! How's your day going?");
                }
                else if (activity.Text.Contains("yes") || activity.Text.Contains("YES") || activity.Text.Contains("yea") || activity.Text.Contains("YEA") || activity.Text.Contains("su") || activity.Text.Contains("SU"))
                {
                    //HAPPYIERRESPONSE
                    reply = activity.CreateReply($"sure here's another happy thing!");
                }
                else if (Globals.interactionCount > 1 && (activity.Text.Contains("no") || activity.Text.Contains("NO") || activity.Text.Contains("Bye") || activity.Text.Contains("No") || activity.Text.Contains("nu") || activity.Text.Contains("na") || activity.Text.Contains("Nu")))
                {
                    //BYE
                    reply = activity.CreateReply($"Hope you have a great day! See you again soon :) ");
                }
                else if (activity.Text == "angry")
                {
                    reply = activity.CreateReply($"Don't be angry D:");
                }
                else if (activity.Text == "contempt")
                {
                    reply = activity.CreateReply($"What are you unhappy about?");
                }
                else if (activity.Text == "disgust")
                {
                    reply = activity.CreateReply($"Don't give me that dirty look!");
                }
                else if (activity.Text == "fear")
                {
                    reply = activity.CreateReply($"Fear leads to anger, anger leads to pain, pain leads to suffering.");
                }
                else if (activity.Text == "happy")
                {
                    reply = activity.CreateReply($"I am glad you are happy!");
                }
                else if (activity.Text == "neutral")
                {
                    reply = activity.CreateReply($"How is it going?");
                }
                else if (activity.Text == "sadness")
                {
                    reply = activity.CreateReply($"There there, every little thing is gonna be alright!");
                }
                else if (activity.Text == "surprise")
                {
                    reply = activity.CreateReply($"Tell me about it.");
                }
                else
                {

                    //reading from text file
                    string[] currentMoodString = File.ReadAllLines(@"C:\Users\Joohyun\Desktop\HackPrinceton\VideoAnalyzer\WriteLines.txt", Encoding.UTF8);
                    //change it back to float and store into a local array, then compare it against existing values to detect spikes or changes
                    
                    //very first remedy
                    if (Globals.interactionCount == 1)
                    {

                        //convert to float and find the max
                        for (int i = 0; i < 8; i++)
                        {
                            Globals.pastMood[i] = float.Parse(currentMoodString[i]);
                            if (Globals.pastMood[i] >= Globals.maxMood)
                            {
                                Globals.maxMood = Globals.pastMood[i];
                                Globals.maxMoodIndex = i;
                            }
                        }

                        //you know the dominant mood, here's the response if it's a negative mood
                        //anger, contempt, disgust, fear, sadness, neutral we show a happy respose here
                        if (Globals.maxMoodIndex == 0 || Globals.maxMoodIndex == 1 || Globals.maxMoodIndex == 2 || Globals.maxMoodIndex == 3 || Globals.maxMoodIndex == 5 || Globals.maxMoodIndex == 6)
                        {
                            //since you're sad/neutral here's a happy thing
                            //Attachment attachment3;
                            //attachment3.ContentUrl = $"http://i.imgur.com/sgMXV.jpg";
                            //attachment3.ContentType = "image/jpg";
                            //attachment3.Name = "RedPanda";
                            //reply.Attachments = new List<Attachment>();
                            //reply.Attachments.Add(attachment3);
               
                            reply = activity.CreateReply($"Oh no, you don't seem to be having too good a day. So here's something to cheer you up! ");



                        }
                        else
                        {
                            //HAPPIER and happier response
                            reply = activity.CreateReply($"HAPPY THING sIncE you WANT HAPPIER TypE CAT");
                        }
                        Globals.interactionCount++;

                    }
                    //after the first count
                    else 
                    {

                        float[] currentMood = { 0, 0, 0, 0, 0, 0, 0, 0 };
                        //convert to float and find the max
                        for (int i = 0; i < 8; i++)
                        {
                            currentMood[i] = float.Parse(currentMoodString[i]);
                            if (currentMood[i] >= Globals.maxMoodNew)
                            {
                                Globals.maxMoodNew = currentMood[i];
                                Globals.maxMoodIndexNew = i;
                            }
                        }

                        //compare between past mood and current mood
                        //if the past mood 
                        if(Globals.maxMoodIndex == 0 || Globals.maxMoodIndex == 1 || Globals.maxMoodIndex == 2 || Globals.maxMoodIndex == 3 || Globals.maxMoodIndex == 5 || Globals.maxMoodIndex == 6){
                            if (Globals.maxMoodIndexNew == 0 || Globals.maxMoodIndexNew == 1 || Globals.maxMoodIndexNew == 2 || Globals.maxMoodIndexNew == 3 || Globals.maxMoodIndexNew == 5 || Globals.maxMoodIndexNew == 6)
                            {
                                //HARRASSMENT REPLY
                                reply = activity.CreateReply($"ARE YOU HARRASSED? HERE ARE RESOURCES?" + Globals.interactionCount);
                            }
                            else
                            {
                                reply = activity.CreateReply("Hey! Glad to see I made you happy! Do you wanna see another something that will cheer you up?");
                            }
                        }
                        //you were happy in the past
                        else
                        {
                            if (Globals.maxMoodIndexNew == 0 || Globals.maxMoodIndexNew == 1 || Globals.maxMoodIndexNew == 2 || Globals.maxMoodIndexNew == 3 || Globals.maxMoodIndexNew == 5 || Globals.maxMoodIndexNew == 6)
                            {
                                reply = activity.CreateReply("Oh no! You didn't like it? Do you want me to try and cheer you up again? :o ");
                            }
                            else
                            {
                                reply = activity.CreateReply("I'm glad we're making your day! Wanna see another attempt?");
                            }
                        }
                        Console.WriteLine(Globals.interactionCount);
                    }
                   
                }
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
                // Polling one second
                //SetTimer();
                //Console.ReadLine();
                //aTimer.Stop();
                //aTimer.Dispose();
                Activity reply = message.CreateReply($"You are typing!");
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        /*private static void SetTimer()
        {
            aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                              e.SignalTime);
        }*/
    }
}