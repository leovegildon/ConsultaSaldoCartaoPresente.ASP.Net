using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Web;
using System.Net;
using System.IO;

namespace ConsultaSaldoCartaoPresente
{
    public partial class ConsultaSaldo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public class Details
        {
            public string productCategoryCode { get; set; }
            public string statusCode { get; set; }
            public string specVersion { get; set; }
        }

        public class Header
        {
            public string signature { get; set; }
            public Details details { get; set; }
        }

        public class AdditionalTxnFields
        {
            public string productId { get; set; }
            public string balanceAmount { get; set; }
            public string transactionUniqueId { get; set; }
            public string correlatedTransactionUniqueId { get; set; }
        }

        public class Transaction
        {
            public string primaryAccountNumber { get; set; }
            public string processingCode { get; set; }
            public string transactionAmount { get; set; }
            public string transmissionDateTime { get; set; }
            public string systemTraceAuditNumber { get; set; }
            public string localTransactionTime { get; set; }
            public string localTransactionDate { get; set; }
            public string merchantCategoryCode { get; set; }
            public string pointOfServiceEntryMode { get; set; }
            public string acquiringInstitutionIdentifier { get; set; }
            public string retrievalReferenceNumber { get; set; }
            public string authIdentificationResponse { get; set; }
            public string responseCode { get; set; }
            public string merchantTerminalId { get; set; }
            public string merchantIdentifier { get; set; }
            public string transactionCurrencyCode { get; set; }
            public AdditionalTxnFields additionalTxnFields { get; set; }
        }

        public class Response
        {
            public Header header { get; set; }
            public Transaction transaction { get; set; }
        }

        public class RootObject
        {
            public Response response { get; set; }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string data = DateTime.Now.ToString("yyMMdd");
            string hora = DateTime.Now.ToString("HHmmss");
            string dataHoraUtc = DateTime.UtcNow.ToString("yyMMddHHmmss");

            //Corrige: "Erro ao criar canal seguro para SSL/TLS."
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.Expect100Continue = true;



            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://webpos.blackhawk-net.com:8443/transactionManagement/v2/transaction");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string jsonEnvio = "{" +
   "\"request\":{" +
      "\"header\":{" +
         "\"signature\":\"BHNUMS\"," +
         "\"details\":{" +
            "\"specVersion\":\"43\"," +
            "\"productCategoryCode\":\"01\"" +
         "}" +
      "}," +
      "\"transaction\":{" +
         "\"primaryAccountNumber\":" + "\"" + txtUpc.Text.Substring(13, 19) + "\"" + "," +
         "\"processingCode\":\"315400\"," +
         "\"transmissionDateTime\":" + "\"" + dataHoraUtc + "\"" + "," +
         "\"systemTraceAuditNumber\":\"001099\"," +
         "\"localTransactionTime\":" + "\"" + hora + "\"" + "," +
         "\"localTransactionDate\":" + "\"" + data + "\"" + "," +
         "\"merchantCategoryCode\":\"5045\"," +
         "\"pointOfServiceEntryMode\":\"041\"," +
         "\"acquiringInstitutionIdentifier\":\"60300004947\"," +
         "\"retrievalReferenceNumber\":\"000000000845\"," +
         "\"merchantTerminalId\":\"01006     001   \"," +
         "\"merchantIdentifier\":\"60300004947    \"," +
         "\"merchantLocation\":\"Nome Da Rua e Numero  Sao Paulo    SP BR\"," +
         "\"transactionCurrencyCode\":\"986\"," +
         "\"additionalTxnFields\":{" +
            "\"productId\":\"505164408154\"" +
         "}" +
      "}" +
   "}" +
"}";

                streamWriter.Write(jsonEnvio);
            }


            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                //System.IO.StreamWriter escrever5 = new System.IO.StreamWriter("LogJsonRetorno.txt", true);
                //escrever5.WriteLine("*******************************************************");
                //escrever5.WriteLine(DateTime.Now);
                //escrever5.WriteLine("*******************************************************");
                //escrever5.WriteLine(result.ToString());
                //escrever5.WriteLine("****************************************************************************************************************************");
                //escrever5.Close();
                //MessageBox.Show(result.ToString());
                RootObject dados = JsonConvert.DeserializeObject<RootObject>(result);
                txtUpc.Text = "";
                string reais = dados.response.transaction.additionalTxnFields.balanceAmount.Substring(8, 3);
                string centavos = dados.response.transaction.additionalTxnFields.balanceAmount.Substring(11, 2);
                txtSaldo.Text = "R$" + reais + "," + centavos;
                txtProduto.Text = dados.response.transaction.additionalTxnFields.productId;
                txtCartao.Text = dados.response.transaction.primaryAccountNumber;
                txtRetorno.Text = dados.response.transaction.responseCode;

            }
                

        }
    }
}