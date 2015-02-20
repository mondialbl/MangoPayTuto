﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

using MangoPay.SDK;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Core.Enumerations;

 using Helper;

namespace WebTest
{
    public partial class Tuto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblDebug.Text = "";
                lblDebug.Text += "Tuto MangoPay...SANDBOX<br>";
                lblDebug.Text += "..............................................................................................................<br><br>";
                lblDebug.Text += "Initiating MangoPayApi...<br>";
                
                MangoPayApi api = new MangoPayApi();
                api.Config.ClientId = ConfigurationManager.AppSettings["SandBoxClientId"];
                api.Config.ClientPassword = ConfigurationManager.AppSettings["SandBoxClientPassword"];
                lblDebug.Text += "api.Config.BaseUrl : " + api.Config.BaseUrl + "<br>";
                lblDebug.Text += "api.Config.ClientId : " + api.Config.ClientId + "<br>";
                lblDebug.Text += "api.Config.ClientPassword : " + api.Config.ClientPassword + "<br><br>";
                
                //Start playing
                UserNaturalDTO userJohn = Samples.CreateUserJohn(api);
                Samples._sandbox_John_UserId = userJohn.Id;
                UserNaturalDTO userJulie = Samples.CreateUserJulie(api);
                Samples._sandbox_Julie_UserId = userJulie.Id;
                WalletDTO walletJohn = Samples.CreateJohnWallet(api);
                Samples._sandbox_John_WalletId = walletJohn.Id;
                WalletDTO walletJulie = Samples.CreateJulieWallet(api);
                Samples._sandbox_Julie_WalletId = walletJulie.Id;
                ReactorCreditCard fakeVisa = ReactorCreditCard.FakeVisa1();
                Ninja.AddMoneyOnUserWalletByCreditCard(api, userJohn, walletJohn, fakeVisa, 100);
                Ninja.AddMoneyOnUserWalletByCreditCard(api, userJulie, walletJulie, fakeVisa, 200);

                lblDebug.Text += Samples.GetJohnWalletInfo(api);
                lblDebug.Text += Samples.GetJulieWalletInfo(api);

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
    }
}


