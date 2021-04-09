using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Web;
using System.Net;
using System.IO;

namespace ConsultaSaldoCartaoPresente
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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



        private void button1_Click(object sender, EventArgs e)
        {

            
            //Corrige: "Erro ao criar canal seguro para SSL/TLS."
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.Expect100Continue = true;


            var httpWebRequest = (HttpWebRequest)WebRequest.Create(txtUrl.Text);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                
                string jsonEnvio = "{"+  
   "\"request\":{"+  
      "\"header\":{"+  
         "\"signature\":\"BHNUMS\","+
         "\"details\":{"+  
            "\"specVersion\":\"43\","+
            "\"productCategoryCode\":\"01\""+
         "}"+
      "},"+
      "\"transaction\":{"+  
         "\"primaryAccountNumber\":\"6039533504002033669\","+
         "\"processingCode\":\"315400\","+
         "\"transmissionDateTime\":\"191224112701\","+
         "\"systemTraceAuditNumber\":\"001099\","+
         "\"localTransactionTime\":\"112701\","+
         "\"localTransactionDate\":\"191224\","+
         "\"merchantCategoryCode\":\"5045\","+
         "\"pointOfServiceEntryMode\":\"041\","+
         "\"acquiringInstitutionIdentifier\":\"00000000000\","+
         "\"retrievalReferenceNumber\":\"000000000845\","+
         "\"merchantTerminalId\":\"01006     001   \","+
         "\"merchantIdentifier\":\"60300004947    \","+
         "\"merchantLocation\":\"Nome Da Rua e Numero  Sao Paulo    SP BR\","+
         "\"transactionCurrencyCode\":\"986\","+
         "\"additionalTxnFields\":{"+  
            "\"productId\":\"505164408154\""+
         "}"+
      "}"+
   "}"+
"}";

                streamWriter.Write(richJsonEnvio.Text);
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    System.IO.StreamWriter escrever5 = new System.IO.StreamWriter("LogJsonRetorno.txt", true);
                    escrever5.WriteLine(result.ToString());
                    escrever5.Close();
                    MessageBox.Show(result.ToString());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}
