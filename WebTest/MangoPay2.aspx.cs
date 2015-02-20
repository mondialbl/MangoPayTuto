using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MangoPay.SDK;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Core.Enumerations;

using Helper;

namespace WebTest
{
    public partial class MangoPay2 : System.Web.UI.Page
    {
        private MangoPayApi api;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblDebug.Text = "";


                lblDebug.Text += "Test MangoPay...SANDBOX<br>";
                lblDebug.Text += "..............................................................................................................<br><br>";

                lblDebug.Text += "Initiating MangoPayApi...<br>";
                MangoPayApi api = new MangoPayApi();

                api.Config.ClientId = "reactor03";
                api.Config.ClientPassword = "r9YNsNSwOHB8YT8EWrmUD3SO2pJY2PNk88O6CGazCHL0Ny4ddK";

                lblDebug.Text += "api.Config.BaseUrl : " + api.Config.BaseUrl + "<br>";
                lblDebug.Text += "api.Config.ClientId : " + api.Config.ClientId + "<br>";
                lblDebug.Text += "api.Config.ClientPassword : " + api.Config.ClientPassword + "<br><br>";

                /*********************************************************************************************************/

                lblDebug.Text += Samples.GetJohnWalletInfo(api);
                lblDebug.Text += Samples.GetJulieWalletInfo(api);

                UserNaturalDTO userJohn = Samples.GetJohn(api);
                WalletDTO johnWallet = Samples.GetJohnWallet(api);
                ReactorCreditCard fakeVisa = ReactorCreditCard.FakeVisa1();
                Ninja.AddMoneyOnUserWalletByCreditCard(api, userJohn, johnWallet, fakeVisa, 100);

                lblDebug.Text += Samples.GetJohnWalletInfo(api);

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