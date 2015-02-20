using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RestSharp;

using MangoPay.SDK;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Core.Enumerations;

using Helper;



namespace WebTest
{
    public partial class MangoPay1 : System.Web.UI.Page
    {
        private MangoPayApi api; 

        private string _sandbox_John_UserId = "5775821";
        private string _sandbox_Julie_UserId = "5776936";
        private string _sandbox_John_WalletId = "5776455";
        private string _sandbox_Julie_WalletId = "5776938";
        private string _sandbox_John_CardId = "5800750";



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblDebug.Text = "";


                lblDebug.Text += "Test MangoPay...SANDBOX<br>";
                lblDebug.Text += "..............................................................................................................<br><br>";

                lblDebug.Text += "Initiating MangoPayApi...<br>";
                api = new MangoPayApi();

                api.Config.ClientId = "reactor03";
                api.Config.ClientPassword = "r9YNsNSwOHB8YT8EWrmUD3SO2pJY2PNk88O6CGazCHL0Ny4ddK";

                lblDebug.Text += "api.Config.BaseUrl : " + api.Config.BaseUrl + "<br>";
                lblDebug.Text += "api.Config.ClientId : " + api.Config.ClientId + "<br>";
                lblDebug.Text += "api.Config.ClientPassword : " + api.Config.ClientPassword + "<br><br>";

                //CreateNewClient();

                //CreateJohnAccount();
                //CreateJulieAccount();

                GetAllUsers();

                //CreateJohnWallet();
                //CreateJulieWallet();

                GetJohnWalletInfo();
                GetJulieWalletInfo();

                //GetJohnsWalletWithMoney(10000);
                //CardPayInFromJulieToJohnWallet();

            }
            catch (Exception ex)
            {
                lblDebug.Text += "Error !<br>";
                while (ex != null)
                {
                    lblDebug.Text += ex.Message + "<br>";
                    ex = ex.InnerException;
                }
            }
            

        }

       
        public void CreateNewClient()
        {
            //Attention, mieux vaux créer ses comptes sur l'interface web de leurs site, suis pas certain que ca fonctionne... ou j'ai confondu prod et sandbox


            lblDebug.Text += "<br>CreateNewClient<br>";

            ClientDTO client = api.Clients.Create("reactor01", "Reactor", "yannvasseur@reactor.fr");
            lblDebug.Text += "ok<br>";

            // you'll receive your passphrase here, note it down and keep in secret
            string passphrase = client.Passphrase;

            lblDebug.Text += "passphrase : " + passphrase + "<br>";


        }
                
        public void CreateJohnAccount()
        {
            lblDebug.Text += "Creating John account<br>";
            UserDTO john = null;
            try
            {
                john = api.Users.Get(_sandbox_John_UserId);
            }
            catch { }

            if (john == null)
            {               
                UserNaturalPostDTO user = new UserNaturalPostDTO("john.doe@sample.org", "John", "Doe", new DateTime(1975, 12, 21, 0, 0, 0), CountryIso.FR, CountryIso.FR);
                user.Occupation = "programmer";
                user.IncomeRange = 3;
                user.Address = "Some Address";

                john = api.Users.Create(user);

                lblDebug.Text += "John account created, userId : " + john.Id + "<br>";
            }
            else
            {
                lblDebug.Text += "John account is already created : " + john.Id + "<br>";
            }
        }
        public void CreateJulieAccount()
        {
            lblDebug.Text += "Creating Julie account<br>";
            UserDTO julie = null;
            try
            {
                julie = api.Users.Get(_sandbox_Julie_UserId);
            }
            catch { }

            if (julie == null)
            {
                UserNaturalPostDTO user = new UserNaturalPostDTO("julie.Mclowed@sample.org", "Julie", "Mclowed", new DateTime(1985, 10, 1, 0, 0, 0), CountryIso.FR, CountryIso.FR);
                user.Occupation = "fleuriste";
                user.IncomeRange = 3;
                user.Address = "Some Address";

                julie = api.Users.Create(user);

                lblDebug.Text += "Julie account created, userId : " + julie.Id + "<br>";
            }
            else
            {
                lblDebug.Text += "Julie account is already created : " + julie.Id + "<br>";
            }
        }

       

        public void GetAllUsers()
        {
            lblDebug.Text += "<br>GetAllUsers<br>";

            ListPaginated<UserDTO> users = api.Users.GetAll();

            lblDebug.Text += "Nbr Users : " + users.Count + "<br>";

            foreach (UserDTO user in users)
                lblDebug.Text += "user : " + user.Email + "<br>";

        }
       
        public void CreateJohnWallet()
        {
            lblDebug.Text += "<br>Creating John wallet<br>";

            UserDTO john = api.Users.Get(_sandbox_John_UserId);

            if (john != null)
            {   
                WalletDTO johnWallet= null;

                try
                {
                    johnWallet = api.Wallets.Get(_sandbox_John_WalletId);
                }
                catch { }

                if (johnWallet == null)
                {
                    List<string> owners = new List<string>();
                    owners.Add(john.Id);
                    WalletPostDTO wallet = new WalletPostDTO(owners, "Wallet de John", CurrencyIso.EUR);
                    johnWallet = api.Wallets.Create(wallet);

                    lblDebug.Text += "John's wallet created, walletId : " + johnWallet.Id + "<br>";
                }
                else
                {
                    lblDebug.Text += "John's wallet is already created : " + johnWallet.Id + "<br>";
                }
            }

        }
        public void CreateJulieWallet()
        {
            lblDebug.Text += "<br>Creating Julie wallet<br>";

            UserDTO julie = api.Users.Get(_sandbox_Julie_UserId);

            if (julie != null)
            {
                WalletDTO julieWallet = null;

                try
                {
                    julieWallet = api.Wallets.Get(_sandbox_Julie_WalletId);
                }
                catch { }

                if (julieWallet == null)
                {
                    List<string> owners = new List<string>();
                    owners.Add(julie.Id);
                    WalletPostDTO wallet = new WalletPostDTO(owners, "Wallet de Julie", CurrencyIso.EUR);                    
                    julieWallet = api.Wallets.Create(wallet);

                    lblDebug.Text += "Julie's wallet created, walletId : " + julieWallet.Id + "<br>";
                }
                else
                {
                    lblDebug.Text += "Julie's wallet is already created : " + julieWallet.Id + "<br>";
                }
            }

        }

        public WalletDTO GetJohnWallet()
        {
            return api.Wallets.Get(_sandbox_John_WalletId);
        }

        public void GetJohnWalletInfo()
        {
            lblDebug.Text += "<br>Get John Wallet Info : ";

            WalletDTO wallet = api.Wallets.Get(_sandbox_John_WalletId);

            lblDebug.Text += "id : " + wallet.Id + " CreationDate : " + wallet.CreationDate + " balance : " + wallet.Balance.Amount.ToString() + " " + wallet.Balance.Currency.ToString() + " Currency : " + wallet.Currency.ToString() + " Owners : " + wallet.Owners.Aggregate((a, b) => a + ", " + b) + "<br>";
                

        }
        public void GetJulieWalletInfo()
        {
            lblDebug.Text += "<br>Get Julie Wallet Info : ";

            WalletDTO wallet = api.Wallets.Get(_sandbox_Julie_WalletId);

            lblDebug.Text += "id : " + wallet.Id + " CreationDate : " + wallet.CreationDate + " balance : " + wallet.Balance.Amount.ToString() + " " + wallet.Balance.Currency.ToString() + " Currency : " + wallet.Currency.ToString() + " Owners : " + wallet.Owners.Aggregate((a, b) => a + ", " + b) + "<br>";


        }



        public static String GetPaylineCorrectRegistartionData(CardRegistrationDTO cardRegistration)
        {
            RestClient client = new RestClient(cardRegistration.CardRegistrationURL);

            RestRequest request = new RestRequest(Method.POST);
            request.AddParameter("data", cardRegistration.PreregistrationData);
            request.AddParameter("accessKeyRef", cardRegistration.AccessKey);
            request.AddParameter("cardNumber", "4970100000000154");
            request.AddParameter("cardExpirationDate", "1218");
            request.AddParameter("cardCvx", "123");

            IRestResponse response = client.Execute(request);

            String responseString = response.Content;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return responseString;
            else
                throw new Exception(responseString);
        }




        public WalletDTO GetJohnsWalletWithMoney(int amount)
        {

                //UserNaturalDTO john = this.GetJohn();
                //WalletDTO johnWallet = this.GetJohnWallet();

                ////1 :  Envoi une requete de création de carte (pré-enregistrement)
                //CardRegistrationPostDTO cardRegistrationPost = new CardRegistrationPostDTO(johnWallet.Owners[0], CurrencyIso.EUR);

                ////2 : récupère un objet carte depuis le serveur MangoPay (permet de prendre une url donnée par MangoPay qui est le serveur de token)
                //CardRegistrationDTO cardRegistration = this.api.CardRegistrations.Create(cardRegistrationPost);
                
                ////3 : créer un objet d'update carte 
                //CardRegistrationPutDTO cardRegistrationPut = new CardRegistrationPutDTO();

                ////4 : à partir de l'objet carte en 2, récupère un token donné par le server de token (url)
                //cardRegistrationPut.RegistrationData = GetPaylineCorrectRegistartionData(cardRegistration); //exemple : data=VLTIjgpf1ag15dmwRyhmORVzZJ3g4beuWtM-XsLxX42SJIgXGyCSMWLYR8VvJ7xd_pUJJxM61mjOtNI_u2eb6A645Q1xtacJUXE8ZAEkH_SIexemLrNF98M4OD2KHeYV0ftIYwFxOdfmDQ5GtM_cIg
                //lblDebug.Text += "RegistrationData : " + cardRegistrationPut.RegistrationData;
            
                ////5 : update la carte sur MangoPay
                //cardRegistration = this.api.CardRegistrations.Update(cardRegistrationPut, cardRegistration.Id);

                ////6 : récupère depuis MangoPay la fameuse carte. 
                //CardDTO card = this.api.Cards.Get(cardRegistration.CardId);

                ////// create pay-in CARD DIRECT
                ////PayInCardDirectPostDTO payIn = new PayInCardDirectPostDTO(cardRegistration.UserId, cardRegistration.UserId,
                ////    new Money { Amount = amount, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR },
                ////    BaseTest._johnsWalletWithMoney.Id, "http://test.com", card.Id);

                ////if (card.CardType == CardType.CB || card.CardType == CardType.VISA || card.CardType == CardType.MASTERCARD || card.CardType == CardType.CB_VISA_MASTERCARD)
                ////    payIn.CardType = CardType.CB_VISA_MASTERCARD;
                ////else if (card.CardType == CardType.AMEX)
                ////    payIn.CardType = CardType.AMEX;

                ////// create Pay-In
                ////this.Api.PayIns.CreateCardDirect(payIn);


                return null; //WalletDTO wallet 
        }




        public void CardPayInFromJulieToJohnWallet()
        {
            lblDebug.Text += "<br>CardPayInFromJulieToJohnWallet<br>";



            // create pay-in PRE-AUTHORIZED DIRECT
            //PayInPreauthorizedDirectPostDTO payIn = new PayInPreauthorizedDirectPostDTO(_sandbox_John_UserId, new Money { Amount = 10000, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR }, wallet.Id, cardPreAuthorization.Id);
               




            //string authorId = _sandbox_Julie_UserId;

            //Money debitedFunds = new Money();
            //debitedFunds.Amount = 1000;
            //debitedFunds.Currency = CurrencyIso.EUR;

            //Money fees = new Money();
            //fees.Amount = 0;
            //fees.Currency = CurrencyIso.EUR;

            //string creditedWalletId = _sandbox_Julie_WalletId;
            //string returnURL = "http=//nopCommerce.reactor.fr/";
            
            //PayInCardWebPostDTO payInCardWebPostDTO = new PayInCardWebPostDTO(authorId, debitedFunds, fees, creditedWalletId, returnURL, CountryIso.FR, CardType.CB_VISA_MASTERCARD);


            //try
            //{
            //    PayInCardWebDTO payInCardWebDTO = api.PayIns.CreateCardWeb(payInCardWebPostDTO);


            //    if (payInCardWebDTO != null)
            //    {
            //        lblDebug.Text += "Operation done";
            //    }

            //}
            //catch (Newtonsoft.Json.JsonSerializationException ex)
            //{
            //    lblDebug.Text += "Error :" + ex.Message;
            //}
            
        }
       
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public void Sample()
        {

            //// get some Natural user
            //UserNaturalDTO user = api.Users.GetNatural("");

            //// create update entity
            //UserNaturalPutDTO userPut = new UserNaturalPutDTO
            //{
            //    Tag = user.Tag,
            //    Email = user.Email,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName + " - CHANGED",
            //    Address = user.Address,
            //    Birthday = user.Birthday,
            //    Nationality = user.Nationality,
            //    CountryOfResidence = user.CountryOfResidence,
            //    Occupation = user.Occupation,
            //    IncomeRange = user.IncomeRange
            //};

            //// save updated user
            //UserNaturalDTO userSaved = api.Users.UpdateNatural(userPut, user.Id);

            //// get his bank accounts
            //Pagination pagination = new Pagination(2, 10); // get 2nd page, 10 items per page
            //ListPaginated<BankAccountDTO> accounts = api.Users.GetBankAccounts(user.Id, pagination);

            //// get all users (with pagination)
            //Pagination pagination = new Pagination(1, 8); // get 1st page, 8 items per page
            //ListPaginated<UserDTO> users = api.Users.GetAll(pagination);

        }

        public void SampleUsage()
        {
            lblDebug.Text += "<br>SampleUsage<br>";

            



            //// get some Natural user
            //UserNaturalDTO user = api.Users.GetNatural("");

            //// create update entity
            //UserNaturalPutDTO userPut = new UserNaturalPutDTO
            //{
            //    Tag = user.Tag,
            //    Email = user.Email,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName + " - CHANGED",
            //    Address = user.Address,
            //    Birthday = user.Birthday,
            //    Nationality = user.Nationality,
            //    CountryOfResidence = user.CountryOfResidence,
            //    Occupation = user.Occupation,
            //    IncomeRange = user.IncomeRange
            //};

            //// save updated user
            //UserNaturalDTO userSaved = api.Users.UpdateNatural(userPut, user.Id);

            //// get his bank accounts
            //Pagination pagination = new Pagination(2, 10); // get 2nd page, 10 items per page
            //ListPaginated<BankAccountDTO> accounts = api.Users.GetBankAccounts(user.Id, pagination);

            //// get all users (with pagination)
            //Pagination pagination = new Pagination(1, 8); // get 1st page, 8 items per page
            //ListPaginated<UserDTO> users = api.Users.GetAll(pagination);

        }
    }
}