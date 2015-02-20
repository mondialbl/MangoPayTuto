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
    public class Samples
    {


        private static string _sandbox_John_UserId = "5775821";
        private static string _sandbox_Julie_UserId = "5776936";
        private static string _sandbox_John_WalletId = "5776455";
        private static string _sandbox_Julie_WalletId = "5776938";

        private static string _sandbox_John_CardId = "5802446";
        private static string _sandbox_John_CardRegistrationId = "5802445"; //A choper.The validity of a registered card (CardId) before a payment or a pre-authorisation is  30min maximum. Once one of these operations is made, the card « validity » field change to « VALID ». Then,it is available up to the expiry date.


        public static UserNaturalDTO CreateUserJohn(MangoPayApi api)
        {

            UserNaturalPostDTO user = new UserNaturalPostDTO("john.doe@sample.org", "John", "Doe", new DateTime(1975, 12, 21, 0, 0, 0), CountryIso.FR, CountryIso.FR);
            user.Occupation = "programmer";
            user.IncomeRange = 3;
            user.Address = "Some Address";

            UserNaturalDTO john = api.Users.Create(user);

            return john;
        
        }
        public static UserNaturalDTO CreateUserJulie(MangoPayApi api)
        {

            UserNaturalPostDTO user = new UserNaturalPostDTO("julie.Mclowed@sample.org", "Julie", "Mclowed", new DateTime(1985, 10, 1, 0, 0, 0), CountryIso.FR, CountryIso.FR);
            user.Occupation = "fleuriste";
            user.IncomeRange = 3;
            user.Address = "Some Address";

            UserNaturalDTO julie = api.Users.Create(user);

            return julie;

        }

        public static UserNaturalDTO GetJohn(MangoPayApi api)
        {
            return (UserNaturalDTO)api.Users.GetNatural(_sandbox_John_UserId);
        }
        public static UserNaturalDTO GetJulie(MangoPayApi api)
        {
            return (UserNaturalDTO)api.Users.GetNatural(_sandbox_Julie_UserId);
        }
        
        public static WalletDTO CreateJohnWallet(MangoPayApi api)
        {
            List<string> owners = new List<string>();
            owners.Add(_sandbox_John_UserId);
            WalletPostDTO wallet = new WalletPostDTO(owners, "Wallet de John", CurrencyIso.EUR);
            WalletDTO johnWallet = api.Wallets.Create(wallet);

            return johnWallet;

        }
        public static WalletDTO CreateJulieWallet(MangoPayApi api)
        {
            List<string> owners = new List<string>();
            owners.Add(_sandbox_Julie_UserId);
            WalletPostDTO wallet = new WalletPostDTO(owners, "Wallet de Julie", CurrencyIso.EUR);
            WalletDTO julieWallet = api.Wallets.Create(wallet);

            return julieWallet;

        }

        public static WalletDTO GetJohnWallet(MangoPayApi api)
        {
            return (WalletDTO)api.Wallets.Get(_sandbox_John_WalletId);
        }
        public static WalletDTO GetJulieWallet(MangoPayApi api)
        {
            return (WalletDTO)api.Wallets.Get(_sandbox_Julie_WalletId);
        }

        public static string GetJohnWalletInfo(MangoPayApi api)
        {
            string str = "Get John Wallet Info : ";

            WalletDTO wallet = GetJohnWallet(api);

            str += "id : " + wallet.Id + " CreationDate : " + wallet.CreationDate + " balance : " + wallet.Balance.Amount.ToString() + " " + wallet.Balance.Currency.ToString() + " Currency : " + wallet.Currency.ToString() + " Owners : " + wallet.Owners.Aggregate((a, b) => a + ", " + b) + "<br>";

            return str;
        }
        public static string GetJulieWalletInfo(MangoPayApi api)
        {
            string str = "Get Julie Wallet Info : ";

            WalletDTO wallet = GetJulieWallet(api);

            str += "id : " + wallet.Id + " CreationDate : " + wallet.CreationDate + " balance : " + wallet.Balance.Amount.ToString() + " " + wallet.Balance.Currency.ToString() + " Currency : " + wallet.Currency.ToString() + " Owners : " + wallet.Owners.Aggregate((a, b) => a + ", " + b) + "<br>";

            return str;
        }


     

        public static Tuple<CardRegistrationDTO, CardDTO> CreateJohnCard(MangoPayApi api)
        {
            //Simule un enregistrement sur le web d'une carte de crédit (ici fake)
            ReactorCreditCard fakeVisa = ReactorCreditCard.FakeVisa1();

            Tuple<CardRegistrationDTO, CardDTO> cardTuple = Ninja.AddCreditCardToMangoPay(api, fakeVisa, GetJohn(api));

            return cardTuple;
        }
        public static CardDTO GetJohnCard(MangoPayApi api)
        {
            return (CardDTO)api.Cards.Get(_sandbox_John_CardId);
        }
        public static CardRegistrationDTO GetJohnCardRegistration(MangoPayApi api)
        {
            return (CardRegistrationDTO)api.CardRegistrations.Get(_sandbox_John_CardRegistrationId);
        }

        

    }
}