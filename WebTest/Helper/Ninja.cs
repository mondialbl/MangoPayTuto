using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RestSharp;

using MangoPay.SDK;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Core.Enumerations;

namespace Helper
{
    public class Ninja
    {


        public static void AddMoneyOnUserWalletByCreditCard(MangoPayApi api, UserNaturalDTO user, WalletDTO wallet, ReactorCreditCard reactorCard, int amount)
        {
            Tuple<CardRegistrationDTO, CardDTO> cardTuple = Ninja.AddCreditCardToMangoPay(api, reactorCard, user);

            PayInCardDirectDTO payInCardDirectDTO = Ninja.CreatePayInCardDirect(api, cardTuple.Item1, cardTuple.Item2, wallet, amount);
        }





        //https://docs.mangopay.com/api-references/card-registration/
        public static Tuple<CardRegistrationDTO, CardDTO> AddCreditCardToMangoPay(MangoPayApi api, ReactorCreditCard card, UserNaturalDTO user)
        {
            //1 :  Envoi une requete de création de carte (pré-enregistrement)
            CardRegistrationPostDTO cardRegistrationPost = new CardRegistrationPostDTO(user.Id, CurrencyIso.EUR);

            //2 : récupère un objet CardRegistration depuis le serveur MangoPay (permet de prendre une url du serveur de token délivré par MangoPay)
            CardRegistrationDTO cardRegistration = api.CardRegistrations.Create(cardRegistrationPost);

            //3 : créer un objet d'update CardRegistration 
            CardRegistrationPutDTO cardRegistrationPut = new CardRegistrationPutDTO();

            //4 : à partir de l'objet carte en 2, récupère un token donné par le server de token (url)
            cardRegistrationPut.RegistrationData = Ninja.GetPaylineCorrectRegistartionData(cardRegistration, card); //exemple de retour : data=VLTIjgpf1ag15dmwRyhmORVzZJ3g4beuWtM-XsLxX42SJIgXGyCSMWLYR8VvJ7xd_pUJJxM61mjOtNI_u2eb6A645Q1xtacJUXE8ZAEkH_SIexemLrNF98M4OD2KHeYV0ftIYwFxOdfmDQ5GtM_cIg

            //5 : update la carte sur MangoPay
            cardRegistration = api.CardRegistrations.Update(cardRegistrationPut, cardRegistration.Id);

            //6 : récupère depuis MangoPay la fameuse carte. 
            CardDTO mangoPayCard = api.Cards.Get(cardRegistration.CardId);

            return new Tuple<CardRegistrationDTO,CardDTO>(cardRegistration, mangoPayCard);           
        }

        public static String GetPaylineCorrectRegistartionData(CardRegistrationDTO cardRegistration, ReactorCreditCard card)
        {
            RestClient client = new RestClient(cardRegistration.CardRegistrationURL);

            RestRequest request = new RestRequest(Method.POST);
            request.AddParameter("data", cardRegistration.PreregistrationData);
            request.AddParameter("accessKeyRef", cardRegistration.AccessKey);
            request.AddParameter("cardNumber", card.cardNumber);
            request.AddParameter("cardExpirationDate", card.cardExpirationDate);
            request.AddParameter("cardCvx", card.cardCvx);

            IRestResponse response = client.Execute(request);

            String responseString = response.Content;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return responseString;
            else
                throw new Exception(responseString);
        }

        
   

        //The validity of a registered card (CardId) before a payment or a pre-authorisation is  30min maximum. Once one of these operations is made, 
        //the card « validity » field change to « VALID ». Then,it is available up to the expiry date.
        public static PayInCardDirectDTO CreatePayInCardDirect(MangoPayApi api, CardRegistrationDTO cardRegistration, CardDTO card, WalletDTO wallet, int amount)
        {
            // create pay-in CARD DIRECT
            PayInCardDirectPostDTO payIn = new PayInCardDirectPostDTO(cardRegistration.UserId, cardRegistration.UserId,
                new Money { Amount = amount, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR }, wallet.Id, "http://reactor.fr", card.Id);

            if (card.CardType == CardType.CB || card.CardType == CardType.VISA || card.CardType == CardType.MASTERCARD || card.CardType == CardType.CB_VISA_MASTERCARD)
                payIn.CardType = CardType.CB_VISA_MASTERCARD;
            else if (card.CardType == CardType.AMEX)
                payIn.CardType = CardType.AMEX;

            // create Pay-In
            PayInCardDirectDTO payInCardDirectDTO = api.PayIns.CreateCardDirect(payIn);

            return payInCardDirectDTO;


        }



    }
}