﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class p_Message
    {
        private const int POCKETTYPE = 2; // 1 байта
        private int length; //3 байта
        public string receiver, sender; //16 байт
        public string message; // Max 150 символов = 150 байт. MAX Размер пакета 120 байта

        
        public p_Message(byte[] input)
        {
            if (int.Parse(ASCIIEncoding.GetEncoding(1251).GetString(input, 0, 1)) == POCKETTYPE)
            {
                length = int.Parse(ASCIIEncoding.GetEncoding(1251).GetString(input, 1, 3)); 
                string s = ASCIIEncoding.GetEncoding(1251).GetString(input, 4, length);
                receiver = s.Remove(s.LastIndexOf("::"));
                message = s.Substring(s.IndexOf("::") + 2);
            }
            else
            {
                Console.WriteLine("Неверный тип пакета");
            }
        }
        public p_Message(string s, string m)
        {
            sender = s;
            message = m;
            length = (ASCIIEncoding.GetEncoding(1251).GetBytes(sender + "::" + message)).Length;
        }

        public byte[] MakePocket()
        {
            byte[] bts0, bts1, bts2;

            bts0 = ASCIIEncoding.GetEncoding(1251).GetBytes(Convert.ToString(POCKETTYPE));

            if (length < 10)
            {
                bts1 = ASCIIEncoding.GetEncoding(1251).GetBytes(Convert.ToString("0" + "0" + length));
            }
            else
            {
                if (length < 100)
                {
                    bts1 = ASCIIEncoding.GetEncoding(1251).GetBytes(Convert.ToString("0" + length));
                }
                else
                {
                    bts1 = ASCIIEncoding.GetEncoding(1251).GetBytes(Convert.ToString(length));
                }
            }

            bts2 = ASCIIEncoding.GetEncoding(1251).GetBytes(sender + "::" + message);

            bts0 = bts0.Concat(bts1.Concat(bts2).ToArray()).ToArray();

            return bts0;    
        }
    }
}
