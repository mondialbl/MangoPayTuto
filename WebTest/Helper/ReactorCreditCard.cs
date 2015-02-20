using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MangoPay.SDK;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Core.Enumerations;


namespace Helper
{
    public class ReactorCreditCard
    {

        public string data { get; set; }
        public string accessKeyRef { get; set; }
        public string cardNumber { get; set; }
        public string cardExpirationDate { get; set; }
        public string cardCvx { get; set; }
        

        //Constructor
        public ReactorCreditCard()
        {
            
            
        }


        public static ReactorCreditCard FakeVisa1()
        {
            ReactorCreditCard card = new ReactorCreditCard();

            card.cardNumber = "4970100000000154";
            card.cardExpirationDate = "1218";
            card.cardCvx = "123";

            return card;
        }


       


        



    }
}