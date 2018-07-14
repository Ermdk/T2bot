using System;
using Telegram.Bot;
using MihaZupan;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace T2Bot
{

    class MainClass
    {

        static HttpToSocks5Proxy GetProxy = new HttpToSocks5Proxy("107.181.187.182", 61683, "kATOsLqebq", "edk.kzn");
        static readonly TelegramBotClient Bot = new TelegramBotClient("561349235:AAEolvAsJnWFPToxjV-jBcrk2GMfWwRNyd0", GetProxy);
        private static readonly ReplyKeyboardMarkup Key = new ReplyKeyboardMarkup();
        private const string BSinfoReqest = "Необходима информация о сайте";
        private const string greetings = "Привет, это T2_DutyBot. Добро пожаловать. Нажми команду Необходима информация о сайте для продолжения."; private const bool resizeKeyboard = true;
        private const string pointBS = "Укажите, пожалуйста, по какому сайту необходима информация?!";

        public static void Main(string[] args)
        {

            GetProxy.ResolveHostnamesLocally = true;
            Bot.StartReceiving();
            Bot.OnMessage += Bot_OnMessage;
            Console.ReadLine();
        }


        protected static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {

            var BSnumberinput = e.Message.Text;
            int value;
            using (StreamReader reader = new StreamReader(@"/Users/Dima/Desktop/Копия BS_info.csv", Encoding.GetEncoding(1251), true))

            {
                List<string> listBSnumber = new List<string>();
                List<string> listBSadress = new List<string>();
                List<string> listBScontractor = new List<string>();
                List<string> listBSrectifier = new List<string>();
                List<string> listBSpriority = new List<string>();
                List<string> listBScontacts1 = new List<string>();
                List<string> listBScontacts2 = new List<string>();


                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    listBSnumber.Add(values[0]);
                    listBSadress.Add(values[1]);
                    listBScontractor.Add(values[2]);
                    listBSrectifier.Add(values[3]);
                    listBSpriority.Add(values[4]);
                    listBScontacts1.Add(values[5]);
                    listBScontacts2.Add(values[6]);

                }

                if (e.Message.Text == BSnumberinput && int.TryParse(BSnumberinput, out value) && listBSnumber.Contains(Convert.ToString(BSnumberinput)))
                {

                    var indexOfBS = listBSnumber.IndexOf(BSnumberinput);
                    var BSadress = listBSadress.ElementAt(indexOfBS);
                    var BScontractor = listBScontractor.ElementAt(indexOfBS);
                    var BSrectifier = listBSrectifier.ElementAt(indexOfBS);
                    var BSpriority = listBSpriority.ElementAt(indexOfBS);
                    var BScontacts1 = listBScontacts1.ElementAt(indexOfBS);
                    var BScontacts2 = listBScontacts2.ElementAt(indexOfBS);

                    Bot.SendTextMessageAsync
                    (e.Message.Chat.Id, "Адрес: " + "\r\n" + Convert.ToString(BSadress)
                     + "\r\n" + "Подрядчик: " + "\r\n" + Convert.ToString(BScontractor)
                      + "\r\n" + "Тип выпрямителя: " + "\r\n" + Convert.ToString(BSrectifier)
                       + "\r\n" + "Приоритет БС: " + "\r\n" + Convert.ToString(BSpriority)
                         + "\r\n" + "Котакты: " + "\r\n" + Convert.ToString(BScontacts1) + "\r\n" + Convert.ToString(BScontacts2),
                     replyMarkup: new ReplyKeyboardMarkup(new[] { new KeyboardButton("Другой сайт") }, resizeKeyboard)
                    );
                }

                else if (e.Message.Text == BSnumberinput && int.TryParse(BSnumberinput, out value) && !listBSnumber.Contains(Convert.ToString(BSnumberinput)))
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Данной БС нет в списке", replyMarkup: new ReplyKeyboardMarkup(new[] { new KeyboardButton("Другой сайт") }, resizeKeyboard));
                }

                else if (e.Message.Text == BSinfoReqest || e.Message.Text == "Другой сайт")
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, pointBS, replyMarkup: new ReplyKeyboardRemove());
                }

                else
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, greetings,
                    replyMarkup: new ReplyKeyboardMarkup(new[] { new KeyboardButton(BSinfoReqest) }, resizeKeyboard));
                }

            }

        }

    }
}
